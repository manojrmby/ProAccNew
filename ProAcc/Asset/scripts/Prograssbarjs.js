/*! For license information please see monkey-bar.min.js.LICENSE */
!(function (e) {
    var t = {};
    function s(r) {
        if (t[r]) return t[r].exports;
        var i = (t[r] = { i: r, l: !1, exports: {} });
        return e[r].call(i.exports, i, i.exports, s), (i.l = !0), i.exports;
    }
    (s.m = e),
        (s.c = t),
        (s.d = function (e, t, r) {
            s.o(e, t) || Object.defineProperty(e, t, { enumerable: !0, get: r });
        }),
        (s.r = function (e) {
            'undefined' != typeof Symbol &&
                Symbol.toStringTag &&
                Object.defineProperty(e, Symbol.toStringTag, { value: 'Module' }),
                Object.defineProperty(e, '__esModule', { value: !0 });
        }),
        (s.t = function (e, t) {
            if ((1 & t && (e = s(e)), 8 & t)) return e;
            if (4 & t && 'object' == typeof e && e && e.__esModule) return e;
            var r = Object.create(null);
            if (
                (s.r(r),
                    Object.defineProperty(r, 'default', { enumerable: !0, value: e }),
                    2 & t && 'string' != typeof e)
            )
                for (var i in e)
                    s.d(
                        r,
                        i,
                        function (t) {
                            return e[t];
                        }.bind(null, i),
                    );
            return r;
        }),
        (s.n = function (e) {
            var t =
                e && e.__esModule
                    ? function () {
                        return e.default;
                    }
                    : function () {
                        return e;
                    };
            return s.d(t, 'a', t), t;
        }),
        (s.o = function (e, t) {
            return Object.prototype.hasOwnProperty.call(e, t);
        }),
        (s.p = ''),
        s((s.s = 0));
})([
    function (e, t, s) {
        'use strict';
        s.r(t);
        const r = new WeakMap(),
            i = (e) => 'function' == typeof e && r.has(e),
            n =
                void 0 !== window.customElements &&
                void 0 !== window.customElements.polyfillWrapFlushCallback,
            o = (e, t, s = null) => {
                for (; t !== s;) {
                    const s = t.nextSibling;
                    e.removeChild(t), (t = s);
                }
            },
            a = {},
            l = {},
            h = `{{lit-${String(Math.random()).slice(2)}}}`,
            d = `\x3c!--${h}--\x3e`,
            c = new RegExp(`${h}|${d}`),
            p = '$lit$';
        class u {
            constructor(e, t) {
                (this.parts = []), (this.element = t);
                const s = [],
                    r = [],
                    i = document.createTreeWalker(t.content, 133, null, !1);
                let n = 0,
                    o = -1,
                    a = 0;
                const {
                    strings: l,
                    values: { length: d },
                } = e;
                for (; a < d;) {
                    const e = i.nextNode();
                    if (null !== e) {
                        if ((o++ , 1 === e.nodeType)) {
                            if (e.hasAttributes()) {
                                const t = e.attributes,
                                    { length: s } = t;
                                let r = 0;
                                for (let e = 0; e < s; e++) m(t[e].name, p) && r++;
                                for (; r-- > 0;) {
                                    const t = l[a],
                                        s = f.exec(t)[2],
                                        r = s.toLowerCase() + p,
                                        i = e.getAttribute(r);
                                    e.removeAttribute(r);
                                    const n = i.split(c);
                                    this.parts.push({
                                        type: 'attribute',
                                        index: o,
                                        name: s,
                                        strings: n,
                                    }),
                                        (a += n.length - 1);
                                }
                            }
                            'TEMPLATE' === e.tagName &&
                                (r.push(e), (i.currentNode = e.content));
                        } else if (3 === e.nodeType) {
                            const t = e.data;
                            if (t.indexOf(h) >= 0) {
                                const r = e.parentNode,
                                    i = t.split(c),
                                    n = i.length - 1;
                                for (let t = 0; t < n; t++) {
                                    let s,
                                        n = i[t];
                                    if ('' === n) s = _();
                                    else {
                                        const e = f.exec(n);
                                        null !== e &&
                                            m(e[2], p) &&
                                            (n =
                                                n.slice(0, e.index) +
                                                e[1] +
                                                e[2].slice(0, -p.length) +
                                                e[3]),
                                            (s = document.createTextNode(n));
                                    }
                                    r.insertBefore(s, e),
                                        this.parts.push({ type: 'node', index: ++o });
                                }
                                '' === i[n]
                                    ? (r.insertBefore(_(), e), s.push(e))
                                    : (e.data = i[n]),
                                    (a += n);
                            }
                        } else if (8 === e.nodeType)
                            if (e.data === h) {
                                const t = e.parentNode;
                                (null !== e.previousSibling && o !== n) ||
                                    (o++ , t.insertBefore(_(), e)),
                                    (n = o),
                                    this.parts.push({ type: 'node', index: o }),
                                    null === e.nextSibling ? (e.data = '') : (s.push(e), o--),
                                    a++;
                            } else {
                                let t = -1;
                                for (; -1 !== (t = e.data.indexOf(h, t + 1));)
                                    this.parts.push({ type: 'node', index: -1 }), a++;
                            }
                    } else i.currentNode = r.pop();
                }
                for (const e of s) e.parentNode.removeChild(e);
            }
        }
        const m = (e, t) => {
            const s = e.length - t.length;
            return s >= 0 && e.slice(s) === t;
        },
            g = (e) => -1 !== e.index,
            _ = () => document.createComment(''),
            f = /([ \x09\x0a\x0c\x0d])([^\0-\x1F\x7F-\x9F "'>=/]+)([ \x09\x0a\x0c\x0d]*=[ \x09\x0a\x0c\x0d]*(?:[^ \x09\x0a\x0c\x0d"'`<>=]*|"[^"]*|'[^']*))$/;
        class y {
            constructor(e, t, s) {
                (this.__parts = []),
                    (this.template = e),
                    (this.processor = t),
                    (this.options = s);
            }
            update(e) {
                let t = 0;
                for (const s of this.__parts) void 0 !== s && s.setValue(e[t]), t++;
                for (const e of this.__parts) void 0 !== e && e.commit();
            }
            _clone() {
                const e = n
                    ? this.template.element.content.cloneNode(!0)
                    : document.importNode(this.template.element.content, !0),
                    t = [],
                    s = this.template.parts,
                    r = document.createTreeWalker(e, 133, null, !1);
                let i,
                    o = 0,
                    a = 0,
                    l = r.nextNode();
                for (; o < s.length;)
                    if (((i = s[o]), g(i))) {
                        for (; a < i.index;)
                            a++ ,
                                'TEMPLATE' === l.nodeName &&
                                (t.push(l), (r.currentNode = l.content)),
                                null === (l = r.nextNode()) &&
                                ((r.currentNode = t.pop()), (l = r.nextNode()));
                        if ('node' === i.type) {
                            const e = this.processor.handleTextExpression(this.options);
                            e.insertAfterNode(l.previousSibling), this.__parts.push(e);
                        } else
                            this.__parts.push(
                                ...this.processor.handleAttributeExpressions(
                                    l,
                                    i.name,
                                    i.strings,
                                    this.options,
                                ),
                            );
                        o++;
                    } else this.__parts.push(void 0), o++;
                return n && (document.adoptNode(e), customElements.upgrade(e)), e;
            }
        }
        const v = ` ${h} `;
        class b {
            constructor(e, t, s, r) {
                (this.strings = e),
                    (this.values = t),
                    (this.type = s),
                    (this.processor = r);
            }
            getHTML() {
                const e = this.strings.length - 1;
                let t = '',
                    s = !1;
                for (let r = 0; r < e; r++) {
                    const e = this.strings[r],
                        i = e.lastIndexOf('\x3c!--');
                    s = (i > -1 || s) && -1 === e.indexOf('--\x3e', i + 1);
                    const n = f.exec(e);
                    t +=
                        null === n
                            ? e + (s ? v : d)
                            : e.substr(0, n.index) + n[1] + n[2] + p + n[3] + h;
                }
                return (t += this.strings[e]);
            }
            getTemplateElement() {
                const e = document.createElement('template');
                return (e.innerHTML = this.getHTML()), e;
            }
        }
        const S = (e) =>
            null === e || !('object' == typeof e || 'function' == typeof e),
            w = (e) => Array.isArray(e) || !(!e || !e[Symbol.iterator]);
        class x {
            constructor(e, t, s) {
                (this.dirty = !0),
                    (this.element = e),
                    (this.name = t),
                    (this.strings = s),
                    (this.parts = []);
                for (let e = 0; e < s.length - 1; e++)
                    this.parts[e] = this._createPart();
            }
            _createPart() {
                return new P(this);
            }
            _getValue() {
                const e = this.strings,
                    t = e.length - 1;
                let s = '';
                for (let r = 0; r < t; r++) {
                    s += e[r];
                    const t = this.parts[r];
                    if (void 0 !== t) {
                        const e = t.value;
                        if (S(e) || !w(e)) s += 'string' == typeof e ? e : String(e);
                        else for (const t of e) s += 'string' == typeof t ? t : String(t);
                    }
                }
                return (s += e[t]);
            }
            commit() {
                this.dirty &&
                    ((this.dirty = !1),
                        this.element.setAttribute(this.name, this._getValue()));
            }
        }
        class P {
            constructor(e) {
                (this.value = void 0), (this.committer = e);
            }
            setValue(e) {
                e === a ||
                    (S(e) && e === this.value) ||
                    ((this.value = e), i(e) || (this.committer.dirty = !0));
            }
            commit() {
                for (; i(this.value);) {
                    const e = this.value;
                    (this.value = a), e(this);
                }
                this.value !== a && this.committer.commit();
            }
        }
        class N {
            constructor(e) {
                (this.value = void 0),
                    (this.__pendingValue = void 0),
                    (this.options = e);
            }
            appendInto(e) {
                (this.startNode = e.appendChild(_())),
                    (this.endNode = e.appendChild(_()));
            }
            insertAfterNode(e) {
                (this.startNode = e), (this.endNode = e.nextSibling);
            }
            appendIntoPart(e) {
                e.__insert((this.startNode = _())), e.__insert((this.endNode = _()));
            }
            insertAfterPart(e) {
                e.__insert((this.startNode = _())),
                    (this.endNode = e.endNode),
                    (e.endNode = this.startNode);
            }
            setValue(e) {
                this.__pendingValue = e;
            }
            commit() {
                for (; i(this.__pendingValue);) {
                    const e = this.__pendingValue;
                    (this.__pendingValue = a), e(this);
                }
                const e = this.__pendingValue;
                e !== a &&
                    (S(e)
                        ? e !== this.value && this.__commitText(e)
                        : e instanceof b
                            ? this.__commitTemplateResult(e)
                            : e instanceof Node
                                ? this.__commitNode(e)
                                : w(e)
                                    ? this.__commitIterable(e)
                                    : e === l
                                        ? ((this.value = l), this.clear())
                                        : this.__commitText(e));
            }
            __insert(e) {
                this.endNode.parentNode.insertBefore(e, this.endNode);
            }
            __commitNode(e) {
                this.value !== e && (this.clear(), this.__insert(e), (this.value = e));
            }
            __commitText(e) {
                const t = this.startNode.nextSibling,
                    s = 'string' == typeof (e = null == e ? '' : e) ? e : String(e);
                t === this.endNode.previousSibling && 3 === t.nodeType
                    ? (t.data = s)
                    : this.__commitNode(document.createTextNode(s)),
                    (this.value = e);
            }
            __commitTemplateResult(e) {
                const t = this.options.templateFactory(e);
                if (this.value instanceof y && this.value.template === t)
                    this.value.update(e.values);
                else {
                    const s = new y(t, e.processor, this.options),
                        r = s._clone();
                    s.update(e.values), this.__commitNode(r), (this.value = s);
                }
            }
            __commitIterable(e) {
                Array.isArray(this.value) || ((this.value = []), this.clear());
                const t = this.value;
                let s,
                    r = 0;
                for (const i of e)
                    void 0 === (s = t[r]) &&
                        ((s = new N(this.options)),
                            t.push(s),
                            0 === r ? s.appendIntoPart(this) : s.insertAfterPart(t[r - 1])),
                        s.setValue(i),
                        s.commit(),
                        r++;
                r < t.length && ((t.length = r), this.clear(s && s.endNode));
            }
            clear(e = this.startNode) {
                o(this.startNode.parentNode, e.nextSibling, this.endNode);
            }
        }
        class C {
            constructor(e, t, s) {
                if (
                    ((this.value = void 0),
                        (this.__pendingValue = void 0),
                        2 !== s.length || '' !== s[0] || '' !== s[1])
                )
                    throw new Error(
                        'Boolean attributes can only contain a single expression',
                    );
                (this.element = e), (this.name = t), (this.strings = s);
            }
            setValue(e) {
                this.__pendingValue = e;
            }
            commit() {
                for (; i(this.__pendingValue);) {
                    const e = this.__pendingValue;
                    (this.__pendingValue = a), e(this);
                }
                if (this.__pendingValue === a) return;
                const e = !!this.__pendingValue;
                this.value !== e &&
                    (e
                        ? this.element.setAttribute(this.name, '')
                        : this.element.removeAttribute(this.name),
                        (this.value = e)),
                    (this.__pendingValue = a);
            }
        }
        class T extends x {
            constructor(e, t, s) {
                super(e, t, s),
                    (this.single = 2 === s.length && '' === s[0] && '' === s[1]);
            }
            _createPart() {
                return new A(this);
            }
            _getValue() {
                return this.single ? this.parts[0].value : super._getValue();
            }
            commit() {
                this.dirty &&
                    ((this.dirty = !1), (this.element[this.name] = this._getValue()));
            }
        }
        class A extends P { }
        let k = !1;
        try {
            const e = {
                get capture() {
                    return (k = !0), !1;
                },
            };
            window.addEventListener('test', e, e),
                window.removeEventListener('test', e, e);
        } catch (e) { }
        class E {
            constructor(e, t, s) {
                (this.value = void 0),
                    (this.__pendingValue = void 0),
                    (this.element = e),
                    (this.eventName = t),
                    (this.eventContext = s),
                    (this.__boundHandleEvent = (e) => this.handleEvent(e));
            }
            setValue(e) {
                this.__pendingValue = e;
            }
            commit() {
                for (; i(this.__pendingValue);) {
                    const e = this.__pendingValue;
                    (this.__pendingValue = a), e(this);
                }
                if (this.__pendingValue === a) return;
                const e = this.__pendingValue,
                    t = this.value,
                    s =
                        null == e ||
                        (null != t &&
                            (e.capture !== t.capture ||
                                e.once !== t.once ||
                                e.passive !== t.passive)),
                    r = null != e && (null == t || s);
                s &&
                    this.element.removeEventListener(
                        this.eventName,
                        this.__boundHandleEvent,
                        this.__options,
                    ),
                    r &&
                    ((this.__options = V(e)),
                        this.element.addEventListener(
                            this.eventName,
                            this.__boundHandleEvent,
                            this.__options,
                        )),
                    (this.value = e),
                    (this.__pendingValue = a);
            }
            handleEvent(e) {
                'function' == typeof this.value
                    ? this.value.call(this.eventContext || this.element, e)
                    : this.value.handleEvent(e);
            }
        }
        const V = (e) =>
            e &&
            (k
                ? { capture: e.capture, passive: e.passive, once: e.once }
                : e.capture);
        const O = new (class {
            handleAttributeExpressions(e, t, s, r) {
                const i = t[0];
                if ('.' === i) {
                    return new T(e, t.slice(1), s).parts;
                }
                return '@' === i
                    ? [new E(e, t.slice(1), r.eventContext)]
                    : '?' === i
                        ? [new C(e, t.slice(1), s)]
                        : new x(e, t, s).parts;
            }
            handleTextExpression(e) {
                return new N(e);
            }
        })();
        function M(e) {
            let t = U.get(e.type);
            void 0 === t &&
                ((t = { stringsArray: new WeakMap(), keyString: new Map() }),
                    U.set(e.type, t));
            let s = t.stringsArray.get(e.strings);
            if (void 0 !== s) return s;
            const r = e.strings.join(h);
            return (
                void 0 === (s = t.keyString.get(r)) &&
                ((s = new u(e, e.getTemplateElement())), t.keyString.set(r, s)),
                t.stringsArray.set(e.strings, s),
                s
            );
        }
        const U = new Map(),
            R = new WeakMap();
        (window.litHtmlVersions || (window.litHtmlVersions = [])).push('1.1.2');
        const j = (e, ...t) => new b(e, t, 'html', O),
            $ = 133;
        function z(e, t) {
            const {
                element: { content: s },
                parts: r,
            } = e,
                i = document.createTreeWalker(s, $, null, !1);
            let n = B(r),
                o = r[n],
                a = -1,
                l = 0;
            const h = [];
            let d = null;
            for (; i.nextNode();) {
                a++;
                const e = i.currentNode;
                for (
                    e.previousSibling === d && (d = null),
                    t.has(e) && (h.push(e), null === d && (d = e)),
                    null !== d && l++;
                    void 0 !== o && o.index === a;

                )
                    (o.index = null !== d ? -1 : o.index - l), (o = r[(n = B(r, n))]);
            }
            h.forEach((e) => e.parentNode.removeChild(e));
        }
        const q = (e) => {
            let t = 11 === e.nodeType ? 0 : 1;
            const s = document.createTreeWalker(e, $, null, !1);
            for (; s.nextNode();) t++;
            return t;
        },
            B = (e, t = -1) => {
                for (let s = t + 1; s < e.length; s++) {
                    const t = e[s];
                    if (g(t)) return s;
                }
                return -1;
            };
        const F = (e, t) => `${e}--${t}`;
        let I = !0;
        void 0 === window.ShadyCSS
            ? (I = !1)
            : void 0 === window.ShadyCSS.prepareTemplateDom &&
            (console.warn(
                'Incompatible ShadyCSS version detected. Please update to at least @webcomponents/webcomponentsjs@2.0.2 and @webcomponents/shadycss@1.3.1.',
            ),
                (I = !1));
        const L = (e) => (t) => {
            const s = F(t.type, e);
            let r = U.get(s);
            void 0 === r &&
                ((r = { stringsArray: new WeakMap(), keyString: new Map() }),
                    U.set(s, r));
            let i = r.stringsArray.get(t.strings);
            if (void 0 !== i) return i;
            const n = t.strings.join(h);
            if (void 0 === (i = r.keyString.get(n))) {
                const s = t.getTemplateElement();
                I && window.ShadyCSS.prepareTemplateDom(s, e),
                    (i = new u(t, s)),
                    r.keyString.set(n, i);
            }
            return r.stringsArray.set(t.strings, i), i;
        },
            H = ['html', 'svg'],
            W = new Set(),
            J = (e, t, s) => {
                W.add(e);
                const r = s ? s.element : document.createElement('template'),
                    i = t.querySelectorAll('style'),
                    { length: n } = i;
                if (0 === n) return void window.ShadyCSS.prepareTemplateStyles(r, e);
                const o = document.createElement('style');
                for (let e = 0; e < n; e++) {
                    const t = i[e];
                    t.parentNode.removeChild(t), (o.textContent += t.textContent);
                }
                ((e) => {
                    H.forEach((t) => {
                        const s = U.get(F(t, e));
                        void 0 !== s &&
                            s.keyString.forEach((e) => {
                                const {
                                    element: { content: t },
                                } = e,
                                    s = new Set();
                                Array.from(t.querySelectorAll('style')).forEach((e) => {
                                    s.add(e);
                                }),
                                    z(e, s);
                            });
                    });
                })(e);
                const a = r.content;
                s
                    ? (function (e, t, s = null) {
                        const {
                            element: { content: r },
                            parts: i,
                        } = e;
                        if (null == s) return void r.appendChild(t);
                        const n = document.createTreeWalker(r, $, null, !1);
                        let o = B(i),
                            a = 0,
                            l = -1;
                        for (; n.nextNode();) {
                            for (
                                l++ ,
                                n.currentNode === s &&
                                ((a = q(t)), s.parentNode.insertBefore(t, s));
                                -1 !== o && i[o].index === l;

                            ) {
                                if (a > 0) {
                                    for (; -1 !== o;) (i[o].index += a), (o = B(i, o));
                                    return;
                                }
                                o = B(i, o);
                            }
                        }
                    })(s, o, a.firstChild)
                    : a.insertBefore(o, a.firstChild),
                    window.ShadyCSS.prepareTemplateStyles(r, e);
                const l = a.querySelector('style');
                if (window.ShadyCSS.nativeShadow && null !== l)
                    t.insertBefore(l.cloneNode(!0), t.firstChild);
                else if (s) {
                    a.insertBefore(o, a.firstChild);
                    const e = new Set();
                    e.add(o), z(s, e);
                }
            };
        window.JSCompiler_renameProperty = (e, t) => e;
        const D = {
            toAttribute(e, t) {
                switch (t) {
                    case Boolean:
                        return e ? '' : null;
                    case Object:
                    case Array:
                        return null == e ? e : JSON.stringify(e);
                }
                return e;
            },
            fromAttribute(e, t) {
                switch (t) {
                    case Boolean:
                        return null !== e;
                    case Number:
                        return null === e ? null : Number(e);
                    case Object:
                    case Array:
                        return JSON.parse(e);
                }
                return e;
            },
        },
            G = (e, t) => t !== e && (t == t || e == e),
            K = {
                attribute: !0,
                type: String,
                converter: D,
                reflect: !1,
                hasChanged: G,
            },
            Q = Promise.resolve(!0),
            X = 1,
            Y = 4,
            Z = 8,
            ee = 16,
            te = 32,
            se = 'finalized';
        class re extends HTMLElement {
            constructor() {
                super(),
                    (this._updateState = 0),
                    (this._instanceProperties = void 0),
                    (this._updatePromise = Q),
                    (this._hasConnectedResolver = void 0),
                    (this._changedProperties = new Map()),
                    (this._reflectingProperties = void 0),
                    this.initialize();
            }
            static get observedAttributes() {
                this.finalize();
                const e = [];
                return (
                    this._classProperties.forEach((t, s) => {
                        const r = this._attributeNameForProperty(s, t);
                        void 0 !== r && (this._attributeToPropertyMap.set(r, s), e.push(r));
                    }),
                    e
                );
            }
            static _ensureClassProperties() {
                if (
                    !this.hasOwnProperty(
                        JSCompiler_renameProperty('_classProperties', this),
                    )
                ) {
                    this._classProperties = new Map();
                    const e = Object.getPrototypeOf(this)._classProperties;
                    void 0 !== e && e.forEach((e, t) => this._classProperties.set(t, e));
                }
            }
            static createProperty(e, t = K) {
                if (
                    (this._ensureClassProperties(),
                        this._classProperties.set(e, t),
                        t.noAccessor || this.prototype.hasOwnProperty(e))
                )
                    return;
                const s = 'symbol' == typeof e ? Symbol() : `__${e}`;
                Object.defineProperty(this.prototype, e, {
                    get() {
                        return this[s];
                    },
                    set(t) {
                        const r = this[e];
                        (this[s] = t), this._requestUpdate(e, r);
                    },
                    configurable: !0,
                    enumerable: !0,
                });
            }
            static finalize() {
                const e = Object.getPrototypeOf(this);
                if (
                    (e.hasOwnProperty(se) || e.finalize(),
                        (this[se] = !0),
                        this._ensureClassProperties(),
                        (this._attributeToPropertyMap = new Map()),
                        this.hasOwnProperty(JSCompiler_renameProperty('properties', this)))
                ) {
                    const e = this.properties,
                        t = [
                            ...Object.getOwnPropertyNames(e),
                            ...('function' == typeof Object.getOwnPropertySymbols
                                ? Object.getOwnPropertySymbols(e)
                                : []),
                        ];
                    for (const s of t) this.createProperty(s, e[s]);
                }
            }
            static _attributeNameForProperty(e, t) {
                const s = t.attribute;
                return !1 === s
                    ? void 0
                    : 'string' == typeof s
                        ? s
                        : 'string' == typeof e
                            ? e.toLowerCase()
                            : void 0;
            }
            static _valueHasChanged(e, t, s = G) {
                return s(e, t);
            }
            static _propertyValueFromAttribute(e, t) {
                const s = t.type,
                    r = t.converter || D,
                    i = 'function' == typeof r ? r : r.fromAttribute;
                return i ? i(e, s) : e;
            }
            static _propertyValueToAttribute(e, t) {
                if (void 0 === t.reflect) return;
                const s = t.type,
                    r = t.converter;
                return ((r && r.toAttribute) || D.toAttribute)(e, s);
            }
            initialize() {
                this._saveInstanceProperties(), this._requestUpdate();
            }
            _saveInstanceProperties() {
                this.constructor._classProperties.forEach((e, t) => {
                    if (this.hasOwnProperty(t)) {
                        const e = this[t];
                        delete this[t],
                            this._instanceProperties ||
                            (this._instanceProperties = new Map()),
                            this._instanceProperties.set(t, e);
                    }
                });
            }
            _applyInstanceProperties() {
                this._instanceProperties.forEach((e, t) => (this[t] = e)),
                    (this._instanceProperties = void 0);
            }
            connectedCallback() {
                (this._updateState = this._updateState | te),
                    this._hasConnectedResolver &&
                    (this._hasConnectedResolver(),
                        (this._hasConnectedResolver = void 0));
            }
            disconnectedCallback() { }
            attributeChangedCallback(e, t, s) {
                t !== s && this._attributeToProperty(e, s);
            }
            _propertyToAttribute(e, t, s = K) {
                const r = this.constructor,
                    i = r._attributeNameForProperty(e, s);
                if (void 0 !== i) {
                    const e = r._propertyValueToAttribute(t, s);
                    if (void 0 === e) return;
                    (this._updateState = this._updateState | Z),
                        null == e ? this.removeAttribute(i) : this.setAttribute(i, e),
                        (this._updateState = this._updateState & ~Z);
                }
            }
            _attributeToProperty(e, t) {
                if (this._updateState & Z) return;
                const s = this.constructor,
                    r = s._attributeToPropertyMap.get(e);
                if (void 0 !== r) {
                    const e = s._classProperties.get(r) || K;
                    (this._updateState = this._updateState | ee),
                        (this[r] = s._propertyValueFromAttribute(t, e)),
                        (this._updateState = this._updateState & ~ee);
                }
            }
            _requestUpdate(e, t) {
                let s = !0;
                if (void 0 !== e) {
                    const r = this.constructor,
                        i = r._classProperties.get(e) || K;
                    r._valueHasChanged(this[e], t, i.hasChanged)
                        ? (this._changedProperties.has(e) ||
                            this._changedProperties.set(e, t),
                            !0 !== i.reflect ||
                            this._updateState & ee ||
                            (void 0 === this._reflectingProperties &&
                                (this._reflectingProperties = new Map()),
                                this._reflectingProperties.set(e, i)))
                        : (s = !1);
                }
                !this._hasRequestedUpdate && s && this._enqueueUpdate();
            }
            requestUpdate(e, t) {
                return this._requestUpdate(e, t), this.updateComplete;
            }
            async _enqueueUpdate() {
                let e, t;
                this._updateState = this._updateState | Y;
                const s = this._updatePromise;
                this._updatePromise = new Promise((s, r) => {
                    (e = s), (t = r);
                });
                try {
                    await s;
                } catch (e) { }
                this._hasConnected ||
                    (await new Promise((e) => (this._hasConnectedResolver = e)));
                try {
                    const e = this.performUpdate();
                    null != e && (await e);
                } catch (e) {
                    t(e);
                }
                e(!this._hasRequestedUpdate);
            }
            get _hasConnected() {
                return this._updateState & te;
            }
            get _hasRequestedUpdate() {
                return this._updateState & Y;
            }
            get hasUpdated() {
                return this._updateState & X;
            }
            performUpdate() {
                this._instanceProperties && this._applyInstanceProperties();
                let e = !1;
                const t = this._changedProperties;
                try {
                    (e = this.shouldUpdate(t)) && this.update(t);
                } catch (t) {
                    throw ((e = !1), t);
                } finally {
                    this._markUpdated();
                }
                e &&
                    (this._updateState & X ||
                        ((this._updateState = this._updateState | X), this.firstUpdated(t)),
                        this.updated(t));
            }
            _markUpdated() {
                (this._changedProperties = new Map()),
                    (this._updateState = this._updateState & ~Y);
            }
            get updateComplete() {
                return this._getUpdateComplete();
            }
            _getUpdateComplete() {
                return this._updatePromise;
            }
            shouldUpdate(e) {
                return !0;
            }
            update(e) {
                void 0 !== this._reflectingProperties &&
                    this._reflectingProperties.size > 0 &&
                    (this._reflectingProperties.forEach((e, t) =>
                        this._propertyToAttribute(t, this[t], e),
                    ),
                        (this._reflectingProperties = void 0));
            }
            updated(e) { }
            firstUpdated(e) { }
        }
        re[se] = !0;
        const ie =
            'adoptedStyleSheets' in Document.prototype &&
            'replace' in CSSStyleSheet.prototype,
            ne = Symbol();
        class oe {
            constructor(e, t) {
                if (t !== ne)
                    throw new Error(
                        'CSSResult is not constructable. Use `unsafeCSS` or `css` instead.',
                    );
                this.cssText = e;
            }
            get styleSheet() {
                return (
                    void 0 === this._styleSheet &&
                    (ie
                        ? ((this._styleSheet = new CSSStyleSheet()),
                            this._styleSheet.replaceSync(this.cssText))
                        : (this._styleSheet = null)),
                    this._styleSheet
                );
            }
            toString() {
                return this.cssText;
            }
        }
        const ae = (e, ...t) => {
            const s = t.reduce(
                (t, s, r) =>
                    t +
                    ((e) => {
                        if (e instanceof oe) return e.cssText;
                        if ('number' == typeof e) return e;
                        throw new Error(
                            `Value passed to 'css' function must be a 'css' function result: ${e}. Use 'unsafeCSS' to pass non-literal values, but\n            take care to ensure page security.`,
                        );
                    })(s) +
                    e[r + 1],
                e[0],
            );
            return new oe(s, ne);
        };
        (window.litElementVersions || (window.litElementVersions = [])).push(
            '2.2.1',
        );
        const le = (e) =>
            e.flat
                ? e.flat(1 / 0)
                : (function e(t, s = []) {
                    for (let r = 0, i = t.length; r < i; r++) {
                        const i = t[r];
                        Array.isArray(i) ? e(i, s) : s.push(i);
                    }
                    return s;
                })(e);
        class he extends re {
            static finalize() {
                super.finalize.call(this),
                    (this._styles = this.hasOwnProperty(
                        JSCompiler_renameProperty('styles', this),
                    )
                        ? this._getUniqueStyles()
                        : this._styles || []);
            }
            static _getUniqueStyles() {
                const e = this.styles,
                    t = [];
                if (Array.isArray(e)) {
                    le(e)
                        .reduceRight((e, t) => (e.add(t), e), new Set())
                        .forEach((e) => t.unshift(e));
                } else e && t.push(e);
                return t;
            }
            initialize() {
                super.initialize(),
                    (this.renderRoot = this.createRenderRoot()),
                    window.ShadowRoot &&
                    this.renderRoot instanceof window.ShadowRoot &&
                    this.adoptStyles();
            }
            createRenderRoot() {
                return this.attachShadow({ mode: 'open' });
            }
            adoptStyles() {
                const e = this.constructor._styles;
                0 !== e.length &&
                    (void 0 === window.ShadyCSS || window.ShadyCSS.nativeShadow
                        ? ie
                            ? (this.renderRoot.adoptedStyleSheets = e.map(
                                (e) => e.styleSheet,
                            ))
                            : (this._needsShimAdoptedStyleSheets = !0)
                        : window.ShadyCSS.ScopingShim.prepareAdoptedCssText(
                            e.map((e) => e.cssText),
                            this.localName,
                        ));
            }
            connectedCallback() {
                super.connectedCallback(),
                    this.hasUpdated &&
                    void 0 !== window.ShadyCSS &&
                    window.ShadyCSS.styleElement(this);
            }
            update(e) {
                super.update(e);
                const t = this.render();
                t instanceof b &&
                    this.constructor.render(t, this.renderRoot, {
                        scopeName: this.localName,
                        eventContext: this,
                    }),
                    this._needsShimAdoptedStyleSheets &&
                    ((this._needsShimAdoptedStyleSheets = !1),
                        this.constructor._styles.forEach((e) => {
                            const t = document.createElement('style');
                            (t.textContent = e.cssText), this.renderRoot.appendChild(t);
                        }));
            }
            render() { }
        }
        (he.finalized = !0),
            (he.render = (e, t, s) => {
                if (!s || 'object' != typeof s || !s.scopeName)
                    throw new Error('The `scopeName` option is required.');
                const r = s.scopeName,
                    i = R.has(t),
                    n = I && 11 === t.nodeType && !!t.host,
                    a = n && !W.has(r),
                    l = a ? document.createDocumentFragment() : t;
                if (
                    (((e, t, s) => {
                        let r = R.get(t);
                        void 0 === r &&
                            (o(t, t.firstChild),
                                R.set(t, (r = new N(Object.assign({ templateFactory: M }, s)))),
                                r.appendInto(t)),
                            r.setValue(e),
                            r.commit();
                    })(e, l, Object.assign({ templateFactory: L(r) }, s)),
                        a)
                ) {
                    const e = R.get(l);
                    R.delete(l);
                    const s = e.value instanceof y ? e.value.template : void 0;
                    J(r, l, s), o(t, t.firstChild), t.appendChild(l), R.set(t, e);
                }
                !i && n && window.ShadyCSS.styleElement(t.host);
            });
        customElements.define(
            'monkey-bar',
            class extends he {
                static get styles() {
                    //Manoj
                    return ae`:host{--white:hsl(216, 33%, 97%);--black:hsl(210, 24%, 16%);--progress-height:1.5rem;--progress-font-size:1rem;--progress-bg:hsl(214, 15%, 91%);--progress-border-radius:0.25rem;--progress-box-shadow:inset 0 1px 2px rgba(var(--black), 0.075);--progress-bar-bg:hsl(120,100%,25%);--progress-bar-color:var(--white);--progress-bar-transition:width 0.6s ease;--progress-bar-striped-bg:rgba(229, 232, 235, 0.15);--progress-bar-striped-angle:45deg;--progress-bar-animation-timing:1s linear infinite;background-color:var(--monkey-bg,var(--progress-bg));border-radius:var(--monkey-border-radius,var(--progress-border-radius));box-shadow:var(--monkey-box-shadow,var(--progress-box-shadow));display:flex;font-size:var(--monkey-font-size,var(--progress-font-size));font-weight:700;height:var(--monkey-height,var(--progress-height));line-height:var(--monkey-height,var(--progress-height));overflow:hidden}:host([hidden]){display:none!important}@keyframes progressbar-stripes{0%{background-position-x:var(--progress-height)}}.progressbar{background-color:var(--monkey-bar-bg,var(--progress-bar-bg));color:var(--monkey-bar-color,var(--progress-bar-color));display:flex;flex-direction:column;justify-content:center;overflow:hidden;text-align:center;transition:var(--monkey-bar-transition,var(--progress-bar-transition));white-space:nowrap}.progressbar--striped{background-image:linear-gradient(var(--monkey-bar-striped-angle,var(--progress-bar-striped-angle)),var(--monkey-bar-striped-bg,var(--progress-bar-striped-bg)) 25%,transparent 25%,transparent 50%,var(--monkey-bar-striped-bg,var(--progress-bar-striped-bg)) 50%,var(--monkey-bar-striped-bg,var(--progress-bar-striped-bg)) 75%,transparent 75%,transparent);background-size:var(--monkey-height,var(--progress-height)) var(--monkey-height,var(--progress-height))}.progressbar--animated{animation:progressbar-stripes var(--monkey-bar-animation-timing,var(--progress-bar-animation-timing))}`;
                }
                static get properties() {
                    return {
                        min: { type: Number, reflect: !0 },
                        max: { type: Number, reflect: !0 },
                        now: { type: Number, reflect: !0 },
                        labeled: { type: Boolean },
                        striped: { type: Boolean },
                        animated: { type: Boolean },
                    };
                }
                constructor() {
                    super(),
                        (this.min = 0),
                        (this.max = 100),
                        (this.now = 0),
                        (this.labeled = !1),
                        (this.striped = !1),
                        (this.animated = !1);
                }
                render() {
                    return j`<div class="progressbar ${
                        this.striped ? 'progressbar--striped' : ''
                        } ${
                        this.animated ? 'progressbar--animated' : ''
                        }" role="progressbar" style="width: ${this.now}%;" aria-valuemin="${
                        this.min
                        }" aria-valuemax="${this.max}" aria-valuenow="${this.now}">${
                        this.labeled ? `${this.now}%` : ''
                        }</div>`;
                }
            },
        );
    },
]);
//# sourceMappingURL=monkey-bar.min.js.map
