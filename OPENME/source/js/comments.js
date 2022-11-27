function initLoad(xhr) {
    let result = xhr.target.responseText;
    if (result == null) {
        return;
    }

    let elements = new DOMParser().parseFromString(result, 'text/html').body.children;
    let content = document.getElementById("content");
    while (elements.length > 0) {
        content.appendChild(elements[0]);
    }
}

function commentLoad(xhr, element) {
    let result = xhr.target.responseText;
    if (result == null) {
        return;
    }

    let elements = new DOMParser().parseFromString(result, 'text/html').body.children;

    let prev = element;
    while (elements.length > 0) {
        let added = elements[0]; 
        prev.after(added);
        prev = added;
    }
}

function initComments(id) {
    let xhr = new XMLHttpRequest();
    xhr.open('GET', '/data/allcomments?id=' + id, true);
    xhr.addEventListener("load", initLoad);
    xhr.send();
}

function newComment(id) {
    let form = document.comment;
    if (form == null) {
        return;
    }

    let text = form.text.value;
    if (text.length < 1) {
        return;
    }

    let xhr = new XMLHttpRequest();
    xhr.open('POST', '/data/addcomment', true);
    xhr.addEventListener("load", (xhr) => commentLoad(xhr, form));
    xhr.send('id=' + id + '&text=' + text);

    form.text.value = "";
}

function applyEditing(id) {
    let form = document.getElementById('comment' + id);
    let text = form.text.value;
    
    let xhr = new XMLHttpRequest();
    xhr.open('POST', '/data/editcomment', true);
    xhr.addEventListener("load", function(xhr) { 
        commentLoad(xhr, form);
        form.remove();
    });
    xhr.send('id=' + id + '&text=' + text)
}

function editComment(id) {
    let form = document.getElementById('comment' + id);
    let input = form.text;
    let submit = form.querySelector("input[type=\"submit\"]");
    if (submit == null) {
        return;
    }

    input.setAttribute("class", 'input hovered-card');
    input.toggleAttribute("readonly");

    submit.value = "Применить";
    submit.onclick = () => applyEditing(id);
}