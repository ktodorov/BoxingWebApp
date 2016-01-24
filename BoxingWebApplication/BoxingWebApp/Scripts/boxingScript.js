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