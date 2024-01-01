function errors(jqXHR, exception, container, pre_data = null, callback = null) {
    var msg = "";
    if (jqXHR.status === 0) {
        msg = 'Connection Problem';
        $("#app").html("<h3 align='center'>Internet connection lost</h3>");
    } else if (jqXHR.status == 404) {
        msg = 'Requested page not found. [404]';
    } else if (jqXHR.status == 500) {
        /*msg = 'Internal Server Error [500].';*/
        var responseText2 = jQuery.parseJSON(jqXHR.responseText);
        msg = 'Error (500): ' + responseText2.message + "<br>";
        msg += 'Line: ' + responseText2.line + "<br>";
        $.each(responseText2.errors, function (value, index) {
            var errorarray = eval('responseText.errors.' + value);
            $.each(errorarray, function (value1, index1) {
                msg += value + ": " + index1 + "<br>";
                $('input[name="' + value + '"], select[name="' + value + '"], textarea[name="' + value + '"]').css('border', '1px solid red').parent('.input-field').find('.helper-text').attr('data-error', index1);
            })
        });
    } else if (exception === 'parsererror') {
        msg = 'Requested JSON parse failed.';
    } else if (exception === 'timeout') {
        msg = 'Time out error.';
    } else if (exception === 'abort') {
        msg = 'Ajax request aborted.';
    } else {
        var responseText = jQuery.parseJSON(jqXHR.responseText);
        var num = 1;
        $.each(responseText.errors, function (value, index) {
            var errorarray = eval('responseText.errors.' + value);
            $.each(errorarray, function (value1, index1) {
                msg += num++ + " . " + index1 + "\n";
                $('input[name="' + value + '"]').addClass('invalid').parent('.input-parent').find('.validation-message').html('<small class="text-danger">' + index1 + '</small>');
            })
        });
    }
    if (pre_data != null) {
        $(container).html(pre_data).removeAttr('disabled');
    }
    if (callback != null) {
        callback(msg);
    }
    return msg;
}

$(document).ready(function () {
    $(".aem-sidebar-items").find('.active').parent('ul').show();
    var paretns = $('.aem-sidebar-items .active').parents('ul');
    var paretns2 = $('.active').parents('li').find('.parent').addClass('lactive');
    $.map(paretns, function (one, two) {
        $(one).show();
    });
    $('#loading').html(aem.loading());
});
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#image').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]); // convert to base64 string
    }
}

$("#img_input").change(function () {
    readURL(this);
    $("#update_btn").slideDown();
});
var aem = {
    showElement: function (element) {
        $(element).slideDown();
    },
    hideElement: function (element) {
        $(element).slideUp();
    },
    toggleElemnt: function (event, element) {
        typeof ($(event.target).attr('class')) != "undefined" ? $(event.target).toggleClass('lactive') : $(event.target).find('i').toggleClass('lactive');
        $(element).slideToggle();
    },
    collapseSidebar: function () {
        $('.aem-sidebar').toggleClass('aem-collapse').show();
        $('#content').toggleClass('pr-0');
    },
    _delete: function (event, message = null) {
        event.preventDefault();
        swal({
            icon: 'info',
            title: 'Are you sure',
            text: message,
            buttons: true
        }).then((result) => {
            if (result) {
                window.location.href = $(event.target).attr('href');
            }
        });
    },
    loading: function () {
        return `
            <div class="center">
                <div class="preloader-wrapper small active">
                <div class="spinner-layer spinner-green-only">
                  <div class="circle-clipper left">
                    <div class="circle"></div>
                  </div><div class="gap-patch">
                    <div class="circle"></div>
                  </div><div class="circle-clipper right">
                    <div class="circle"></div>
                  </div>
                </div>
            </div>
        `;
    },
    modal: function (event, route) {
        var models = $('.modal');
        $.map(models, function (one, two) {
            $(one).remove();
        });
        var url = route != null ? route : $(event.target).data('url');
        var modal = document.createElement("div");
        modal.setAttribute('class', 'modal');
        modal.setAttribute('id', 'ajax_modal');
        modal.innerHTML = `
            <div class="modal-content" id="ajax_modal_content">
                ${aem.loading()}
            </div>
        `;
        $('body').append(modal);
        $('.modal').modal();
        var instance = M.Modal.getInstance($('#ajax_modal'));
        instance.open();

        $.ajax({
            url: url,
            method: 'GET',
            cache: false,
            contentType: "application/json; charset=utf-8",
            datatype: "json",
            success: function (data) {
                $("#ajax_modal").html(data);
                M.updateTextFields();
                $('select').formSelect();
            },
            error: function () {
                instance.close();
                M.toast({
                    html: 'Someting went wrong please try again',
                    classes: 'red rounded'
                });
            }
        });


    },
    confirmForm: function (event, message = null, form = true) {
        event.preventDefault()
        swal({
            icon: 'info',
            title: 'Are you sure',
            text: message,
            buttons: true
        }).then((result) => {
            if (result) {
                if (form) {
                    $(event.target).parent('form').submit();
                } else {
                    window.location.href = $(event.target).attr('href');

                }
            }
        });
    },
    request: function (loading_area, url, method, data, target, type = "ajax", callback = null) {
        var default_loading_area = $(loading_area).html();
        $(loading_area).html(aem.loading()).prop('disabled', true);
        $('input').removeClass('invalid');
        $('.validation-message').html('');
        $.ajax({
            url: url,
            method: method,
            data: data,
            success: function (response) {
                $(target).html(response);
            },
            error: function (one, two) {
                swal({
                    title: 'Something went wrong',
                    text: errors(one, two),
                    icon: 'error'
                });
                $(loading_area).html(default_loading_area).removeAttr('disabled');
            }
        });
    },
}