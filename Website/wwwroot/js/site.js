window.HTMLTitleElement = (title) => {
    document.title = title;
};

window.OnInformationChangeAnimation = (id) => {
    var animation = document.getElementById(`${id}`).toggleClass('onInformationChangeAnimation');

    setTimeout(function () {
        var timeout = document.getElementById(`${id}`).toggleClass('onInformationChangeAnimation');
    }, 6000);
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
    var sModal = document.getElementById(`${elementId}`).modal({
        backdrop: 'static',
        keyboard: false,
        show: false
    });
};

window.confirmRemove = (elementId) => {
    var confirmRemoval = document.getElementById(`${elementId}`).modal('toggle');
};

window.modalToggle = (elementId) => {
    var toggleModal = document.getElementById(`${elementId}`).modal('toggle');
};