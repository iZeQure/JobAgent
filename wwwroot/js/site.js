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

window.HTMLBodyElement = () => {
    $('#vacancyAdminDetails').modal({
        backdrop: 'static',
        keyboard: false,
        show: false
    });

    $('#jobAdvertModalCreateNew').modal({
        backdrop: 'static',
        keyboard: false,
        show: false
    });

    $('#vacandyRemoveConfirmation').modal({
        backdrop: 'static',
        keyboard: false,
        show: false
    });

    $('#contractModalCreateNew').modal({
        backdrop: 'static',
        keyboard: false,
        show: false
    });

    $('#contractDetails').modal({
        backdrop: 'static',
        keyboard: false,
        show: false
    });
}

window.ToggleUpdateVacancyModal = () => {
    $('#vacancyAdminDetails').modal('toggle');
}

window.ToggleUpdateCompanyModal = () => {
    $('#companyDetailsModal').modal('toggle');
}

window.ToggleRemoveVacancyModal = () => {
    $('#vacandyRemoveConfirmation').modal('toggle');
}

window.ToggleCreateJobAdvertModal = () => {
    $('#jobAdvertModalCreateNew').modal('toggle');
}

window.ToggleRemoveCompanyConfirmationModal = () => {
    $('#removeCompanyModal').modal('toggle');
}