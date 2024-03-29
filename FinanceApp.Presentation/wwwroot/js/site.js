﻿function redirectToAction(url) {
    window.location.href = url;
}
function Accept(amount) {
    fetch('/Finance/Accept?amount='+amount, {
        method: 'PUT',
    }).then(response => {
            location.href = "/Identity/Profile";
    })
}