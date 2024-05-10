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
        cancel: ".nimbus--edit,.task-link,.task-checkbox",
        revert: true,
        containment: "document",
    });
}