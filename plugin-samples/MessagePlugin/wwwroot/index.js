
window.external.receiveMessage(msg => {
    if (msg && msg.startsWith("__bwv:")) {
        return;
    }
    console.log(msg);
});

window.pluginAlert = (msg) => {
    alert("pluginAlert: " + msg);
}