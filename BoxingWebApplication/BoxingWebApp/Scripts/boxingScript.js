function updateQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}

$(function () {
    $(".pageSizeDropDown").change(function () {
        var newPageSize = $('option:selected', $(this)).text();
        var newUrl = updateQueryStringParameter((updateQueryStringParameter(window.location.href, 'skip', 0)), 'take', newPageSize);
        window.location.replace(newUrl);
    });
});

$(function () {
    $(".searchButton").click(function () {
        debugger;
        var searchString = $(this).closest('tr').find('.searchInput').val();
        var newUrl = updateQueryStringParameter(window.location.href, 'search', searchString);
        window.location.replace(newUrl);
    });
});

$(function () {
    $(".searchInput").keyup(function (event) {
        if (event.keyCode == 13) {
            var searchButton = $(this).closest('tr').find('.searchButton');
            if (searchButton) {
                searchButton.click();
            }
        }
    });
});

$(function () {
    $('.datetimepicker').datetimepicker({
        format: 'DD/MM/YYYY, HH:mm',
        useCurrent: true,
        showTodayButton: true
    });
});