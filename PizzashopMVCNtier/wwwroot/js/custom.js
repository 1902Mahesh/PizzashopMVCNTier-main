function toggleCheckBoxes(source){
    let checkBoxs = document.querySelectorAll(".row-checkbox");

    checkBoxs.forEach(checkbox => checkbox.checked = source.checked)
}