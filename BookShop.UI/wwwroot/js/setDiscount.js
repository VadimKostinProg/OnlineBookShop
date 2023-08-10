function categoryChanged(categorySelect) {
    let categoryId = categorySelect.value;

    fetch(`/Admin/Products/GetProductsByCategory?categoryId=${categoryId}`, { method: 'GET' })
        .then(response => response.json())
        .then(result => {
            let select = document.getElementById('productsSelect');

            removeOptions(select);

            for (let i = 0; i < result.length; i++) {
                var option = document.createElement('option');
                option.value = result[i].id;
                option.text = result[i].title;
                select.appendChild(option);
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

function removeOptions(select) {
    while (select.options.length > 0) {
        select.remove(0);
    }
}