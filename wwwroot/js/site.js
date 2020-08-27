window.HTMLTitleElement = (title) => {
    document.title = title;
}

window.OnInformationChangeAnimation = (id) => {
    console.info(`Animating => ${id}`);

    $(`#${id}`).toggleClass('onInformationChangeAnimation');

    setTimeout(function () {
        $(`#${id}`).toggleClass('onInformationChangeAnimation');
    }, 6000);

    //$(`#${id}`).toggleClass('animation', 3000);
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

window.ToggleUpdateVacancyModal = () => {
    $('#vacancyAdminDetails').modal('toggle');
}

window.ToggleRemoveVacancyModal = () => {
    $('#vacandyRemoveConfirmation').modal('toggle');
}