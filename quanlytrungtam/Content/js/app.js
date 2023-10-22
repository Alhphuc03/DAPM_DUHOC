$(document).ready(function() {
    $('.nav-link.active .sub-menu').slideDown();
    // $("p").slideUp();

    $('#sidebar-menu .arrow').click(function() {
        $(this).parents('li').children('.sub-menu').slideToggle();
        $(this).toggleClass('fa-angle-right fa-angle-down');
    });

    $("input[name='checkall']").click(function() {
        var checked = $(this).is(':checked');
        $('.table-checkall tbody tr td input:checkbox').prop('checked', checked);
    });
});

function chooseFile(fileInput){
    if(fileInput.files && fileInput.files[0]){
        var reader=new FileReader();
        reader.onload=function(e){
            $('#image').attr('src',e.target.result);
        }
        reader.readAsDataURL(fileInput.files[0]);
    }
}

