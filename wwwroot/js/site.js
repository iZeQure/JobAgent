window.HTMLTitleElement = (title) => {
    document.title = title;
}

window.OnInformationChangeAnimation = (id) => {
    console.info(`Animating => ${id}`);

    $(`#${id}`).toggleClass('onInformationChangeAnimation');

    setTimeout(function () {
        $(`#${id}`).toggleClass('onInformationChangeAnimation');
    }, 6000);
}

window.PopoverInformation = (name, body, color) => {
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

window.StaticModal = (elementId) => {
    $(`#${elementId}`).modal({
        backdrop: 'static',
        keyboard: false,
        show: false
    });
}

window.confirmRemove = (elementId) => {
    $(`#${elementId}`).modal('toggle');
}

window.modalToggle = (elementId) => {
    $(`#${elementId}`).modal('toggle');
} 