$(document).ready(function () {
    $('.sidenav').sidenav();
    $('.collapsible').collapsible();
    $('.dropdown-trigger').dropdown({
        constrainWidth: false,
        coverTrigger: false
    });
    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        autoClose: true,
        container: 'body'
    });
    $('.modal').modal({
        preventScrolling: false
    }); 
    $('.carousel').carousel();
    $('.timepicker').timepicker();
    $('.tooltipped').tooltip(); 
    $('.select').formSelect();
});