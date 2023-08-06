var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

const messageInput = document.getElementById("messageInput");
const sendButton = document.getElementById("sendButton");
sendButton.disabled = true;

connection.on("ReceiveMessage", function (message, user) {
    var username = message.split(':')[0]
    addMessage(message, false, username);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

sendButton.addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var receiver = (document.getElementById("reveiverInput") || {}).value || "admin@admin.com";
    var message = `${user}: ${messageInput.value}`;
    connection.invoke("SendToUserMessage", message, receiver).catch(function (err) {
        return console.error(err.toString());
    });

    addMessage(message, true);
    messageInput.value = "";

    event.preventDefault();
});

function addMessage(text, isCurrentUser, user) {
    const whichSideClass = isCurrentUser ? "end-0" : "start-0";
    var p = createEl('p', { "class": 'border rounded p-2 px-3 m-0 position-absolute ' + whichSideClass }, text);

    if (user != undefined) {
        const href = `/Chat/Chat?user=${encodeURIComponent(user)}`;
        p.appendChild(createEl('a', { "href": href },
            createEl('i', { "class": 'bi bi-reply' })));
    }

    var div = createEl('div', { "class": 'py-1 position-relative', "style": 'width:100%;' }, p);

    document.getElementById("messagesList").appendChild(div);
}

function createEl(type, attributes, ...content) {
    const result = document.createElement(type);

    for (let [attr, value] of Object.entries(attributes || {})) {
        if (attr.substring(0, 2) == 'on') {
            result.addEventListener(attr.substring(2).toLocaleLowerCase(), value);
        } else {
            if (typeof value === 'boolean') {
                result[attr] = value;
            } else {
                result.setAttribute(attr, value);
            }
        }
    }

    content = content.reduce((a, c) => a.concat(Array.isArray(c) ? c : [c]), []);

    content.forEach(e => {
        if (typeof e == 'string' || typeof e == 'number') {
            const node = document.createTextNode(e);
            result.appendChild(node);
        } else {
            result.appendChild(e);
        }
    });

    return result;
}