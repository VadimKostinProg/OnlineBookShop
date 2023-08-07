let button = document.getElementById("submitButton");

button.addEventListener('click', async function () {
    let userIdVal = document.getElementById("userIdInput").value;
    let productIdVal = document.getElementById("productIdInput").value;
    let countVal = document.getElementById("countInput").value;

    const item = {
        userId: userIdVal,
        productId: productIdVal,
        count: countVal
    };

    fetch('/Customer/ShoppingCart/SetShoppingCartItem', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
    .then(result => {
        console.log(result);
        window.location.href = '/';
    })
    .catch(error => {
        console.error('Error:', error);
    });
});