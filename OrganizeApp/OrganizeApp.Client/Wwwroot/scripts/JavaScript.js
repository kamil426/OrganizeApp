class Helpers {
    static dotNetHelper;

    static setDotNetHelper(value) {
        Helpers.dotNetHelper = value;
    }

    static async OnDrop(taskId, statusId) {
        await Helpers.dotNetHelper.invokeMethodAsync('OnDrop', taskId, statusId);
    }
}

window.Helpers = Helpers;

export function UncheckCheckboxes() {

    var checkBoxes = document.getElementsByClassName('task-checkbox');
    for (var i = 0; i < checkBoxes.length; i++) {
        if (checkBoxes[i].type == 'checkbox' && checkBoxes[i].checked) {
            checkBoxes[i].checked = false;
        }
    }
}

export function CreateDroppable() {

    $('.canban-column').droppable({
        accept: $('.draggable-task'),
        tolerance: "intersect",
        drop: function (event, ui) {
            var droppable = $(this);

            if ($.contains(droppable[0], ui.draggable[0])) {
                return;
            }

            var taskId = parseInt(ui.draggable.attr('data-id'));
            var statusId = parseInt(droppable.attr('data-id'));

            ui.draggable.removeAttr("style");
            ui.draggable.draggable("destroy");
            ui.draggable.css("visibility", "hidden");
            
            Helpers.OnDrop(taskId, statusId).then(function () {

                var tasks = document.getElementsByClassName("draggable-task");
                for (var i = 0; i < tasks.length; i++) {
                    tasks[i].style.visibility = "visible";
                }
            });
        }
    });
}

export function CreateDraggable(taskId) {

    var tasks = document.getElementsByClassName('draggable-task');
    var task;
    for (var i = 0; i < tasks.length; i++) {
        if (tasks[i].getAttribute('data-id') == taskId) {
            task = tasks[i];
            break;
        }
    }

    $(task).draggable({
        start: function (event, ui) {
            $(task).css("z-index", "1");
        },
        stop: function (event, ui) {
            $(task).css("z-index", "0");
        },
        cancel: ".nimbus--edit,.task-link,.task-checkbox",
        revert: true,
        containment: "document",
    });
}

export function SetSchedulerHeight(view) {

    var scheduler = document.getElementById("scheduler");

    switch (view) {

        case "Miesiąc":
            scheduler.style.height = "48.1em";
            break;
        case "Tydzień":
        case "Dzień":
            scheduler.style.height = "77.8em";
            break;
    }
}

export function SetLoginHeader(userName, href) {

    var spinner = document.getElementsByClassName("spinner-header")[0].children[0];
    var spinnerText = document.getElementsByClassName("spinner-header")[0].children[1];

    spinner.style.display = "none";

    if (href != null) {

        var aList = document.getElementsByTagName("a");
        for (var i = 0; i < aList.length; i++) {
            if (aList[i].href == href) {
                aList[i].removeAttribute("href");
            }
        }
    }

    if (userName == null) {
        spinnerText.textContent = "Jesteś niezalogowany!";
        return;
    }

    spinnerText.style.display = "inline";
    spinnerText.textContent = "Witaj " + userName;

    var loginLinkDiv = document.getElementsByClassName("login-link")[0];
    loginLinkDiv.style.display = "block";
    var loginLinkA = loginLinkDiv.children[0];

    loginLinkA.setAttribute("href", "/logout");
    loginLinkA.children[0].textContent = "Wyloguj się";
}