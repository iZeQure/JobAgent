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

    switch (color) {
        case 'Danger':
            $('.popover-danger').popover({
                title: name,
                content: body,
                trigger: 'hover',
                placement: 'left',
                animation: true
            });
            break;

        case 'Info':
            $('.popover-info').popover({
                title: name,
                content: body,
                trigger: 'hover',
                placement: 'left',
                animation: true
            });
            break;

        default:
            console.log("POPCORN HAR KNÆKKET SIG IGEN FOR SAAATAN HENT SMØREN!");
            break;
    }

}