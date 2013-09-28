$(function() {
    var hub = $.connection.forumHub;
    var notifyBar = $("#topicsNotifyBar");

    //hub.client.onTopicAdded = function() {
    //    var currentCount = notifyBar.data('item-count');
    //    currentCount++;
    //    notifyBar.text(currentCount + ' new topics');
    //    notifyBar.fadeIn(500);
    //    notifyBar.data('item-count', currentCount);
    //};

    $.connection.hub.start().done(function() {
        notifyBar.on('click', function() {
            location.reload();
        });
    });
});