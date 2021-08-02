window.setDocumentTitleOnAfterRender = function (documentTitle) {
    document.title = documentTitle;
}

window.onInformationChangeAnimateTableRow = function (rowId) {
    $(`#${rowId}`).toggleClass('onInformationChangeAnimation');

    setTimeout(function () {
        $(`#${rowId}`).toggleClass('onInformationChangeAnimation');
    }, 6000);
}

window.toggleModalVisibility = function (modalId) {
    $(`#${modalId}`).modal('toggle');
}

window.tooltipInformation = function (name, body, color) {

    //if (color == 'Danger') {
    //    $('.popover-danger').popover({
    //        title: name,
    //        content: body,
    //        trigger: 'hover',
    //        placement: 'left',
    //        animation: true
    //    });
    //}
    //else {
    //    $('.popover-info').popover({
    //        title: name,
    //        content: body,
    //        trigger: 'hover',
    //        placement: 'left',
    //        animation: true
    //    });
    //}

    var popover = null;

    if (color == 'Danger') {
        popover = new bootstrap.Popover(document.querySelector('.popover-danger'), {
            title: name,
            content: body,
            trigger: 'hover',
            placement: 'left',
            animation: true
        });

    }
    else {
        popover = new bootstrap.Popover(document.querySelector('.popover-info'), {
            title: name,
            content: body,
            trigger: 'hover',
            placement: 'left',
            animation: true
        });
    }
}