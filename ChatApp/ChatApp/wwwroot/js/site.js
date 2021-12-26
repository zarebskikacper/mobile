// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function () {
    var btnSend = document.getElementById('btn-send');
    var chatMessage = document.getElementById('chat-message');
    var chatContainer = document.getElementById('chat-container');
    var userLoginVal = document.getElementById('user-login').innerText;

    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/chathub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.start().then(function () {
        console.log("connected");
        connection.invoke('UserSignedIn', {
            login: userLoginVal
        });
    });

    connection.on("ChatMessageReceived", (obj) => {
        var message = obj.message;
        var createdOn = obj.formattedCreatedOn;
        var login = obj.login;

        $(chatContainer).prepend('<li><span class="text-success">' + '[' + createdOn + '] ' + login + ': </span>' + message + '</li>');
    });

    connection.on("UserSignedIn", (obj) => {
        var createdOn = obj.formattedCreatedOn;
        var login = obj.login;

        $(chatContainer).prepend('<li class="text-danger">' + '[' + createdOn + '] ' + 'Nowy użytkownik: ' + login + '</li>');
    });

    $(btnSend).click(function () {
        var message = $(chatMessage).val();
        connection.invoke('SendChatMessage', {
            login: userLoginVal,
            message: message
        });

        $(chatMessage).val('');
    })


})();