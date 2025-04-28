const stripe = Stripe('pk_test_51RFotYCSOPGzLKGt8El4ZheVsf9Aik9U7cuP7u5o6YQJWDRljL3S5TBVSaseF65ufFTKIrxH702mB1wg5wuRN3s000ehWCyJVG');

document.getElementById('booking-form')?.addEventListener('submit', async function(e) {
    e.preventDefault();
    
    const tripId = document.getElementById('TripId').value;
    const numberOfSeats = document.getElementById('NumberOfSeats').value;
    const totalAmount = parseFloat(document.getElementById('TotalPrice').value);

    try {
        const response = await fetch('/Payment/CreateCheckoutSession', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                tripId: tripId,
                numberOfSeats: parseInt(numberOfSeats),
                totalAmount: totalAmount,
                currency: 'usd'
            })
        });

        const session = await response.json();
        const result = await stripe.redirectToCheckout({
            sessionId: session.id
        });

        if (result.error) {
            console.error('Stripe Error:', result.error);
            alert(result.error.message);
        }
    } catch (error) {
        console.error('Payment Error:', error);
        alert('An error occurred during payment processing.');
    }
});
