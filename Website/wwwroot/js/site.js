window.HTMLTitleElement = (title) => {
    document.title = title;
};

window.OnInformationChangeAnimation = (id) => {
    $(`#${id}`).toggleClass('onInformationChangeAnimation');

    setTimeout(function () {
        $(`#${id}`).toggleClass('onInformationChangeAnimation');
    }, 6000);

    //var animation = document.getElementById(`${id}`)
    //animation.toggleClass('onInformationChangeAnimation');

    //setTimeout(function () {
    //    var timeout = document.getElementById(`${id}`)
    //    timeout.toggleClass('onInformationChangeAnimation');
    //}, 6000);
};

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

    //if (color == 'Danger') {
    //    var popover = new bootstrap.Popover(document.querySelector('.popover-danger'), {
    //        title: name,
    //        content: body,
    //        trigger: 'hover',
    //        placement: 'left',
    //        animation: true
    //    });
    //}
    //else {
    //    var popover = new bootstrap.Popover(document.querySelector('.popover-info'), {
    //        title: name,
    //        content: body,
    //        trigger: 'hover',
    //        placement: 'left',
    //        animation: true
    //    });
    //}
};

window.StaticModal = (elementId) => {
    $(`#${elementId}`).modal({
        backdrop: true,
        keyboard: false,
        show: false
    });

    //var sModal = document.getElementById(`${elementId}`)

    //sModal.modal({
    //    backdrop: 'static',
    //    keyboard: false,
    //    show: false
    //})
};

window.confirmRemove = (elementId) => {
    $(`#${elementId}`).modal('toggle');

    //var confirmRemoval = document.getElementById(`${elementId}`)
    //confirmRemoval.modal('toggle')
};

window.modalToggle = (elementId) => {
    $(`#${elementId}`).modal('toggle');

    //var toggleModal = document.getElementById(`${elementId}`)
    //toggleModal.modal('toggle')
};