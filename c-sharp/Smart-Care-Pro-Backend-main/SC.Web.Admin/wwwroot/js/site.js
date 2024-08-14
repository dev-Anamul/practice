
//==============SEARCH=============//

$(document).ready(function () {

    $("#myInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myTable tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

   document.getElementById("showAllButton").addEventListener("click", function (event) {
      event.preventDefault();
      document.getElementById("nameInput").value = "";
      document.getElementById("filterForm").submit();
   });
});

//==============VALIDATION=============//

// 1. Accept Only Alphabat

function IsAlphabat(e) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || e.charCode == 32)
            return true;
        else
            return false;
    }
    catch (err) {
        // alert(err.Description);
    }
}

// 2. Accept only Numeric

function Isnumeric(evt) {
    // Only ASCII character in that range allowed
    var ASCIICode = (evt.which) ? evt.which : evt.keyCode
    if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
        return false;
    return true;
}

//Select search
$(document).ready(function () {
    $('.select2').select2();

    const currentUrl = window.location.pathname;
    var segments = currentUrl.split('/'); // Split the path into segments
    var controllerName = segments[1];
    var queryString = window.location.search;
    var params = new URLSearchParams(queryString);
    var paramValue = params.get('module');
    var paramValueParent = params.get('parent');
    console.log(paramValueParent)
    $('#sidebar-nav .nav-item .main-link').each(function () {

        var mainNavLinkText = $(this).text();
        let mainLink = $(this);
        if (paramValueParent == null) {
            if (mainNavLinkText.trim() == paramValue) {
                mainLink.removeClass('collapsed');
                mainLink.add('aria-expanded', 'true')
                mainLink.siblings('ul').addClass('show')

                $(this).parent().find('.nav-item').each(function () {
                    let nestedUrl = $(this).find('.nav-link').attr('href');

                    if (nestedUrl.trim().includes(controllerName)) {

                        $(this).children('.nav-link').addClass('active').focus();

                    }
                })
                return false;
            }
        } else {
            if (mainNavLinkText.trim() == paramValueParent) {
                mainLink.removeClass('collapsed');
                mainLink.add('aria-expanded', 'true')
                mainLink.siblings('ul').addClass('show')
           
                $(this).parent().find('.nav-item').each(function () {
                    let nestedUrl = $(this).find('.nav-link').attr('href');

                    if (nestedUrl.trim().includes(controllerName)) {

                        $(this).children('.nav-link').closest('ul').addClass('show');
                        $(this).children('.nav-link').addClass('active').focus();

                        return false;
                    }
                })
                return false;
            }
        }
    });
});