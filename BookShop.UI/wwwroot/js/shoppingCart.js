let tbody = document.getElementById('tbody');
let totalPrice = document.getElementById('totalPrice');
let container = document.getElementById('shoppingCartContainer');

if (tbody.childElementCount <= 1) {
    showEmptyBasket();
}

function updateItem(productIdVal, userIdVal, itemNumberVal) {
    let input = document.getElementById(`count-${itemNumberVal}`);
    let countVal = input.value;

    if (countVal < 1) {
        input.value = 1;
        countVal = 1;
    }

    const item = {
        productId: productIdVal,
        userId: userIdVal,
        count: countVal
    };

    fetch('/Customer/ShoppingCart/SetShoppingCartItem', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(result => {
            totalPrice.innerHTML = `$${result}`;
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

function deleteItem(productId, userId, button) {
    fetch(`/Customer/ShoppingCart/DeleteShoppingCartItem?userId=${userId}&productId=${productId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(result => {
            row = button.parentNode.parentNode;
            row.remove();
            if (tbody.childElementCount <= 1) {
                showEmptyBasket();
            } else {
                totalPrice.innerHTML = `$${result}`;
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

function clearItems(userId) {
    fetch(`/Customer/ShoppingCart/Clear?userId=${userId}`, {
        method: 'POST'
    })
        .then(result => {
            showEmptyBasket();
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

function showEmptyBasket() {
    container.innerHTML =
        '<div class="text-center">' +
            '<h3>Your basket is empty! Fill it before submit your order.</h3>' + 
        '</div>';
}