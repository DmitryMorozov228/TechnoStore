function declOfNum(number, titles) {
    number = number % 100;
    if (number >= 11 && number <= 19)
    {
        return titles[2];
    }
    var i = number % 10;
    switch (i)
    {
        case 1:
            return titles[0];
        case 2:
        case 3:
        case 4:
            return titles[1];
        default:
            return titles[2];
    }
}

function setUsers() {
    var i = document.getElementById('users').innerHTML;
    document.getElementById('text2').innerHTML = declOfNum(i, ['Пользователь', 'Пользователя', 'Пользователей'])
}

function setProducts() {
    var i = document.getElementById('products').innerHTML;
    document.getElementById('text1').innerHTML = declOfNum(i, ['Товар', 'Товара', 'Товаров'])
}

function setCategories() {
    var i = document.getElementById('categories').innerHTML;
    document.getElementById('text3').innerHTML = declOfNum(i, ['Категория', 'Категории', 'Категорий'])
}

function setCount() {
    var i = document.getElementById('count').innerHTML;
    document.getElementById('text4').innerHTML = declOfNum(i, ['товар', 'товара', 'товаров'])
}

function goAll() {
    setUsers();
    setProducts();
    setCategories();
}

