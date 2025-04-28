// Booking System JavaScript
class BookingSystem {
    constructor() {
        // Initialize Stripe
        this.stripe = Stripe('pk_test_51RFotYCSOPGzLKGt8El4ZheVsf9Aik9U7cuP7u5o6YQJWDRljL3S5TBVSaseF65ufFTKIrxH702mB1wg5wuRN3s000ehWCyJVG');
        
        this.initializeDatePicker();
        this.initializeSearchForm();
        this.initializeBookingModal();
        this.initializeTermsCheckbox();
        this.attachEventListeners();
        this.handleMissingImages();
        
        // Add loading indicator
        this.loadingOverlay = this.createLoadingOverlay();
    }

    // Add loading overlay
    createLoadingOverlay() {
        const overlay = document.createElement('div');
        overlay.className = 'loading-overlay d-none';
        overlay.innerHTML = `
            <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
            <p class="mt-2">Processing your request...</p>
        `;
        document.body.appendChild(overlay);
        return overlay;
    }

    showLoading(message = 'Processing your request...') {
        this.loadingOverlay.querySelector('p').textContent = message;
        this.loadingOverlay.classList.remove('d-none');
    }

    hideLoading() {
        this.loadingOverlay.classList.add('d-none');
    }

    // Improve handleBookingFormSubmit with better error handling and user feedback
    handleBookingFormSubmit(event) {
        event.preventDefault();
        
        if (!this.bookingForm.checkValidity()) {
            event.stopPropagation();
            this.bookingForm.classList.add('was-validated');
            this.showNotification('Please fill in all required fields.', 'warning');
            return;
        }

        const formData = new FormData(this.bookingForm);
        const tripId = formData.get('TripId');
        const numberOfSeats = formData.get('NumberOfSeats');
        const totalAmount = parseFloat(document.getElementById('TotalPrice').value);
        
        // Trigger the payment process from site.js
        if (typeof window.handlePayment === 'function') {
            window.handlePayment(tripId, numberOfSeats, totalAmount);
            this.bookingModal.hide();
        } else {
            this.showNotification('Payment system is not available', 'error');
        }
    }

    // Add notification system
    showNotification(message, type = 'info') {
        const toast = document.createElement('div');
        toast.className = `toast align-items-center text-white bg-${type} border-0`;
        toast.setAttribute('role', 'alert');
        toast.setAttribute('aria-live', 'assertive');
        toast.setAttribute('aria-atomic', 'true');
        
        toast.innerHTML = `
            <div class="d-flex">
                <div class="toast-body">
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        `;
        
        const container = document.querySelector('.toast-container') || this.createToastContainer();
        container.appendChild(toast);
        
        const bsToast = new bootstrap.Toast(toast);
        bsToast.show();
        
        toast.addEventListener('hidden.bs.toast', () => {
            toast.remove();
        });
    }

    createToastContainer() {
        const container = document.createElement('div');
        container.className = 'toast-container position-fixed bottom-0 end-0 p-3';
        document.body.appendChild(container);
        return container;
    }

    initializeDatePicker() {
        const today = new Date().toISOString().split('T')[0];
        const tripDateInput = document.getElementById('TripDate');
        if (tripDateInput) {
            tripDateInput.value = today;
            tripDateInput.min = today;
        }
    }

    initializeSearchForm() {
        this.searchForm = document.getElementById('trip-search-form');
        this.searchButton = document.getElementById('search-button');
        this.clearSearchBtn = document.getElementById('clear-search-btn');
        this.searchResultsSection = document.getElementById('search-results-section');
        this.searchResultsContainer = document.getElementById('search-results-container');
        this.availableTripsSection = document.getElementById('available-trips-section');
    }

    initializeBookingModal() {
        const modalElement = document.getElementById('bookingModal');
        if (modalElement) {
            this.bookingModal = new bootstrap.Modal(modalElement);
            this.termsCheckbox = document.getElementById('terms');
            this.payButton = document.getElementById('pay-button');
            this.seatsDropdown = document.getElementById('NumberOfSeats');
            this.bookingForm = document.getElementById('booking-form');
        } else {
            console.error('Booking modal element not found');
        }
    }

    initializeTermsCheckbox() {
        if (this.termsCheckbox && this.payButton) {
            this.termsCheckbox.addEventListener('change', () => {
                this.payButton.disabled = !this.termsCheckbox.checked;
                this.updatePayButtonStyle();
            });
        }
    }

   
    
   

    updateTripAvailability(tripId, availableSeats) {
        console.log(`Updating trip availability: Trip ${tripId} has ${availableSeats} seats available`);
        
        // Update in search results
        const searchResultRow = document.querySelector(`#search-results-tbody .trip-row[data-trip-id="${tripId}"]`);
        if (searchResultRow) {
            const seatsCell = searchResultRow.querySelector('td:nth-child(5) span');
            if (seatsCell) {
                seatsCell.textContent = availableSeats;
                seatsCell.className = availableSeats > 5 ? "text-success" : availableSeats > 0 ? "text-warning" : "text-danger";
            }
            
            // Update book button
            const bookButton = searchResultRow.querySelector('.book-trip-btn');
            if (bookButton) {
                if (availableSeats <= 0) {
                    bookButton.disabled = true;
                    bookButton.classList.remove('btn-primary');
                    bookButton.classList.add('btn-outline-secondary');
                    bookButton.textContent = 'Sold Out';
                } else {
                    bookButton.disabled = false;
                    bookButton.classList.remove('btn-outline-secondary');
                    bookButton.classList.add('btn-primary');
                    bookButton.textContent = 'Book Now';
                }
                bookButton.setAttribute('data-available', availableSeats);
            }
        }

        // Update in available trips table
        const availableTripRow = document.querySelector(`#trips-table .trip-row[data-trip-id="${tripId}"]`);
        if (availableTripRow) {
            const seatsCell = availableTripRow.querySelector('td:nth-child(5) span');
            if (seatsCell) {
                seatsCell.textContent = availableSeats;
                seatsCell.className = availableSeats > 5 ? "text-success" : availableSeats > 0 ? "text-warning" : "text-danger";
            }
            
            // Update book button
            const bookButton = availableTripRow.querySelector('.book-trip-btn');
            if (bookButton) {
                if (availableSeats <= 0) {
                    bookButton.disabled = true;
                    bookButton.classList.remove('btn-primary');
                    bookButton.classList.add('btn-outline-secondary');
                    bookButton.textContent = 'Sold Out';
                } else {
                    bookButton.disabled = false;
                    bookButton.classList.remove('btn-outline-secondary');
                    bookButton.classList.add('btn-primary');
                    bookButton.textContent = 'Book Now';
                }
                bookButton.setAttribute('data-available', availableSeats);
            }
        }

        // Update in booking modal if open
        const modalTripId = document.getElementById('TripId').value;
        if (modalTripId === tripId) {
            document.getElementById('modal-seats').textContent = availableSeats;
            this.updateSeatsDropdown(availableSeats);
        }
    }

    addNewTripToTable(tripId, fromStation, toStation, tripDate, vehicle, availableSeats, price) {
        // Create new row for available trips table
        const newRow = document.createElement('tr');
        newRow.className = 'trip-row';
        newRow.setAttribute('data-trip-id', tripId);
        newRow.setAttribute('data-from-station-id', fromStation);
        newRow.setAttribute('data-to-station-id', toStation);
        newRow.setAttribute('data-trip-date', tripDate);
        
        newRow.innerHTML = `
            <td>${fromStation}</td>
            <td>${toStation}</td>
            <td>${tripDate}</td>
            <td>${vehicle}</td>
            <td><span class="${availableSeats > 5 ? "text-success" : availableSeats > 0 ? "text-warning" : "text-danger"}">${availableSeats}</span></td>
            <td>$${price}</td>
            <td>
                <button class="btn btn-sm btn-primary book-trip-btn"
                        data-trip-id="${tripId}"
                        data-from="${fromStation}"
                        data-to="${toStation}"
                        data-date="${tripDate}"
                        data-price="${price}"
                        data-available="${availableSeats}"
                        data-vehicle="${vehicle}">
                    Book Now
                </button>
            </td>
        `;

        // Add to available trips table
        const tbody = document.getElementById('trips-table').querySelector('tbody');
        if (tbody) {
            tbody.insertBefore(newRow, tbody.firstChild);
            this.attachBookButtonListeners([newRow.querySelector('.book-trip-btn')]);
        }
    }

    updatePayButtonStyle() {
        if (this.payButton) {
            if (this.payButton.disabled) {
                this.payButton.classList.add('btn-secondary');
                this.payButton.classList.remove('btn-primary');
            } else {
                this.payButton.classList.add('btn-primary');
                this.payButton.classList.remove('btn-secondary');
            }
        }
    }

    handleSearch(event) {
        event.preventDefault();
        this.showLoadingState();

        const searchCriteria = this.getSearchCriteria();
        const matchingTrips = this.findMatchingTrips(searchCriteria);
        this.displaySearchResults(matchingTrips);

        this.resetSearchButton();
    }

    getSearchCriteria() {
        return {
            fromStationId: document.getElementById('FromStationId').value,
            toStationId: document.getElementById('TOStationId').value,
            tripDate: document.getElementById('TripDate').value
        };
    }

    findMatchingTrips(criteria) {
        const tripRows = document.querySelectorAll('.trip-row');
        const matchingTrips = [];
        const currentDate = new Date().toISOString().split('T')[0];

        tripRows.forEach(row => {
            const rowFromStationId = row.getAttribute('data-from-station-id');
            const rowToStationId = row.getAttribute('data-to-station-id');
            const rowTripDate = row.getAttribute('data-trip-date');

            if (rowTripDate < currentDate) return;

            const fromMatch = !criteria.fromStationId || rowFromStationId === criteria.fromStationId;
            const toMatch = !criteria.toStationId || rowToStationId === criteria.toStationId;
            const dateMatch = !criteria.tripDate || rowTripDate === criteria.tripDate;

            if (fromMatch && toMatch && dateMatch) {
                matchingTrips.push(row.cloneNode(true));
            }
        });

        return matchingTrips;
    }

    displaySearchResults(matchingTrips) {
        this.searchResultsContainer.innerHTML = '';

        if (matchingTrips.length > 0) {
            this.createResultsTable(matchingTrips);
        } else {
            this.showNoResults();
        }

        this.searchResultsSection.style.display = 'block';
        this.availableTripsSection.style.display = 'none';
    }

    createResultsTable(matchingTrips) {
        const resultTable = document.createElement('div');
        resultTable.className = 'card border-0 shadow-sm';
        resultTable.innerHTML = `
            <div class="table-responsive">
                <table class="table table-hover mb-0">
                    <thead class="table-light">
                        <tr>
                            <th>From</th>
                            <th>To</th>
                            <th>Date & Time</th>
                            <th>Vehicle</th>
                            <th>Available Seats</th>
                            <th>Price</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody id="search-results-tbody">
                    </tbody>
                </table>
            </div>
        `;

        this.searchResultsContainer.appendChild(resultTable);
        const tbody = document.getElementById('search-results-tbody');
        matchingTrips.forEach(trip => tbody.appendChild(trip));
    }

    showNoResults() {
        const noResults = document.getElementById('no-results-template').content.cloneNode(true);
        this.searchResultsContainer.appendChild(noResults);
    }

    showLoadingState() {
        this.searchButton.disabled = true;
        this.searchButton.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Searching...';
    }

    resetSearchButton() {
        this.searchButton.disabled = false;
        this.searchButton.innerHTML = '<i class="fas fa-search me-2"></i>Search';
    }

    handleBooking(tripData) {
        console.log('Opening booking modal with data:', tripData);
        this.updateModalContent(tripData);
        this.updateSeatsDropdown(tripData.available);
        
        // Make sure the modal is properly initialized before showing it
        if (this.bookingModal) {
            this.bookingModal.show();
        } else {
            console.error('Booking modal not initialized');
            // Try to reinitialize the modal
            this.initializeBookingModal();
            if (this.bookingModal) {
                this.bookingModal.show();
            }
        }
    }

    updateModalContent(tripData) {
        try {
            const elements = {
                'TripId': tripData.id,
                'modal-from': tripData.from,
                'modal-to': tripData.to,
                'modal-date': tripData.date,
                'modal-price': '$' + tripData.price,
                'modal-seats': tripData.available,
                'modal-vehicle': tripData.vehicle,
                'modal-total-price': '$' + tripData.price,
                'TotalPrice': tripData.price
            };

            for (const [id, value] of Object.entries(elements)) {
                const element = document.getElementById(id);
                if (element) {
                    if (element.tagName === 'INPUT') {
                        element.value = value;
                    } else {
                        element.textContent = value;
                    }
                } else {
                    console.error(`Element with id '${id}' not found`);
                }
            }
        } catch (error) {
            console.error('Error updating modal content:', error);
            this.showNotification('Error loading booking details', 'error');
        }
    }

    updateSeatsDropdown(availableSeats) {
        if (this.seatsDropdown) {
            this.seatsDropdown.innerHTML = '';
            for (let i = 1; i <= availableSeats; i++) {
                const option = document.createElement('option');
                option.value = i;
                option.textContent = i + (i === 1 ? ' Seat' : ' Seats');
                this.seatsDropdown.appendChild(option);
            }
        }
    }

    attachEventListeners() {
        if (this.searchForm) {
            this.searchForm.addEventListener('submit', (e) => this.handleSearch(e));
        }

        if (this.clearSearchBtn) {
            this.clearSearchBtn.addEventListener('click', () => {
                this.searchResultsSection.style.display = 'none';
                this.availableTripsSection.style.display = 'block';
                this.searchResultsContainer.innerHTML = '';
                this.searchForm.reset();
                this.initializeDatePicker();
            });
        }

        // Attach book button listeners
        this.attachBookButtonListeners();

        // Handle booking form submission
        if (this.bookingForm) {
            this.bookingForm.addEventListener('submit', (e) => this.handleBookingFormSubmit(e));
        }

        // Handle modal close - remove SignalR code
        const bookingModalElement = document.getElementById('bookingModal');
        if (bookingModalElement) {
            bookingModalElement.addEventListener('hidden.bs.modal', () => {
                // Reset form and UI elements when modal is closed
                if (this.bookingForm) {
                    this.bookingForm.reset();
                }
                if (this.payButton) {
                    this.payButton.disabled = true;
                    this.updatePayButtonStyle();
                }
            });
        }
    }

    handleBookingFormSubmit(event) {
        event.preventDefault();
        
        if (!this.bookingForm.checkValidity()) {
            event.stopPropagation();
            this.bookingForm.classList.add('was-validated');
            return;
        }

        // Get form data
        const formData = new FormData(this.bookingForm);
        const tripId = formData.get('TripId');
        const numberOfSeats = formData.get('NumberOfSeats');
        const totalAmount = parseFloat(document.getElementById('TotalPrice').value);
        
        // Show loading state
        this.payButton.disabled = true;
        this.payButton.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Processing...';
        
       
    }
    // Add this new method to update seat count after booking
    // Remove the semicolon after the parameters
    attachBookButtonListeners(elements = null){
        const bookButtons = elements || document.querySelectorAll('.book-trip-btn');
        console.log('Attaching listeners to', bookButtons.length, 'book buttons');
        
        bookButtons.forEach(button => {
            // Remove any existing event listeners to prevent duplicates
            const newButton = button.cloneNode(true);
            button.parentNode.replaceChild(newButton, button);
            
            newButton.addEventListener('click', (event) => {
                event.preventDefault();
                console.log('Book button clicked');
                
                const tripData = {
                    id: newButton.getAttribute('data-trip-id'),
                    from: newButton.getAttribute('data-from'),
                    to: newButton.getAttribute('data-to'),
                    date: newButton.getAttribute('data-date'),
                    price: newButton.getAttribute('data-price'),
                    available: newButton.getAttribute('data-available'),
                    vehicle: newButton.getAttribute('data-vehicle')
                };
                
                console.log('Trip data:', tripData);
                this.handleBooking(tripData);
            });
        });
    }

    handleMissingImages = () => {
        // Find all images with missing sources
        const images = document.querySelectorAll('img');
        images.forEach(img => {
            img.onerror = function() {
                // Replace with a placeholder or hide the image
                this.style.display = 'none';
                const parent = this.parentElement;
                if (parent) {
                    const placeholder = document.createElement('div');
                    placeholder.className = 'bg-light text-center py-5';
                    placeholder.innerHTML = '<i class="fas fa-image fa-4x text-muted"></i><p class="mt-2 text-muted">Image not available</p>';
                    parent.appendChild(placeholder);
                }
            };
        });
        }
}
document.addEventListener('DOMContentLoaded', function() {
    console.log('DOM loaded, initializing BookingSystem');
    window.bookingSystem = new BookingSystem();
});