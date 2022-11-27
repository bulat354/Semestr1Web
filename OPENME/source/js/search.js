let currentPage = 1;

function submitText() {
    searchArticles();
}

function submitPage(page) {
    let element = document.getElementById("page" + currentPage);
    if (element != null) {
        if (element.hasAttribute("disabled"))
            element.toggleAttribute("disabled");
    }

    currentPage = page;

    element = document.getElementById("page" + currentPage);
    element.toggleAttribute("disabled", true);

    searchArticles();
}

function getPagesCount(searchString) {
    let xnr1 = new XMLHttpRequest();
    xnr1.open('GET', '/data/count?text=' + searchString, false);
    xnr1.send();

    return eval(xnr1.responseText);
}

function applyPages(searchString) {
    let count = getPagesCount(searchString);

    let pages = document.getElementById("pages");
    pages.innerHTML = "";

    for (let i = 1; i <= count; i++) {
        let page = document.createElement("a");
        page.setAttribute("class", "button hovered-card");
        page.setAttribute("id", "page" + i);
        page.innerHTML = i;
        page.addEventListener("click", () => submitPage(i));

        pages.append(page);
    }

    if (currentPage > count) {
        currentPage = count;
    }

    element = document.getElementById("page" + currentPage);
    if (element != null) {
        element.toggleAttribute("disabled", true);
        element.setAttribute("class", "button static-card")
    }
}

function getArticles(searchString, currentPage) {
    let xnr2 = new XMLHttpRequest();
    xnr2.open('GET', '/data/search?text=' + searchString + '&page=' + currentPage, false);
    xnr2.send();

    return new DOMParser().parseFromString(xnr2.responseText, 'text/html').body.children;
}

function removeOldArticles() {
    let articles = document.search.parentElement.children;

    let index = 0;
    while (articles.length > index)
    {
        let element = articles[index];

        if (element.tagName === "A") {
            element.remove();
        }
        else {
            index++;
        }
    }
}

function addNewArticles(searchString, currentPage) {
    let form = document.search;

    let prev = form;
    let elements = getArticles(searchString, currentPage);

    while (elements.length > 0) {
        let element = elements[0];
        prev.after(element);
        prev = element;
    }
}

function searchArticles() {
    let form = document.search;
    let searchString = form.text.value;

    applyPages(searchString);

    removeOldArticles();

    addNewArticles(searchString, currentPage);
}

document.addEventListener("DOMContentLoaded", searchArticles);