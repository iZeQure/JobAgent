window.HTMLTitleElement = (title) => {
    document.title = title;
}

window.HTMLBodyElement = (name, body, color) => {

    console.log(color);

    if (color == 'Danger') {
        $('.popover-danger').popover({
            title: name,
            content: body,
            trigger: 'hover',
            placement: 'left',
            animation: true
        });
    }
    else {
        $('.popover-info').popover({
            title: name,
            content: body,
            trigger: 'hover',
            placement: 'left',
            animation: true
        });
    }    
}

window.HTMLBodyElement = () => {
    $('#vacancyAdminDetails').modal({
        backdrop: 'static',
        keyboard: false,
        show: false
    });
}

window.Modal = () => {
    $('#vacancyAdminDetails').modal('toggle');
}