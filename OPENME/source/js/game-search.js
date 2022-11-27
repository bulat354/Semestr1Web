window.addEventListener("DOMContentLoaded", searchGames);

function searchGames() {
    let added = document.getElementsByClassName("added");
    while (added.length > 0) {
        added.item(0).remove();
    }

    let form = document.search;
    let text = form.text.value;

    let xnr = new XMLHttpRequest();
    xnr.open('GET', '/data/searchgames?text=' + text, false);
    xnr.send();

    let parser = new DOMParser();
    let doc = parser.parseFromString(xnr.responseText, "text/html");
    let parent = form.parentNode;
    while (doc.body.children.length > 0) {
        parent.append(doc.body.children[0]);
    }
}