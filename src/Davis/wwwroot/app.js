window.Davis = {};

const cacheJs = {};
window.Davis.loadJs = (url) => {
    console.debug("loadJs", url);
    if (cacheJs[url]) {
        console.debug(url, "has loaded")
        return;
    }
    cacheJs[url] = true;
    const script = document.createElement("script");
    script.type = "text/javascript";
    script.src = `app://${url}`;
    document.body.appendChild(script);
    cacheJs[url] = script;
}

window.Davis.unloadJs = (url) => {
    console.debug("unloadJs", url);
    if (!cacheJs[url]) {
        console.debug(url, "has unloadJs")
        return;
    }
    const node = cacheJs[url];
    if (node) {
        node.remove();
    }
    delete cacheJs[url];
}

const cacheCss = {};
window.Davis.loadCss = (url) => {
    console.debug("loadCss", url);
    if (cacheCss[url]) {
        console.debug(url, "has loaded")
        return;
    }
    cacheCss[url] = true;
    const link = document.createElement("link");
    link.rel = "stylesheet";
    link.type = "text/css";
    link.href = `app://${url}`;
    const head = document.getElementsByTagName("head")[0];
    head.appendChild(link);
    cacheCss[url] = link;
}

window.Davis.unloadCss = (url) => {
    console.debug("unloadCss", url);
    if (!cacheCss[url]) {
        console.debug(url, "has unloadCss")
        return;
    }
    const node = cacheCss[url];
    if (node) {
        node.remove();
    }
    delete cacheCss[url];
}
