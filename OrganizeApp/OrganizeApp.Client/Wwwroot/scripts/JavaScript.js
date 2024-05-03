export function UncheckCheckboxes() {

    var checkBoxes = document.getElementsByClassName('task-checkbox');

    for (var i = 0; i < checkBoxes.length; i++) {

        if (checkBoxes[i].type == 'checkbox' && checkBoxes[i].checked) {
            checkBoxes[i].checked = false;
        }
    }
}