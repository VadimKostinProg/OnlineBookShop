function deleteAllDiscounts(productId) {
    fetch(`/Admin/Discount/DeleteAllDiscounts?productId=${productId}`, { method: 'POST' })
        .then(result => {
            console.log(result);
            deleteGeneralRow(productId);
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

function deleteDiscount(productId, count) {
    fetch(`/Admin/Discount/DeleteDiscount?productId=${productId}&count=${count}`, { method: 'POST' })
        .then(result => {
            console.log(result);
            deleteDiscountRow(productId, count);
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

function deleteGeneralRow(productId) {
    let row = document.getElementById(`${productId}-row`);
    row.remove();
}

function deleteDiscountRow(productId, count) {
    let row = document.getElementById(`${productId}-${count}-row`);
    row.remove();

    let nestedTable = document.getElementById(`${productId}-nestedTable`);

    if (nestedTable.rows.length == 1) {
        deleteGeneralRow(productId);
    }
}