function deleteOrder(orderId, button) {
    fetch(`/Admin/Orders/DeleteOrder?orderId=${orderId}`, { method: 'POST' })
        .then(result => {
            console.log(result);
            deleteRow(button);
        })
        .error(err => {
            console.error(err);
        });
}

function deleteRow(button) {
    let row = button.parentNode.parentNode;
    row.remove();
}