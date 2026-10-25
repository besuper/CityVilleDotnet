"use strict";(()=>{var Mr=Object.create;var ij=Object.defineProperty;var Lr=Object.getOwnPropertyDescriptor;var Ur=Object.getOwnPropertyNames;var Vr=Object.getPrototypeOf,Xr=Object.prototype.hasOwnProperty;var De=(r,A,e)=>()=>{if(e)throw e[0];try{return r&&(A=r(r=0)),A}catch(j){throw e=[j],j}};var V=(r,A)=>()=>{try{return A||r((A={exports:{}}).exports,A),A.exports}catch(e){throw A=0,e}},ca=(r,A)=>{for(var e in A)ij(r,e,{get:A[e],enumerable:!0})},zr=(r,A,e,j)=>{if(A&&typeof A=="object"||typeof A=="function")for(let a of Ur(A))!Xr.call(r,a)&&a!==e&&ij(r,a,{get:()=>A[a],enumerable:!(j=Lr(A,a))||j.enumerable});return r};var X=(r,A,e)=>(e=r!=null?Mr(Vr(r)):{},zr(A||!r||!r.__esModule?ij(e,"default",{value:r,enumerable:!0}):e,r));function Wr(){Object.defineProperty(Array.prototype,"reduce",{value(...r){if(r.length===0&&window.Prototype&&window.Prototype.Version&&window.Prototype.Version<"1.6.1")return this.length>1?this:this[0];let A=r[0];if(this===null)throw new TypeError("Array.prototype.reduce called on null or undefined");if(typeof A!="function")throw new TypeError(`${A} is not a function`);let e=Object(this),j=e.length>>>0,a=0,t;if(r.length>=2)t=r[1];else{for(;a<j&&!(a in e);)a++;if(a>=j)throw new TypeError("Reduce of empty array with no initial value");t=e[a++]}for(;a<j;)a in e&&(t=A(t,e[a],a,e)),a++;return t}})}function Nr(){typeof window.constructor!="function"||!PA(window.constructor)||(window.Window=window.constructor)}function Jr(){(window.Reflect===void 0||window.Reflect===null)&&(window.Reflect={}),typeof Reflect.get!="function"&&Object.defineProperty(Reflect,"get",{value(r,A){return r[A]}}),typeof Reflect.set!="function"&&Object.defineProperty(Reflect,"set",{value(r,A,e){r[A]=e}}),typeof Reflect.has!="function"&&Object.defineProperty(Reflect,"has",{value(r,A){return A in r}}),typeof Reflect.ownKeys!="function"&&Object.defineProperty(Reflect,"ownKeys",{value(r){return[...Object.getOwnPropertyNames(r),...Object.getOwnPropertySymbols(r)]}})}function PA(r){let A=typeof Function.prototype.toString=="function"?Function.prototype.toString():null;return typeof A=="string"&&A.indexOf("[native code]")>=0?Function.prototype.toString.call(r).indexOf("[native code]")>=0:!1}function fa(){(typeof Array.prototype.reduce!="function"||!PA(Array.prototype.reduce))&&Wr(),(typeof Window!="function"||!PA(Window))&&Nr(),Jr()}var bj=De(()=>{"use strict";l()});function Qr(){if(uj===null){let r=document.createElement("iframe");r.style.display="none",document.documentElement.append(r),uj={Map:r.contentWindow.Map},r.remove()}return uj}function Yr(r,A){let e=globalThis[r];return e!=null&&A(e)||typeof document>"u"?e:Qr()[r]}var uj,_,l=De(()=>{"use strict";bj();uj=null;_=Yr("Map",r=>PA(r)&&["get","set","has","delete","clear","forEach"].every(A=>typeof r.prototype[A]=="function"&&PA(r.prototype[A])))});var sa=V(na=>{"use strict";l();Object.defineProperty(na,"__esModule",{value:!0})});var da=V(la=>{"use strict";l();Object.defineProperty(la,"__esModule",{value:!0})});var ba=V(ia=>{"use strict";l();Object.defineProperty(ia,"__esModule",{value:!0})});var _a=V(ua=>{"use strict";l();Object.defineProperty(ua,"__esModule",{value:!0})});var Ea=V(pa=>{"use strict";l();Object.defineProperty(pa,"__esModule",{value:!0})});var Ba=V(_j=>{"use strict";l();Object.defineProperty(_j,"__esModule",{value:!0});_j.classnames=a6;var A6=r=>Object.entries(r).map(([A,e])=>e&&A),ma=r=>!!r,e6=(r,A,e)=>e.indexOf(r)===A,j6=[];function ga(r){return r?typeof r=="string"?[r]:Array.isArray(r)?r.flatMap(ga).filter(ma):A6(r).filter(ma):j6}function a6(r){let A=ga(r).filter(e6);return A.length>0?A.join(" "):void 0}});var ha=V(N=>{"use strict";l();var r6=N&&N.__createBinding||(Object.create?(function(r,A,e,j){j===void 0&&(j=e);var a=Object.getOwnPropertyDescriptor(A,e);(!a||("get"in a?!A.__esModule:a.writable||a.configurable))&&(a={enumerable:!0,get:function(){return A[e]}}),Object.defineProperty(r,j,a)}):(function(r,A,e,j){j===void 0&&(j=e),r[j]=A[e]})),HA=N&&N.__exportStar||function(r,A){for(var e in r)e!=="default"&&!Object.prototype.hasOwnProperty.call(A,e)&&r6(A,r,e)};Object.defineProperty(N,"__esModule",{value:!0});HA(sa(),N);HA(da(),N);HA(ba(),N);HA(_a(),N);HA(Ea(),N);HA(Ba(),N)});var Ej=V(pj=>{"use strict";l();Object.defineProperty(pj,"__esModule",{value:!0});pj.setAttributes=f6;var t6=ha();function o6(r,A){for(let e of Object.keys(r))e in A&&(A[e]=r[e])}var c6=/^on\p{Lu}/u;function f6(r,A){for(let e of Object.keys(A)){if(e==="__source"||e==="__self"||e==="tsxTag")continue;let j=A[e];if(e==="class"){let a=(0,t6.classnames)(j);a&&r.setAttribute(e,a)}else if(e==="ref")j.current=r;else if(c6.test(e)){let a=e.replace(/Capture$/,""),t=e!==a,o=a.toLowerCase().substring(2);r.addEventListener(o,j,t)}else e==="style"&&typeof j!="string"?o6(j,r.style):e==="dangerouslySetInnerHTML"?r.innerHTML=j:j===!0?r.setAttribute(e,e):(j||j===0||j==="")&&r.setAttribute(e,j.toString())}}});var mj=V(Ae=>{"use strict";l();Object.defineProperty(Ae,"__esModule",{value:!0});Ae.applyChildren=qa;Ae.createDomElement=s6;Ae.applyTsxTag=l6;function n6(r,A){A instanceof Element?r.appendChild(A):typeof A=="string"||typeof A=="number"?r.appendChild(document.createTextNode(A.toString())):console.warn("Unknown type to append: ",A)}function qa(r,A){for(let e of A)!e&&e!==0||(Array.isArray(e)?qa(r,e):n6(r,e))}function s6(r,A){let e=A?.is?{is:A.is}:void 0;return A?.xmlns?document.createElementNS(A.xmlns,r,e):document.createElement(r,e)}function l6(r,A){let e=r,j=A;return j&&"tsxTag"in j&&(e=j.tsxTag,!j.is&&r.includes("-")&&(j={...j,is:r})),{finalTag:e,finalAttrs:j}}});var M=V(ee=>{"use strict";l();Object.defineProperty(ee,"__esModule",{value:!0});ee.jsx=Bj;ee.jsxs=Bj;ee.jsxDEV=Bj;var d6=Ej(),gj=mj();function Bj(r,A){if(typeof r=="function")return r(A);let{children:e,...j}=A,{finalTag:a,finalAttrs:t}=(0,gj.applyTsxTag)(r,j),o=(0,gj.createDomElement)(a,t);return(0,d6.setAttributes)(o,t),(0,gj.applyChildren)(o,[e]),o}});var jr=V($A=>{"use strict";l();Object.defineProperty($A,"__esModule",{value:!0});$A.createRef=$A.h=void 0;$A.createElement=er;var N6=Ej(),Ij=mj();function er(r,A,...e){if(typeof r=="function")return r({...A,children:e});let{finalTag:j,finalAttrs:a}=(0,Ij.applyTsxTag)(r,A),t=(0,Ij.createDomElement)(j,a);return a&&(0,N6.setAttributes)(t,a),(0,Ij.applyChildren)(t,e),t}$A.h=er;var J6=()=>({current:null});$A.createRef=J6});var ar=V(Dj=>{"use strict";l();Object.defineProperty(Dj,"__esModule",{value:!0});Dj.defineCustomElement=Y6;var Q6=M();function Y6(r,A,e){return customElements.define(r,A,e),j=>(0,Q6.jsx)(r,j)}});var tr=V(rr=>{"use strict";l();Object.defineProperty(rr,"__esModule",{value:!0})});var or=V(oA=>{"use strict";l();var Z6=oA&&oA.__createBinding||(Object.create?(function(r,A,e,j){j===void 0&&(j=e);var a=Object.getOwnPropertyDescriptor(A,e);(!a||("get"in a?!A.__esModule:a.writable||a.configurable))&&(a={enumerable:!0,get:function(){return A[e]}}),Object.defineProperty(r,j,a)}):(function(r,A,e,j){j===void 0&&(j=e),r[j]=A[e]})),Qe=oA&&oA.__exportStar||function(r,A){for(var e in r)e!=="default"&&!Object.prototype.hasOwnProperty.call(A,e)&&Z6(A,r,e)};Object.defineProperty(oA,"__esModule",{value:!0});Qe(jr(),oA);Qe(ar(),oA);Qe(M(),oA);Qe(tr(),oA)});function ir(r,A){let e=r.length,j=r.getChannelData(0),a=r.getChannelData(1),t=0,o=0;for(;o<e;)j[o]=A[t],a[o]=A[t+1],o++,t+=2}function br(r,A){return new Function(`return (${r})(...arguments);`)(...A)}var ur=De(()=>{"use strict";l()});var Wj={};ca(Wj,{IntoUnderlyingByteSource:()=>_e,IntoUnderlyingSink:()=>pe,IntoUnderlyingSource:()=>Ee,RuffleHandle:()=>NA,RuffleInstanceBuilder:()=>me,ZipWriter:()=>ge,default:()=>po,global_init:()=>at,initSync:()=>_o});function at(){c.global_init()}function vr(){return{__proto__:null,"./ruffle_web_bg.js":{__proto__:null,__wbg_Error_408e67f47ca7b58b:function(A,e){return Error(d(A,e))},__wbg_Window_a2a6c4d665047b14:function(A){return A.Window},__wbg_WorkerGlobalScope_2664448a7c667d67:function(A){return A.WorkerGlobalScope},__wbg___wbindgen_add_d4e2ca36d51d4d09:function(A,e){return A+e},__wbg___wbindgen_boolean_get_c9c83ebd41b34df3:function(A){let e=A,j=typeof e=="boolean"?e:void 0;return p(j)?16777215:j?1:0},__wbg___wbindgen_debug_string_a57024b9c6e4a48b:function(A,e){let j=Vj(e),a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},__wbg___wbindgen_in_ac983077f137f2e6:function(A,e){return A in e},__wbg___wbindgen_is_function_5e4570eb24ffa122:function(A){return typeof A=="function"},__wbg___wbindgen_is_null_7d13f41e1a2d5140:function(A){return A===null},__wbg___wbindgen_is_string_e6f02f0ea5f20a32:function(A){return typeof A=="string"},__wbg___wbindgen_is_undefined_6cff064c44e0d823:function(A){return A===void 0},__wbg___wbindgen_number_get_136b9679cab35cfb:function(A,e){let j=e,a=typeof j=="number"?j:void 0;m().setFloat64(A+8,p(a)?0:a,!0),m().setInt32(A+0,!p(a),!0)},__wbg___wbindgen_string_get_d154f1e671052120:function(A,e){let j=e,a=typeof j=="string"?j:void 0;var t=p(a)?0:G(a,c.__wbindgen_malloc,c.__wbindgen_realloc),o=g;m().setInt32(A+4,o,!0),m().setInt32(A+0,t,!0)},__wbg___wbindgen_throw_bb96b2010945f0bc:function(A,e){throw new Error(d(A,e))},__wbg__wbg_cb_unref_be22cc64ae6946a0:function(A){A._wbg_cb_unref()},__wbg_a_50b8aa2c55aab913:function(A){return A.a},__wbg_activeTexture_8e65ac2e8d488478:function(A,e){A.activeTexture(e>>>0)},__wbg_activeTexture_fd6262686afdbe2f:function(A,e){A.activeTexture(e>>>0)},__wbg_actualBoundingBoxAscent_4dcab656e4a31f96:function(A){return A.actualBoundingBoxAscent},__wbg_actualBoundingBoxDescent_77c46a72f390cca8:function(A){return A.actualBoundingBoxDescent},__wbg_actualBoundingBoxLeft_862e432cc4b67b21:function(A){return A.actualBoundingBoxLeft},__wbg_actualBoundingBoxRight_7dc82a3b2c564418:function(A){return A.actualBoundingBoxRight},__wbg_addColorStop_35d831fa917ffcd4:function(){return n(function(A,e,j,a){A.addColorStop(e,d(j,a))},arguments)},__wbg_addEventListener_3b8edc02c33d9f77:function(){return n(function(A,e,j,a){A.addEventListener(d(e,j),a)},arguments)},__wbg_addEventListener_d6fb728fba6ad35c:function(){return n(function(A,e,j,a,t){A.addEventListener(d(e,j),a,t)},arguments)},__wbg_addPath_2b5ccbd0d0049498:function(A,e,j){A.addPath(e,j)},__wbg_appendChild_d5cbce3d5fa81471:function(){return n(function(A,e){return A.appendChild(e)},arguments)},__wbg_arrayBuffer_16433f17fbd74397:function(){return n(function(A){return A.arrayBuffer()},arguments)},__wbg_assign_b4bc9b9355dde46c:function(){return n(function(A,e,j){A.assign(d(e,j))},arguments)},__wbg_attachShader_26751604f00d1f1b:function(A,e,j){A.attachShader(e,j)},__wbg_attachShader_61baa58641ea664a:function(A,e,j){A.attachShader(e,j)},__wbg_b_e13835841694635f:function(A){return A.b},__wbg_baseURI_2009585b672a389a:function(){return n(function(A,e){let j=e.baseURI;var a=p(j)?0:G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},arguments)},__wbg_beginQuery_444a51812fdbf958:function(A,e,j){A.beginQuery(e>>>0,j)},__wbg_beginRenderPass_3c53642423af50dc:function(){return n(function(A,e){return A.beginRenderPass(e)},arguments)},__wbg_bezierCurveTo_6c962e111be3f1d0:function(A,e,j,a,t,o,f){A.bezierCurveTo(e,j,a,t,o,f)},__wbg_bindAttribLocation_1e182a50e1556784:function(A,e,j,a,t){A.bindAttribLocation(e,j>>>0,d(a,t))},__wbg_bindAttribLocation_9cc5ab15df1d042d:function(A,e,j,a,t){A.bindAttribLocation(e,j>>>0,d(a,t))},__wbg_bindBufferRange_5a8d28ef662d8746:function(A,e,j,a,t,o){A.bindBufferRange(e>>>0,j>>>0,a,t,o)},__wbg_bindBuffer_1fb12d083d2a22af:function(A,e,j){A.bindBuffer(e>>>0,j)},__wbg_bindBuffer_31cb159ab5dc5ba7:function(A,e,j){A.bindBuffer(e>>>0,j)},__wbg_bindFramebuffer_32ce672324ce8a16:function(A,e,j){A.bindFramebuffer(e>>>0,j)},__wbg_bindFramebuffer_e620067056f9316f:function(A,e,j){A.bindFramebuffer(e>>>0,j)},__wbg_bindRenderbuffer_765cffe23b9c36f7:function(A,e,j){A.bindRenderbuffer(e>>>0,j)},__wbg_bindRenderbuffer_9b313332bd7aa049:function(A,e,j){A.bindRenderbuffer(e>>>0,j)},__wbg_bindSampler_28b0a4c34c6f96d4:function(A,e,j){A.bindSampler(e>>>0,j)},__wbg_bindTexture_4c54ffb64c33564f:function(A,e,j){A.bindTexture(e>>>0,j)},__wbg_bindTexture_6fe86367f6be8f59:function(A,e,j){A.bindTexture(e>>>0,j)},__wbg_bindVertexArrayOES_96a4898652eac0d8:function(A,e){A.bindVertexArrayOES(e)},__wbg_bindVertexArray_0185d931d681d806:function(A,e){A.bindVertexArray(e)},__wbg_blendColor_402572bc445d3ac3:function(A,e,j,a,t){A.blendColor(e,j,a,t)},__wbg_blendColor_af92968fedc595b1:function(A,e,j,a,t){A.blendColor(e,j,a,t)},__wbg_blendEquationSeparate_5ab35e46e7f48717:function(A,e,j){A.blendEquationSeparate(e>>>0,j>>>0)},__wbg_blendEquationSeparate_9ad084e8266b8e3c:function(A,e,j){A.blendEquationSeparate(e>>>0,j>>>0)},__wbg_blendEquation_4bab539169e7e865:function(A,e){A.blendEquation(e>>>0)},__wbg_blendEquation_502ed4c6af5bf8ee:function(A,e){A.blendEquation(e>>>0)},__wbg_blendFuncSeparate_2e4d259caaba517e:function(A,e,j,a,t){A.blendFuncSeparate(e>>>0,j>>>0,a>>>0,t>>>0)},__wbg_blendFuncSeparate_66688b15ecc6529c:function(A,e,j,a,t){A.blendFuncSeparate(e>>>0,j>>>0,a>>>0,t>>>0)},__wbg_blendFunc_b7f382e97db2fd5b:function(A,e,j){A.blendFunc(e>>>0,j>>>0)},__wbg_blendFunc_d908118bbb181928:function(A,e,j){A.blendFunc(e>>>0,j>>>0)},__wbg_blitFramebuffer_20b32de88a3097b1:function(A,e,j,a,t,o,f,s,i,b,E){A.blitFramebuffer(e,j,a,t,o,f,s,i,b>>>0,E>>>0)},__wbg_body_d6eca0586d628e3c:function(A){let e=A.body;return p(e)?0:B(e)},__wbg_body_eb2e7e7701fa47ae:function(A){let e=A.body;return p(e)?0:B(e)},__wbg_bufferData_1dd2939db2d88d82:function(A,e,j,a){A.bufferData(e>>>0,j,a>>>0)},__wbg_bufferData_69a44ade0864ba2b:function(A,e,j,a){A.bufferData(e>>>0,j,a>>>0)},__wbg_bufferData_6c10d3e07ec9a2a9:function(A,e,j,a){A.bufferData(e>>>0,j,a>>>0)},__wbg_bufferData_bd2b8bde42f33479:function(A,e,j,a,t){A.bufferData(e>>>0,jA(j,a),t>>>0)},__wbg_bufferData_d359d1c797b8e8b7:function(A,e,j,a){A.bufferData(e>>>0,j,a>>>0)},__wbg_bufferSubData_4f6063d50303b61d:function(A,e,j,a){A.bufferSubData(e>>>0,j,a)},__wbg_bufferSubData_64b69f468a0d3048:function(A,e,j,a){A.bufferSubData(e>>>0,j,a)},__wbg_buffer_78291c0e094ccf99:function(A){return A.buffer},__wbg_button_3963e81aec2b2f60:function(A){return A.button},__wbg_buttons_4a8c6d3d822b6038:function(A){return A.buttons},__wbg_byobRequest_f8b1c89429b77545:function(A){let e=A.byobRequest;return p(e)?0:B(e)},__wbg_byteLength_336bc7d303511ba0:function(A){return A.byteLength},__wbg_byteOffset_2b1d5b10453ce198:function(A){return A.byteOffset},__wbg_c_c811405a34442426:function(A){return A.c},__wbg_callExternalInterface_b1baa2c35fc9e03d:function(){return n(function(A,e,j,a){var t=Zt(j,a);return c.__wbindgen_free(j,a*4,4),br(d(A,e),t)},arguments)},__wbg_callFSCommand_8ef4e8158c6bcd25:function(){return n(function(A,e,j,a,t){return A.callFSCommand(d(e,j),d(a,t))},arguments)},__wbg_call_1c5886ab9c57d1c7:function(){return n(function(A,e){return A.call(e)},arguments)},__wbg_call_35dba3c747ad7521:function(){return n(function(A,e,j){return A.call(e,j)},arguments)},__wbg_cancelAnimationFrame_58acec8573d45a99:function(){return n(function(A,e){A.cancelAnimationFrame(e)},arguments)},__wbg_clearBufferfv_ccbb43fb098f1912:function(A,e,j,a,t){A.clearBufferfv(e>>>0,j,R(a,t))},__wbg_clearBufferiv_8b1c68299632478f:function(A,e,j,a,t){A.clearBufferiv(e>>>0,j,_A(a,t))},__wbg_clearBufferuiv_cd72147d09d432e8:function(A,e,j,a,t){A.clearBufferuiv(e>>>0,j,zA(a,t))},__wbg_clearColor_c4271a8227ced504:function(A,e,j,a,t){A.clearColor(e,j,a,t)},__wbg_clearDepth_887000180cc9eb2e:function(A,e){A.clearDepth(e)},__wbg_clearDepth_c4897278afd894a9:function(A,e){A.clearDepth(e)},__wbg_clearRect_66721231b69373f5:function(A,e,j,a,t){A.clearRect(e,j,a,t)},__wbg_clearRect_81c3c80fbe793b63:function(A,e,j,a,t){A.clearRect(e,j,a,t)},__wbg_clearStencil_3d39149452a2f872:function(A,e){A.clearStencil(e)},__wbg_clearStencil_96978923f9c6fb1f:function(A,e){A.clearStencil(e)},__wbg_clear_20f7614cd20df101:function(A,e){A.clear(e>>>0)},__wbg_clear_332f205d7e52df87:function(A,e){A.clear(e>>>0)},__wbg_click_cdf5981a6746a4b8:function(A){A.click()},__wbg_clientHeight_834c029be3d903a7:function(A){return A.clientHeight},__wbg_clientWaitSync_8800b42d1c534e00:function(A,e,j,a){return A.clientWaitSync(e,j>>>0,a>>>0)},__wbg_clientWidth_ad03e8eb6c2b0c56:function(A){return A.clientWidth},__wbg_clip_186ecc3c70af5766:function(A,e,j){A.clip(e,pr[j])},__wbg_clipboardData_05651f46357b67bc:function(A){let e=A.clipboardData;return p(e)?0:B(e)},__wbg_clipboard_4fea7f044e5b8637:function(A){return A.clipboard},__wbg_closePath_4580feb19a1218cc:function(A){A.closePath()},__wbg_closeVirtualKeyboard_de8246828bdc4c0c:function(A){A.closeVirtualKeyboard()},__wbg_close_0a7ad9b918faec6d:function(){return n(function(A,e){A.close(e)},arguments)},__wbg_close_4d8c26ce7459660f:function(){return n(function(A){return A.close()},arguments)},__wbg_close_7292def578949963:function(){return n(function(A,e,j,a){A.close(e,d(j,a))},arguments)},__wbg_close_72f69f5f2de2bc73:function(){return n(function(A){A.close()},arguments)},__wbg_close_923aebe6bdeee300:function(A){A.close()},__wbg_close_97cdb44c3a7878f6:function(){return n(function(A){A.close()},arguments)},__wbg_close_b857478a8d4c1a16:function(){return n(function(A){A.close()},arguments)},__wbg_code_1bac1fd03147d97e:function(A,e){let j=e.code,a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},__wbg_code_e2719108dd8e1fec:function(A){return A.code},__wbg_colorMask_5646450fe1f1b723:function(A,e,j,a,t){A.colorMask(e!==0,j!==0,a!==0,t!==0)},__wbg_colorMask_8fca508f44773327:function(A,e,j,a,t){A.colorMask(e!==0,j!==0,a!==0,t!==0)},__wbg_compileShader_4ede19e4fc1bebce:function(A,e){A.compileShader(e)},__wbg_compileShader_ac457ada9042f08e:function(A,e){A.compileShader(e)},__wbg_compressedTexSubImage2D_0968a85385b7c463:function(A,e,j,a,t,o,f,s,i,b){A.compressedTexSubImage2D(e>>>0,j,a,t,o,f,s>>>0,i,b)},__wbg_compressedTexSubImage2D_45987d7f0210d36f:function(A,e,j,a,t,o,f,s,i){A.compressedTexSubImage2D(e>>>0,j,a,t,o,f,s>>>0,i)},__wbg_compressedTexSubImage2D_a39446fce0a68ad9:function(A,e,j,a,t,o,f,s,i){A.compressedTexSubImage2D(e>>>0,j,a,t,o,f,s>>>0,i)},__wbg_compressedTexSubImage3D_3da84908295b8ec3:function(A,e,j,a,t,o,f,s,i,b,E,h){A.compressedTexSubImage3D(e>>>0,j,a,t,o,f,s,i,b>>>0,E,h)},__wbg_compressedTexSubImage3D_c0bc017057e3942a:function(A,e,j,a,t,o,f,s,i,b,E){A.compressedTexSubImage3D(e>>>0,j,a,t,o,f,s,i,b>>>0,E)},__wbg_configure_1e2c1c9edad07d26:function(){return n(function(A,e){A.configure(e)},arguments)},__wbg_configure_3edaaed280bc6de8:function(){return n(function(A,e){A.configure(e)},arguments)},__wbg_confirm_f1128d5b70df2707:function(){return n(function(A,e,j){return A.confirm(d(e,j))},arguments)},__wbg_connect_d2a36cf1f5a1ec54:function(){return n(function(A,e){return A.connect(e)},arguments)},__wbg_contains_3ba0161eb6906b95:function(A,e){return A.contains(e)},__wbg_copyBufferSubData_e5dc2aab90456f99:function(A,e,j,a,t,o){A.copyBufferSubData(e>>>0,j>>>0,a,t,o)},__wbg_copyBufferToBuffer_01766818654a9868:function(){return n(function(A,e,j,a,t){A.copyBufferToBuffer(e,j,a,t)},arguments)},__wbg_copyBufferToBuffer_9c174b96fb08d551:function(){return n(function(A,e,j,a,t,o){A.copyBufferToBuffer(e,j,a,t,o)},arguments)},__wbg_copyBufferToTexture_ff632a21ab3fe3a7:function(){return n(function(A,e,j,a){A.copyBufferToTexture(e,j,a)},arguments)},__wbg_copyTexSubImage2D_188da734d1c8aa07:function(A,e,j,a,t,o,f,s,i){A.copyTexSubImage2D(e>>>0,j,a,t,o,f,s,i)},__wbg_copyTexSubImage2D_84d99fa40fabace0:function(A,e,j,a,t,o,f,s,i){A.copyTexSubImage2D(e>>>0,j,a,t,o,f,s,i)},__wbg_copyTexSubImage3D_89064e67340a38b3:function(A,e,j,a,t,o,f,s,i,b){A.copyTexSubImage3D(e>>>0,j,a,t,o,f,s,i,b)},__wbg_copyTextureToBuffer_1234b3210431ad05:function(){return n(function(A,e,j,a){A.copyTextureToBuffer(e,j,a)},arguments)},__wbg_copyTextureToTexture_d2e6a1eb3254b828:function(){return n(function(A,e,j,a){A.copyTextureToTexture(e,j,a)},arguments)},__wbg_copyToAudioBufferInterleaved_27100366a6705d65:function(A,e,j){ir(A,R(e,j))},__wbg_copyTo_394d7e9635015a1f:function(A,e,j){return A.copyTo(jA(e,j))},__wbg_createBindGroupLayout_b1bd63b4e88459d8:function(){return n(function(A,e){return A.createBindGroupLayout(e)},arguments)},__wbg_createBindGroup_f539b26ca341308f:function(A,e){return A.createBindGroup(e)},__wbg_createBufferSource_3679674c3bfc1e4e:function(){return n(function(A){return A.createBufferSource()},arguments)},__wbg_createBuffer_44b37c222efbd326:function(A){let e=A.createBuffer();return p(e)?0:B(e)},__wbg_createBuffer_9b192707f1e81570:function(){return n(function(A,e,j,a){return A.createBuffer(e>>>0,j>>>0,a)},arguments)},__wbg_createBuffer_af6c411fe2b091f8:function(A){let e=A.createBuffer();return p(e)?0:B(e)},__wbg_createBuffer_d800e9b1d41b2ee5:function(){return n(function(A,e){return A.createBuffer(e)},arguments)},__wbg_createCommandEncoder_3352d1ffc36c6fc0:function(A,e){return A.createCommandEncoder(e)},__wbg_createElementNS_f18ede2d74f15ea1:function(){return n(function(A,e,j,a,t){return A.createElementNS(e===0?void 0:d(e,j),d(a,t))},arguments)},__wbg_createElement_7f42344eee7bb810:function(){return n(function(A,e,j){return A.createElement(d(e,j))},arguments)},__wbg_createFramebuffer_4dc2fb6bd93463a5:function(A){let e=A.createFramebuffer();return p(e)?0:B(e)},__wbg_createFramebuffer_e6d8917bf9291c65:function(A){let e=A.createFramebuffer();return p(e)?0:B(e)},__wbg_createLinearGradient_d16f7c26c44e0b0a:function(A,e,j,a,t){return A.createLinearGradient(e,j,a,t)},__wbg_createObjectURL_da379bd6bf9a91c6:function(){return n(function(A,e){let j=URL.createObjectURL(e),a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},arguments)},__wbg_createPattern_f95263b497f37f3c:function(){return n(function(A,e,j,a){let t=A.createPattern(e,d(j,a));return p(t)?0:B(t)},arguments)},__wbg_createPipelineLayout_6eab52c327118937:function(A,e){return A.createPipelineLayout(e)},__wbg_createProgram_2ebbd17565e0ede7:function(A){let e=A.createProgram();return p(e)?0:B(e)},__wbg_createProgram_81b37242eadef893:function(A){let e=A.createProgram();return p(e)?0:B(e)},__wbg_createQuerySet_2dc8cde53df9849d:function(){return n(function(A,e){return A.createQuerySet(e)},arguments)},__wbg_createQuery_5ef5edffbd3a678d:function(A){let e=A.createQuery();return p(e)?0:B(e)},__wbg_createRadialGradient_c816f53e6e0afb32:function(){return n(function(A,e,j,a,t,o,f){return A.createRadialGradient(e,j,a,t,o,f)},arguments)},__wbg_createRenderPipeline_0ebb7ebc653e9207:function(){return n(function(A,e){return A.createRenderPipeline(e)},arguments)},__wbg_createRenderbuffer_be624f81e06a0cfd:function(A){let e=A.createRenderbuffer();return p(e)?0:B(e)},__wbg_createRenderbuffer_cd2638d5dda9c277:function(A){let e=A.createRenderbuffer();return p(e)?0:B(e)},__wbg_createSampler_9bd91d7e928c0060:function(A,e){return A.createSampler(e)},__wbg_createSampler_f1aedbf47c21745a:function(A){let e=A.createSampler();return p(e)?0:B(e)},__wbg_createShaderModule_cefa51336cb288ae:function(A,e){return A.createShaderModule(e)},__wbg_createShader_9a8e5f335caac850:function(A,e){let j=A.createShader(e>>>0);return p(j)?0:B(j)},__wbg_createShader_f8638cf4c19a1d2d:function(A,e){let j=A.createShader(e>>>0);return p(j)?0:B(j)},__wbg_createTexture_42c791197006c64a:function(A){let e=A.createTexture();return p(e)?0:B(e)},__wbg_createTexture_c74740f68b5c2a93:function(A){let e=A.createTexture();return p(e)?0:B(e)},__wbg_createTexture_ed7e9fc04dd54d84:function(){return n(function(A,e){return A.createTexture(e)},arguments)},__wbg_createVertexArrayOES_f7e8c94194c4e075:function(A){let e=A.createVertexArrayOES();return p(e)?0:B(e)},__wbg_createVertexArray_abd18ded26b75653:function(A){let e=A.createVertexArray();return p(e)?0:B(e)},__wbg_createView_da41c2d2cb212715:function(){return n(function(A,e){return A.createView(e)},arguments)},__wbg_ctrlKey_8f6cb44d63052c81:function(A){return A.ctrlKey},__wbg_cullFace_053fc24c214cae86:function(A,e){A.cullFace(e>>>0)},__wbg_cullFace_94e1cd382e8b654f:function(A,e){A.cullFace(e>>>0)},__wbg_currentTarget_81d519ad9e5a92ec:function(A){let e=A.currentTarget;return p(e)?0:B(e)},__wbg_currentTime_5594ee0e8ef1889a:function(A){return A.currentTime},__wbg_d_fda8f6ed85d1e057:function(A){return A.d},__wbg_data_51774c2dcd0a0e9f:function(A,e){let j=e.data,a=zj(j,c.__wbindgen_malloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},__wbg_data_57d8ce4eb5f0a433:function(A){return A.data},__wbg_decodeQueueSize_cc0c71f3c63c501b:function(A){return A.decodeQueueSize},__wbg_decode_cba6160770a46397:function(){return n(function(A,e){A.decode(e)},arguments)},__wbg_deleteBuffer_42bd497a20b76d88:function(A,e){A.deleteBuffer(e)},__wbg_deleteBuffer_50f20219abee4d05:function(A,e){A.deleteBuffer(e)},__wbg_deleteFramebuffer_073235a01c2a0a28:function(A,e){A.deleteFramebuffer(e)},__wbg_deleteFramebuffer_07fcc16563d17920:function(A,e){A.deleteFramebuffer(e)},__wbg_deleteProgram_0191056307686073:function(A,e){A.deleteProgram(e)},__wbg_deleteProgram_ee7f1925cb856dc2:function(A,e){A.deleteProgram(e)},__wbg_deleteQuery_4624acbf9cbfc6e2:function(A,e){A.deleteQuery(e)},__wbg_deleteRenderbuffer_570117534d9608a1:function(A,e){A.deleteRenderbuffer(e)},__wbg_deleteRenderbuffer_ba4a805dfac20358:function(A,e){A.deleteRenderbuffer(e)},__wbg_deleteSampler_527e8d31f81669d9:function(A,e){A.deleteSampler(e)},__wbg_deleteShader_2558228a4ef7373e:function(A,e){A.deleteShader(e)},__wbg_deleteShader_413961eb94f5c67c:function(A,e){A.deleteShader(e)},__wbg_deleteSync_11f80510355180d6:function(A,e){A.deleteSync(e)},__wbg_deleteTexture_0ccd278d6db819ff:function(A,e){A.deleteTexture(e)},__wbg_deleteTexture_aadf9716c394d7be:function(A,e){A.deleteTexture(e)},__wbg_deleteVertexArrayOES_e43a9a425587d52b:function(A,e){A.deleteVertexArrayOES(e)},__wbg_deleteVertexArray_106030034355d246:function(A,e){A.deleteVertexArray(e)},__wbg_delete_daeb0136382e63b0:function(){return n(function(A,e,j){delete A[d(e,j)]},arguments)},__wbg_deltaMode_1eedd4132dd540ba:function(A){return A.deltaMode},__wbg_deltaY_13780a1f1e6d6f8c:function(A){return A.deltaY},__wbg_depthFunc_6c6f948417f5bde4:function(A,e){A.depthFunc(e>>>0)},__wbg_depthFunc_bb3152f635a60ff2:function(A,e){A.depthFunc(e>>>0)},__wbg_depthMask_4e0075e07739355b:function(A,e){A.depthMask(e!==0)},__wbg_depthMask_bdc57b9e64c6b4d8:function(A,e){A.depthMask(e!==0)},__wbg_depthRange_0acaf3031a92d51d:function(A,e,j){A.depthRange(e,j)},__wbg_depthRange_e2d0a59942d33efd:function(A,e,j){A.depthRange(e,j)},__wbg_description_83b8a393160021b9:function(A,e){let j=e.description,a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},__wbg_destination_f6ba56e7f07829d0:function(A){return A.destination},__wbg_destroy_637537007d9eaa44:function(A){A.destroy()},__wbg_devicePixelRatio_e60a2d12bfd01f78:function(A){return A.devicePixelRatio},__wbg_disableVertexAttribArray_98752beca840c3da:function(A,e){A.disableVertexAttribArray(e>>>0)},__wbg_disableVertexAttribArray_aee51b7f1a8ef4cc:function(A,e){A.disableVertexAttribArray(e>>>0)},__wbg_disable_2ad210ba5315372a:function(A,e){A.disable(e>>>0)},__wbg_disable_bb1df5a6c75eaecd:function(A,e){A.disable(e>>>0)},__wbg_dispatchEvent_d63878ba8477faa4:function(){return n(function(A,e){return A.dispatchEvent(e)},arguments)},__wbg_displayClipboardModal_f63cbdc84444d0d4:function(A,e){A.displayClipboardModal(e!==0)},__wbg_displayMessage_c482334a9809d617:function(A,e,j){A.displayMessage(d(e,j))},__wbg_displayRestoredFromBfcacheMessage_8fa6f1f5ab97bab2:function(A){A.displayRestoredFromBfcacheMessage()},__wbg_displayRootMovieDownloadFailedMessage_1b51b531de8bd161:function(A,e,j,a){let t,o;try{t=j,o=a,A.displayRootMovieDownloadFailedMessage(e!==0,d(j,a))}finally{c.__wbindgen_free(t,o,1)}},__wbg_displayUnsupportedVideo_4ba98d735674b2a8:function(A,e,j){A.displayUnsupportedVideo(d(e,j))},__wbg_document_ac38448dbfd31a57:function(A){let e=A.document;return p(e)?0:B(e)},__wbg_done_669171204c3dcae2:function(A){return A.done},__wbg_drawArraysInstancedANGLE_cb3b87925641d5b9:function(A,e,j,a,t){A.drawArraysInstancedANGLE(e>>>0,j,a,t)},__wbg_drawArraysInstanced_45317b22bbf7ffe8:function(A,e,j,a,t){A.drawArraysInstanced(e>>>0,j,a,t)},__wbg_drawArrays_02c354e377984441:function(A,e,j,a){A.drawArrays(e>>>0,j,a)},__wbg_drawArrays_b2004a40c212065c:function(A,e,j,a){A.drawArrays(e>>>0,j,a)},__wbg_drawBuffersWEBGL_0b4935290cba977e:function(A,e){A.drawBuffersWEBGL(e)},__wbg_drawBuffers_f07f796e50bb0077:function(A,e){A.drawBuffers(e)},__wbg_drawElementsInstancedANGLE_8179cb41f5862831:function(A,e,j,a,t,o){A.drawElementsInstancedANGLE(e>>>0,j,a>>>0,t,o)},__wbg_drawElementsInstanced_07717eeb890435e9:function(A,e,j,a,t,o){A.drawElementsInstanced(e>>>0,j,a>>>0,t,o)},__wbg_drawElements_39fd9be525b4845b:function(A,e,j,a,t){A.drawElements(e>>>0,j,a>>>0,t)},__wbg_drawImage_87a05b54f458ec06:function(){return n(function(A,e,j,a,t,o,f,s,i,b){A.drawImage(e,j,a,t,o,f,s,i,b)},arguments)},__wbg_drawIndexed_638959aae942557c:function(A,e,j,a,t,o){A.drawIndexed(e>>>0,j>>>0,a>>>0,t,o>>>0)},__wbg_drawingBufferHeight_9574a81ca1940829:function(A){return A.drawingBufferHeight},__wbg_drawingBufferWidth_95a3d167a67d18dc:function(A){return A.drawingBufferWidth},__wbg_e_59a2a263244aebfc:function(A){return A.e},__wbg_enableVertexAttribArray_90f1a9f570379c36:function(A,e){A.enableVertexAttribArray(e>>>0)},__wbg_enableVertexAttribArray_b072ffcbe4f26e2b:function(A,e){A.enableVertexAttribArray(e>>>0)},__wbg_enable_17346ff3b2257cae:function(A,e){A.enable(e>>>0)},__wbg_enable_db1e433ea267f29b:function(A,e){A.enable(e>>>0)},__wbg_endQuery_0434371d408e59b7:function(A,e){A.endQuery(e>>>0)},__wbg_end_b57473834b877409:function(A){A.end()},__wbg_enqueue_7d68a21eda78e72f:function(){return n(function(A,e){A.enqueue(e)},arguments)},__wbg_entries_7774d489e1da5f4f:function(A){return Object.entries(A)},__wbg_error_757e9472f8410341:function(A,e){let j,a;try{j=A,a=e,console.error(d(A,e))}finally{c.__wbindgen_free(j,a,1)}},__wbg_execCommand_cd03aa5ebc21f204:function(){return n(function(A,e,j){return A.execCommand(d(e,j))},arguments)},__wbg_f_ea46ea3f61f48c32:function(A){return A.f},__wbg_features_5cac120c28ba0475:function(A){return A.features},__wbg_features_dec7bd2fd3d91bd6:function(A){return A.features},__wbg_fenceSync_57ab30f550e5a5a2:function(A,e,j){let a=A.fenceSync(e>>>0,j>>>0);return p(a)?0:B(a)},__wbg_fetch_729fad2e5272298f:function(A,e){return A.fetch(e)},__wbg_files_56a897754f75826b:function(A){let e=A.files;return p(e)?0:B(e)},__wbg_fillRect_3077c0e38eb34cd1:function(A,e,j,a,t){A.fillRect(e,j,a,t)},__wbg_fillText_1b1e3dfee622d89d:function(){return n(function(A,e,j,a,t){A.fillText(d(e,j),a,t)},arguments)},__wbg_fill_99bc71dde47c30ec:function(A,e,j){A.fill(e,pr[j])},__wbg_finish_09ec094c10f41e7b:function(A){return A.finish()},__wbg_finish_81c066eb195fc8a9:function(A){A.finish()},__wbg_finish_94865fee5c90da6b:function(A){A.finish()},__wbg_finish_ec1c191f66a895b1:function(A,e){return A.finish(e)},__wbg_flush_2a8fa6766a4f3ada:function(A){A.flush()},__wbg_flush_918ffb9cfebcbaab:function(A){A.flush()},__wbg_focus_77d7483c7b2b9f30:function(){return n(function(A){A.focus()},arguments)},__wbg_focus_c7d4fe3aba923a18:function(){return n(function(A,e){A.focus(e)},arguments)},__wbg_fontBoundingBoxAscent_c77b10412fdb331d:function(A){return A.fontBoundingBoxAscent},__wbg_fontBoundingBoxDescent_63ee2689f66ed207:function(A){return A.fontBoundingBoxDescent},__wbg_format_41beb9ccd4250e97:function(A){let e=A.format;return p(e)?24:(Xt.indexOf(e)+1||24)-1},__wbg_framebufferRenderbuffer_5736a8553be94035:function(A,e,j,a,t){A.framebufferRenderbuffer(e>>>0,j>>>0,a>>>0,t)},__wbg_framebufferRenderbuffer_e0c873b9f296443d:function(A,e,j,a,t){A.framebufferRenderbuffer(e>>>0,j>>>0,a>>>0,t)},__wbg_framebufferTexture2D_8584b49a205ffe5b:function(A,e,j,a,t,o){A.framebufferTexture2D(e>>>0,j>>>0,a>>>0,t,o)},__wbg_framebufferTexture2D_9abab99d6209666a:function(A,e,j,a,t,o){A.framebufferTexture2D(e>>>0,j>>>0,a>>>0,t,o)},__wbg_framebufferTextureLayer_e236352620170c5a:function(A,e,j,a,t,o){A.framebufferTextureLayer(e>>>0,j>>>0,a,t,o)},__wbg_framebufferTextureMultiviewOVR_9b89dd83134856d3:function(A,e,j,a,t,o,f){A.framebufferTextureMultiviewOVR(e>>>0,j>>>0,a,t,o,f)},__wbg_fromEntries_464704b0ede47aaf:function(){return n(function(A){return Object.fromEntries(A)},arguments)},__wbg_frontFace_188579d7bba462b1:function(A,e){A.frontFace(e>>>0)},__wbg_frontFace_19294c82ae89fa71:function(A,e){A.frontFace(e>>>0)},__wbg_getAttribLocation_bddb3abf7c5c5fc0:function(A,e,j,a){return A.getAttribLocation(e,d(j,a))},__wbg_getBufferSubData_d1d7ad69c40ea085:function(A,e,j,a){A.getBufferSubData(e>>>0,j,a)},__wbg_getContext_123ddade3a0fb2f5:function(){return n(function(A,e,j,a){let t=A.getContext(d(e,j),a);return p(t)?0:B(t)},arguments)},__wbg_getContext_53c8c42beb820370:function(){return n(function(A,e,j,a){let t=A.getContext(d(e,j),a);return p(t)?0:B(t)},arguments)},__wbg_getContext_71c33f14b63da593:function(){return n(function(A,e,j){let a=A.getContext(d(e,j));return p(a)?0:B(a)},arguments)},__wbg_getContext_c5236e0057b35024:function(){return n(function(A,e,j){let a=A.getContext(d(e,j));return p(a)?0:B(a)},arguments)},__wbg_getCurrentTexture_9f3b84d0eaa6cd95:function(){return n(function(A){return A.getCurrentTexture()},arguments)},__wbg_getData_7b73a3e658ca866b:function(){return n(function(A,e,j,a){let t=e.getData(d(j,a)),o=G(t,c.__wbindgen_malloc,c.__wbindgen_realloc),f=g;m().setInt32(A+4,f,!0),m().setInt32(A+0,o,!0)},arguments)},__wbg_getError_417e3c195ccd57de:function(A){return A.getError()},__wbg_getExtension_69f46e4b97514707:function(){return n(function(A,e,j){let a=A.getExtension(d(e,j));return p(a)?0:B(a)},arguments)},__wbg_getExtension_8e8c3be603d4f5ce:function(){return n(function(A,e,j){let a=A.getExtension(d(e,j));return p(a)?0:B(a)},arguments)},__wbg_getGamepads_2493dee1cac4f38b:function(){return n(function(A){return A.getGamepads()},arguments)},__wbg_getImageData_251c6e7a33a280e5:function(){return n(function(A,e,j,a,t){return A.getImageData(e,j,a,t)},arguments)},__wbg_getIndexedParameter_fa6cca29d50de787:function(){return n(function(A,e,j){return A.getIndexedParameter(e>>>0,j>>>0)},arguments)},__wbg_getMappedRange_fb54c6327b2d8d20:function(){return n(function(A,e,j){return A.getMappedRange(e,j)},arguments)},__wbg_getObjectId_98f91727552a61b6:function(A,e){let j=e.getObjectId();var a=p(j)?0:G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},__wbg_getParameter_19325d4aa1b66856:function(){return n(function(A,e){return A.getParameter(e>>>0)},arguments)},__wbg_getParameter_7ddbe9f9606f6a80:function(){return n(function(A,e){return A.getParameter(e>>>0)},arguments)},__wbg_getPreferredCanvasFormat_0ef5034c8902201b:function(A){let e=A.getPreferredCanvasFormat();return(yA.indexOf(e)+1||102)-1},__wbg_getProgramInfoLog_50a07d12dddd0da6:function(A,e,j){let a=e.getProgramInfoLog(j);var t=p(a)?0:G(a,c.__wbindgen_malloc,c.__wbindgen_realloc),o=g;m().setInt32(A+4,o,!0),m().setInt32(A+0,t,!0)},__wbg_getProgramInfoLog_72665662cf78b5a2:function(A,e,j){let a=e.getProgramInfoLog(j);var t=p(a)?0:G(a,c.__wbindgen_malloc,c.__wbindgen_realloc),o=g;m().setInt32(A+4,o,!0),m().setInt32(A+0,t,!0)},__wbg_getProgramParameter_1f5cceb73030e823:function(A,e,j){return A.getProgramParameter(e,j>>>0)},__wbg_getProgramParameter_41e1ea6f52a71ba5:function(A,e,j){return A.getProgramParameter(e,j>>>0)},__wbg_getQueryParameter_fa2ce36cfdedc862:function(A,e,j){return A.getQueryParameter(e,j>>>0)},__wbg_getRandomValues_436a51d0629d84e1:function(){return n(function(A,e){globalThis.crypto.getRandomValues(jA(A,e))},arguments)},__wbg_getReader_9facd4f899beac89:function(){return n(function(A){return A.getReader()},arguments)},__wbg_getRootNode_f79810b049364fd5:function(A){return A.getRootNode()},__wbg_getShaderInfoLog_337a0567e83283d1:function(A,e,j){let a=e.getShaderInfoLog(j);var t=p(a)?0:G(a,c.__wbindgen_malloc,c.__wbindgen_realloc),o=g;m().setInt32(A+4,o,!0),m().setInt32(A+0,t,!0)},__wbg_getShaderInfoLog_663a9b136ab42b32:function(A,e,j){let a=e.getShaderInfoLog(j);var t=p(a)?0:G(a,c.__wbindgen_malloc,c.__wbindgen_realloc),o=g;m().setInt32(A+4,o,!0),m().setInt32(A+0,t,!0)},__wbg_getShaderParameter_95d4ad40668ee798:function(A,e,j){return A.getShaderParameter(e,j>>>0)},__wbg_getShaderParameter_9e9aa18598294f3b:function(A,e,j){return A.getShaderParameter(e,j>>>0)},__wbg_getSupportedExtensions_63e3eaba880055c5:function(A){let e=A.getSupportedExtensions();return p(e)?0:B(e)},__wbg_getSupportedProfiles_7cd826b4eff5e8fc:function(A){let e=A.getSupportedProfiles();return p(e)?0:B(e)},__wbg_getSyncParameter_3eb3ecefa061c5ee:function(A,e,j){return A.getSyncParameter(e,j>>>0)},__wbg_getTime_63fb0332e6c4ec17:function(A){return A.getTime()},__wbg_getTimezoneOffset_4baa793e0d3962a8:function(A){return A.getTimezoneOffset()},__wbg_getUniformBlockIndex_78264d4d94f8252d:function(A,e,j,a){return A.getUniformBlockIndex(e,d(j,a))},__wbg_getUniformLocation_11fd99fee70965dc:function(A,e,j,a){let t=A.getUniformLocation(e,d(j,a));return p(t)?0:B(t)},__wbg_getUniformLocation_c493d2f5f1a6213d:function(A,e,j,a){let t=A.getUniformLocation(e,d(j,a));return p(t)?0:B(t)},__wbg_get_36debceb6d43d7a1:function(A,e){let j=A[e>>>0];return p(j)?0:B(j)},__wbg_get_7473564f5d9fdd2a:function(){return n(function(A,e,j,a){let t=e.get(d(j,a));var o=p(t)?0:G(t,c.__wbindgen_malloc,c.__wbindgen_realloc),f=g;m().setInt32(A+4,f,!0),m().setInt32(A+0,o,!0)},arguments)},__wbg_get_836a517ee3483cda:function(A,e){let j=A[e>>>0];return p(j)?0:B(j)},__wbg_get_971a0c45d172643f:function(){return n(function(A,e){return Reflect.get(A,e)},arguments)},__wbg_get_c0c8f8d7da0c03dd:function(A,e){return A[e>>>0]},__wbg_get_done_ce5b5691b59c07f2:function(A){let e=A.done;return p(e)?16777215:e?1:0},__wbg_get_ed35166764b1a44e:function(){return n(function(A,e,j,a){let t=e[d(j,a)];var o=p(t)?0:G(t,c.__wbindgen_malloc,c.__wbindgen_realloc),f=g;m().setInt32(A+4,f,!0),m().setInt32(A+0,o,!0)},arguments)},__wbg_get_unchecked_e20b893aeafc3fca:function(A,e){return A[e>>>0]},__wbg_get_value_58309ba057b715e1:function(A){return A.value},__wbg_gpu_afdd4387c7afe5f9:function(A){return A.gpu},__wbg_has_b3a6e6d0d28295fa:function(){return n(function(A,e){return Reflect.has(A,e)},arguments)},__wbg_has_eafa12e457ea88fb:function(A,e,j){return A.has(d(e,j))},__wbg_headers_6dedf39f001ae99d:function(A){return A.headers},__wbg_headers_92567b07014384b9:function(A){return A.headers},__wbg_height_b0594a7850e20673:function(A){return A.height},__wbg_height_c25c887c11a170f2:function(A){return A.height},__wbg_height_e56f6fb197710e09:function(A){return A.height},__wbg_height_e6a5d9a72f05fc93:function(A){return A.height},__wbg_host_f512e97ce1222138:function(A){return A.host},__wbg_href_ab966bccc773240e:function(){return n(function(A,e){let j=e.href,a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},arguments)},__wbg_includes_a4b83ade703cb80b:function(A,e,j){return A.includes(e,j)},__wbg_info_971d8b9db3dae69f:function(A){return A.info},__wbg_instanceof_ArrayBuffer_993d02d2d254cad1:function(A){let e;try{e=A instanceof ArrayBuffer}catch{e=!1}return e},__wbg_instanceof_CanvasRenderingContext2d_d23139c3ef7651a3:function(A){let e;try{e=A instanceof CanvasRenderingContext2D}catch{e=!1}return e},__wbg_instanceof_Error_61d8a02a0f3383a1:function(A){let e;try{e=A instanceof Error}catch{e=!1}return e},__wbg_instanceof_GamepadButton_9c609e47a6145e8f:function(A){let e;try{e=A instanceof GamepadButton}catch{e=!1}return e},__wbg_instanceof_Gamepad_31b15eaf4b6abc5a:function(A){let e;try{e=A instanceof Gamepad}catch{e=!1}return e},__wbg_instanceof_HtmlAnchorElement_d90f42ba7073afb6:function(A){let e;try{e=A instanceof HTMLAnchorElement}catch{e=!1}return e},__wbg_instanceof_HtmlButtonElement_806e934e95055a80:function(A){let e;try{e=A instanceof HTMLButtonElement}catch{e=!1}return e},__wbg_instanceof_HtmlCanvasElement_327e7f7530c72bbd:function(A){let e;try{e=A instanceof HTMLCanvasElement}catch{e=!1}return e},__wbg_instanceof_HtmlDocument_a1109ab62f86ff41:function(A){let e;try{e=A instanceof HTMLDocument}catch{e=!1}return e},__wbg_instanceof_HtmlElement_6b02a3740edba922:function(A){let e;try{e=A instanceof HTMLElement}catch{e=!1}return e},__wbg_instanceof_HtmlFormElement_ab33e8c914cfe17d:function(A){let e;try{e=A instanceof HTMLFormElement}catch{e=!1}return e},__wbg_instanceof_HtmlInputElement_6077656bcaf1eb33:function(A){let e;try{e=A instanceof HTMLInputElement}catch{e=!1}return e},__wbg_instanceof_HtmlTextAreaElement_6d5fbbcef108f57a:function(A){let e;try{e=A instanceof HTMLTextAreaElement}catch{e=!1}return e},__wbg_instanceof_Node_ad9597995317f467:function(A){let e;try{e=A instanceof Node}catch{e=!1}return e},__wbg_instanceof_OffscreenCanvasRenderingContext2d_bf5c11dbcfe648e6:function(A){let e;try{e=A instanceof OffscreenCanvasRenderingContext2D}catch{e=!1}return e},__wbg_instanceof_Response_8f49efbd4bfd76d6:function(A){let e;try{e=A instanceof Response}catch{e=!1}return e},__wbg_instanceof_ShadowRoot_55844b1b54688323:function(A){let e;try{e=A instanceof ShadowRoot}catch{e=!1}return e},__wbg_instanceof_WebGl2RenderingContext_e27143c72f888655:function(A){let e;try{e=A instanceof WebGL2RenderingContext}catch{e=!1}return e},__wbg_instanceof_WebGlRenderingContext_7a2f73729caa1761:function(A){let e;try{e=A instanceof WebGLRenderingContext}catch{e=!1}return e},__wbg_instanceof_Window_5625ff9937037a38:function(A){let e;try{e=A instanceof Window}catch{e=!1}return e},__wbg_invalidateFramebuffer_9a711eeb3940aba0:function(){return n(function(A,e,j){A.invalidateFramebuffer(e>>>0,j)},arguments)},__wbg_inverse_979493bf592e8237:function(A){return A.inverse()},__wbg_isActive_030dfade2dac2b18:function(A){return A.isActive},__wbg_isArray_6339f732981044bf:function(A){return Array.isArray(A)},__wbg_isFallbackAdapter_4c8cc3b18677460a:function(A){return A.isFallbackAdapter},__wbg_isVirtualKeyboardFocused_50ace665aa22e045:function(A){return A.isVirtualKeyboardFocused()},__wbg_is_86be747e88e872fb:function(A,e){return Object.is(A,e)},__wbg_key_d1b2fd5ee42567c0:function(A,e){let j=e.key,a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},__wbg_label_7add8cb37a6ef98f:function(A,e){let j=e.label,a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},__wbg_language_8bf4dda293978baf:function(A,e){let j=e.language;var a=p(j)?0:G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},__wbg_lastModified_a866385c6ec928bb:function(A){return A.lastModified},__wbg_length_2dd58ff350b5afcd:function(A){return A.length},__wbg_length_36bd29c6848c2144:function(A){return A.length},__wbg_length_ecfa2c63d3d0d82c:function(A){return A.length},__wbg_length_fe334960471188ea:function(A){return A.length},__wbg_limits_06bcb36c8409843b:function(A){return A.limits},__wbg_limits_601ad2e086ef8141:function(A){return A.limits},__wbg_lineTo_55f2d19e97fe770d:function(A,e,j){A.lineTo(e,j)},__wbg_linkProgram_124252d16ea0ef40:function(A,e){A.linkProgram(e)},__wbg_linkProgram_dd3cfc19950a354c:function(A,e){A.linkProgram(e)},__wbg_localStorage_19bddab1e4cb2413:function(){return n(function(A){let e=A.localStorage;return p(e)?0:B(e)},arguments)},__wbg_location_00f2951912aef6cc:function(A){return A.location},__wbg_location_5d269cf0aa99107a:function(A){return A.location},__wbg_log_1f8cbb01c83d06c2:function(A,e,j,a,t,o,f,s){let i,b;try{i=A,b=e,console.log(d(A,e),d(j,a),d(t,o),d(f,s))}finally{c.__wbindgen_free(i,b,1)}},__wbg_log_a54ca6b45e09078a:function(A,e){let j,a;try{j=A,a=e,console.log(d(A,e))}finally{c.__wbindgen_free(j,a,1)}},__wbg_mapAsync_b0597127f5037286:function(A,e,j,a){return A.mapAsync(e>>>0,j,a)},__wbg_mark_6b7f03786f5e4d61:function(A,e){performance.mark(d(A,e))},__wbg_matchMedia_0e2963d34f3ddd40:function(){return n(function(A,e,j){let a=A.matchMedia(d(e,j));return p(a)?0:B(a)},arguments)},__wbg_matches_72427e51457a4411:function(A){return A.matches},__wbg_maxBindGroupsPlusVertexBuffers_52369f089736ef9d:function(A){return A.maxBindGroupsPlusVertexBuffers},__wbg_maxBindGroups_4e424afe6ce86ca2:function(A){return A.maxBindGroups},__wbg_maxBindingsPerBindGroup_7d035da36821c44f:function(A){return A.maxBindingsPerBindGroup},__wbg_maxBufferSize_423f4a084e32a195:function(A){return A.maxBufferSize},__wbg_maxColorAttachmentBytesPerSample_c4cd9126f6d287c6:function(A){return A.maxColorAttachmentBytesPerSample},__wbg_maxColorAttachments_d924670762b9e250:function(A){return A.maxColorAttachments},__wbg_maxComputeInvocationsPerWorkgroup_707a3868f7cebb59:function(A){return A.maxComputeInvocationsPerWorkgroup},__wbg_maxComputeWorkgroupSizeX_0a4d99463cbd6e5e:function(A){return A.maxComputeWorkgroupSizeX},__wbg_maxComputeWorkgroupSizeY_85123ea0587f7558:function(A){return A.maxComputeWorkgroupSizeY},__wbg_maxComputeWorkgroupSizeZ_a3186b4c5267d44f:function(A){return A.maxComputeWorkgroupSizeZ},__wbg_maxComputeWorkgroupStorageSize_57b297355cfb6204:function(A){return A.maxComputeWorkgroupStorageSize},__wbg_maxComputeWorkgroupsPerDimension_4158f95e673d54c4:function(A){return A.maxComputeWorkgroupsPerDimension},__wbg_maxDynamicStorageBuffersPerPipelineLayout_226b0b70910aa16c:function(A){return A.maxDynamicStorageBuffersPerPipelineLayout},__wbg_maxDynamicUniformBuffersPerPipelineLayout_0e835fda711fc7e6:function(A){return A.maxDynamicUniformBuffersPerPipelineLayout},__wbg_maxInterStageShaderVariables_8c4a1d727e2aa35a:function(A){return A.maxInterStageShaderVariables},__wbg_maxSampledTexturesPerShaderStage_6675f5e91d9a728a:function(A){return A.maxSampledTexturesPerShaderStage},__wbg_maxSamplersPerShaderStage_1910fa38a6ed1e1f:function(A){return A.maxSamplersPerShaderStage},__wbg_maxStorageBufferBindingSize_2e244bded070b18d:function(A){return A.maxStorageBufferBindingSize},__wbg_maxStorageBuffersPerShaderStage_a285f3ebca51ca0d:function(A){return A.maxStorageBuffersPerShaderStage},__wbg_maxStorageTexturesPerShaderStage_7aa946f0fc322a2b:function(A){return A.maxStorageTexturesPerShaderStage},__wbg_maxTextureArrayLayers_0e699147ad00502d:function(A){return A.maxTextureArrayLayers},__wbg_maxTextureDimension1D_aabf6add54decfe2:function(A){return A.maxTextureDimension1D},__wbg_maxTextureDimension2D_dd598b27e9c0c1c4:function(A){return A.maxTextureDimension2D},__wbg_maxTextureDimension3D_f944266c65dfd1a9:function(A){return A.maxTextureDimension3D},__wbg_maxUniformBufferBindingSize_59fa6be7cfbeeb53:function(A){return A.maxUniformBufferBindingSize},__wbg_maxUniformBuffersPerShaderStage_bee5f00a4d706c7f:function(A){return A.maxUniformBuffersPerShaderStage},__wbg_maxVertexAttributes_5cf6392c4e9033fe:function(A){return A.maxVertexAttributes},__wbg_maxVertexBufferArrayStride_548baa887375d865:function(A){return A.maxVertexBufferArrayStride},__wbg_maxVertexBuffers_75d881156591f5da:function(A){return A.maxVertexBuffers},__wbg_measureText_138b46c6b2239fe9:function(){return n(function(A,e,j){return A.measureText(d(e,j))},arguments)},__wbg_measure_0e21b33a1c6e3a29:function(){return n(function(A,e,j,a){let t,o,f,s;try{t=A,o=e,f=j,s=a,performance.measure(d(A,e),d(j,a))}finally{c.__wbindgen_free(t,o,1),c.__wbindgen_free(f,s,1)}},arguments)},__wbg_message_88eda073e68b1d26:function(A,e){let j=e.message,a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},__wbg_message_c141d5e68716b595:function(A){return A.message},__wbg_metaKey_917f037461143e51:function(A){return A.metaKey},__wbg_minStorageBufferOffsetAlignment_5ba9b77792bdadb3:function(A){return A.minStorageBufferOffsetAlignment},__wbg_minUniformBufferOffsetAlignment_ab7d52a5293b22bd:function(A){return A.minUniformBufferOffsetAlignment},__wbg_moveTo_b163e74b8926c626:function(A,e,j){A.moveTo(e,j)},__wbg_name_41b795553ec88cd8:function(A,e){let j=e.name,a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},__wbg_name_7adfb7f7f1539878:function(A){return A.name},__wbg_navigator_6cfdd5fa246d910f:function(A){return A.navigator},__wbg_navigator_e5c345298a9609cd:function(A){return A.navigator},__wbg_new_033da64d293f5a26:function(){return n(function(A){return new VideoDecoder(A)},arguments)},__wbg_new_0_f117d868b403dc07:function(){return new Date},__wbg_new_116be93542d39019:function(){return new Array},__wbg_new_1f27644530c822b2:function(){return n(function(){return new FileReader},arguments)},__wbg_new_20a7c62e9b30cbf7:function(){return n(function(A,e){return new WebSocket(d(A,e))},arguments)},__wbg_new_227d7c05414eb861:function(){return new Error},__wbg_new_358857d90afd5a2d:function(A,e){return new Error(d(A,e))},__wbg_new_3ce973d9e04baf94:function(){return n(function(A){return new EncodedVideoChunk(A)},arguments)},__wbg_new_418fb92a013d5930:function(A,e){try{var j={a:A,b:e},a=(o,f)=>{let s=j.a;j.a=0;try{return _r(s,j.b,o,f)}finally{j.a=s}};return new Promise(a)}finally{j.a=0}},__wbg_new_652118cdee90118f:function(){return n(function(A,e){return new OffscreenCanvas(A>>>0,e>>>0)},arguments)},__wbg_new_6fa4b00b7fe13e4b:function(){return n(function(){return new Path2D},arguments)},__wbg_new_77cc4f4f472aeb81:function(A){return new Uint8Array(A)},__wbg_new_ebe3e0f6837f0879:function(){return new Object},__wbg_new_ec007c098ac92ebf:function(){return n(function(){return new DOMMatrix},arguments)},__wbg_new_f9d6489212f3b2b3:function(A){return new Date(A)},__wbg_new_from_slice_3eea173078478cfe:function(A,e){return new Uint8Array(jA(A,e))},__wbg_new_typed_ad9b105a7be50737:function(){return new Object},__wbg_new_typed_cceaf62d8d95e9f2:function(A,e){try{var j={a:A,b:e},a=(o,f)=>{let s=j.a;j.a=0;try{return _r(s,j.b,o,f)}finally{j.a=s}};return new Promise(a)}finally{j.a=0}},__wbg_new_with_array64_77901f8040d2e3f6:function(){return n(function(A,e){return new DOMMatrix(Jt(A,e))},arguments)},__wbg_new_with_buffer_source_sequence_and_options_a0124a2dac7638be:function(){return n(function(A,e){return new Blob(A,e)},arguments)},__wbg_new_with_byte_offset_and_length_ff6e927f8d72f0c3:function(A,e,j){return new Uint8Array(A,e>>>0,j>>>0)},__wbg_new_with_context_options_06b7c8f962e9da06:function(){return n(function(A){return new rt(A)},arguments)},__wbg_new_with_event_init_dict_77122dca3c723f0c:function(){return n(function(A,e,j){return new CloseEvent(d(A,e),j)},arguments)},__wbg_new_with_str_and_init_5a37d576dec75a86:function(){return n(function(A,e,j){return new Request(d(A,e),j)},arguments)},__wbg_new_with_sw_cced22be0cbff0d3:function(){return n(function(A,e){return new ImageData(A>>>0,e>>>0)},arguments)},__wbg_new_with_u8_array_sequence_6f96909d5e4901f9:function(){return n(function(A){return new Blob(A)},arguments)},__wbg_new_with_u8_array_sequence_and_options_a7cc7b64ed3eb153:function(){return n(function(A,e){return new Blob(A,e)},arguments)},__wbg_new_with_u8_clamped_array_2fcfd0f372cd4225:function(){return n(function(A,e,j){return new ImageData(eo(A,e),j>>>0)},arguments)},__wbg_next_42cf16ee0dafc9e2:function(){return n(function(A){return A.next()},arguments)},__wbg_now_e7c6795a7f81e10f:function(A){return A.now()},__wbg_of_0c6464fa8d2aa86d:function(A){return Array.of(A)},__wbg_of_598c0ff0cd48a890:function(A,e){return Array.of(A,e)},__wbg_offsetX_878997328bd9eaa4:function(A){return A.offsetX},__wbg_offsetY_228d7dd70336f05d:function(A){return A.offsetY},__wbg_ok_917dc17857b16c56:function(A){return A.ok},__wbg_onCallbackAvailable_cf30b2252fbf24c4:function(A,e,j){A.onCallbackAvailable(d(e,j))},__wbg_onSubmittedWorkDone_1190213cee1ecf7e:function(A){return A.onSubmittedWorkDone()},__wbg_openVirtualKeyboard_b322dee3c7264057:function(A){A.openVirtualKeyboard()},__wbg_open_67cee4f3ea60a981:function(){return n(function(A,e,j,a,t){let o=A.open(d(e,j),d(a,t));return p(o)?0:B(o)},arguments)},__wbg_ownKeys_49880e0197268893:function(){return n(function(A){return Reflect.ownKeys(A)},arguments)},__wbg_panic_a77e702cf13dd35e:function(A,e){A.panic(e)},__wbg_parentElement_ef76606593484767:function(A){let e=A.parentElement;return p(e)?0:B(e)},__wbg_performance_3fcf6e32a7e1ed0a:function(A){return A.performance},__wbg_persisted_03e56c5f9080ac54:function(A){return A.persisted},__wbg_pixelStorei_11bdfb5bc6a39d28:function(A,e,j){A.pixelStorei(e>>>0,j)},__wbg_pixelStorei_86481a168d6e225e:function(A,e,j){A.pixelStorei(e>>>0,j)},__wbg_platform_723fb7833ed963df:function(){return n(function(A,e){let j=e.platform,a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},arguments)},__wbg_pointerId_c1e1cd6b32d6d017:function(A){return A.pointerId},__wbg_polygonOffset_2b8b141e8cc17c10:function(A,e,j){A.polygonOffset(e,j)},__wbg_polygonOffset_94b427c5130ab6c2:function(A,e,j){A.polygonOffset(e,j)},__wbg_popDebugGroup_87cc10f02f9baa29:function(A){A.popDebugGroup()},__wbg_popDebugGroup_fc6cf5f2069b07ea:function(A){A.popDebugGroup()},__wbg_pressed_0ef66768049be92d:function(A){return A.pressed},__wbg_preventDefault_19878c58b8010668:function(A){A.preventDefault()},__wbg_protocol_537788ea57915c6c:function(){return n(function(A,e){let j=e.protocol,a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},arguments)},__wbg_prototypesetcall_de8e0d9553586985:function(A,e,j){Uint8Array.prototype.set.call(jA(A,e),j)},__wbg_pushDebugGroup_20949a2b29d3bd19:function(A,e,j){A.pushDebugGroup(d(e,j))},__wbg_pushDebugGroup_356e96c0f79bab32:function(A,e,j){A.pushDebugGroup(d(e,j))},__wbg_push_adb0107829f02d75:function(A,e){return A.push(e)},__wbg_putImageData_78f2f075ef560bf6:function(){return n(function(A,e,j,a){A.putImageData(e,j,a)},arguments)},__wbg_quadraticCurveTo_f60aa9069b458c65:function(A,e,j,a,t){A.quadraticCurveTo(e,j,a,t)},__wbg_queryCounterEXT_b0cddcdfb28830df:function(A,e,j){A.queryCounterEXT(e,j>>>0)},__wbg_querySelectorAll_9b6a612499ecb916:function(){return n(function(A,e,j){return A.querySelectorAll(d(e,j))},arguments)},__wbg_querySelector_2c472eddb417c6b3:function(){return n(function(A,e,j){let a=A.querySelector(d(e,j));return p(a)?0:B(a)},arguments)},__wbg_querySelector_839d6534e69c0f64:function(){return n(function(A,e,j){let a=A.querySelector(d(e,j));return p(a)?0:B(a)},arguments)},__wbg_queueMicrotask_ac694eae12e92dfb:function(A){queueMicrotask(A)},__wbg_queueMicrotask_be5fe34a8f4cad4d:function(A){return A.queueMicrotask},__wbg_queue_7b62c28143d44293:function(A){return A.queue},__wbg_readAsArrayBuffer_1e0bf6cd0613d7fd:function(){return n(function(A,e){A.readAsArrayBuffer(e)},arguments)},__wbg_readBuffer_2de0b72ac08915c8:function(A,e){A.readBuffer(e>>>0)},__wbg_readPixels_0033d2834b498dda:function(){return n(function(A,e,j,a,t,o,f,s){A.readPixels(e,j,a,t,o>>>0,f>>>0,s)},arguments)},__wbg_readPixels_0e3230bf7a891882:function(){return n(function(A,e,j,a,t,o,f,s){A.readPixels(e,j,a,t,o>>>0,f>>>0,s)},arguments)},__wbg_readPixels_8f8bde9ee420ba35:function(){return n(function(A,e,j,a,t,o,f,s){A.readPixels(e,j,a,t,o>>>0,f>>>0,s)},arguments)},__wbg_readText_57255f9c7482c995:function(A){return A.readText()},__wbg_read_ae34ffedeb11f034:function(A){return A.read()},__wbg_readyState_fe79161592fd15ce:function(A){return A.readyState},__wbg_reason_1460f6c833ca7671:function(A,e){let j=e.reason,a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},__wbg_rect_db1056f1138dff21:function(A,e,j,a,t){A.rect(e,j,a,t)},__wbg_redirected_38edd6189354296c:function(A){return A.redirected},__wbg_relatedTarget_0707f779c6356847:function(A){let e=A.relatedTarget;return p(e)?0:B(e)},__wbg_releaseLock_f38d2d1c08212a8a:function(A){A.releaseLock()},__wbg_releasePointerCapture_625918adece6fc4b:function(){return n(function(A,e){A.releasePointerCapture(e)},arguments)},__wbg_reloadWithCanvasRenderer_fe5adde59fb8dd8d:function(A){A.reloadWithCanvasRenderer()},__wbg_removeChild_58f3071cb194ee29:function(){return n(function(A,e){return A.removeChild(e)},arguments)},__wbg_removeEventListener_aa653c6b402cc27e:function(){return n(function(A,e,j,a){A.removeEventListener(d(e,j),a)},arguments)},__wbg_removeEventListener_f0778286eef3aecc:function(){return n(function(A,e,j,a,t){A.removeEventListener(d(e,j),a,t!==0)},arguments)},__wbg_remove_07453fe173d20eee:function(A){A.remove()},__wbg_renderbufferStorageMultisample_a9f65ef0cc53fb37:function(A,e,j,a,t,o){A.renderbufferStorageMultisample(e>>>0,j,a>>>0,t,o)},__wbg_renderbufferStorage_33c57e600b175bd6:function(A,e,j,a,t){A.renderbufferStorage(e>>>0,j>>>0,a,t)},__wbg_renderbufferStorage_3fb6d5a0f3e07d46:function(A,e,j,a,t){A.renderbufferStorage(e>>>0,j>>>0,a,t)},__wbg_replace_b9d88072d7c356a3:function(A,e,j,a){return A.replace(e,d(j,a))},__wbg_requestAdapter_a539af006419f2e9:function(A,e){return A.requestAdapter(e)},__wbg_requestAnimationFrame_bcb3ce6247e27dd4:function(){return n(function(A,e){return A.requestAnimationFrame(e)},arguments)},__wbg_requestDevice_5cb8a582e55d08cb:function(A,e){return A.requestDevice(e)},__wbg_resetTransform_98a89c9c0f94fe2b:function(){return n(function(A){A.resetTransform()},arguments)},__wbg_resolveQuerySet_770f23fabac49845:function(A,e,j,a,t,o){A.resolveQuerySet(e,j>>>0,a>>>0,t,o>>>0)},__wbg_resolve_020f95d838c6ef25:function(A){return Promise.resolve(A)},__wbg_respond_f88cbcebace42068:function(){return n(function(A,e){A.respond(e>>>0)},arguments)},__wbg_restore_43a0248041b088b5:function(A){A.restore()},__wbg_result_89c2bfc79be07ad2:function(){return n(function(A){return A.result},arguments)},__wbg_resume_d3c27715f0790def:function(){return n(function(A){return A.resume()},arguments)},__wbg_revokeObjectURL_709bc205d98c34ba:function(){return n(function(A,e){URL.revokeObjectURL(d(A,e))},arguments)},__wbg_rufflehandle_new:function(A){return NA.__wrap(A)},__wbg_sampleRate_7751976089d109e1:function(A){return A.sampleRate},__wbg_samplerParameterf_d7f38ba3194c43ba:function(A,e,j,a){A.samplerParameterf(e,j>>>0,a)},__wbg_samplerParameteri_3d8994d9967c6803:function(A,e,j,a){A.samplerParameteri(e,j>>>0,a)},__wbg_save_0c65dc2190a45c2a:function(A){A.save()},__wbg_scissor_2f02706fbca6e98a:function(A,e,j,a,t){A.scissor(e,j,a,t)},__wbg_scissor_cdfb84de20f004b6:function(A,e,j,a,t){A.scissor(e,j,a,t)},__wbg_search_e4668fa7ed0474da:function(A,e){return A.search(e)},__wbg_select_d82465f2823758c6:function(A){A.select()},__wbg_send_5f7b516053d59f8d:function(){return n(function(A,e,j){A.send(d(e,j))},arguments)},__wbg_send_e76231c2136733db:function(){return n(function(A,e){A.send(e)},arguments)},__wbg_setAttributeNS_7c12a81b4d738959:function(){return n(function(A,e,j,a,t,o,f){A.setAttributeNS(e===0?void 0:d(e,j),d(a,t),d(o,f))},arguments)},__wbg_setAttribute_507f8367905a9c03:function(){return n(function(A,e,j,a,t){A.setAttribute(d(e,j),d(a,t))},arguments)},__wbg_setBindGroup_11bdbb60cc8b54b9:function(){return n(function(A,e,j,a,t,o,f){A.setBindGroup(e>>>0,j,zA(a,t),o,f>>>0)},arguments)},__wbg_setBindGroup_418c3e0eb6943ce0:function(A,e,j){A.setBindGroup(e>>>0,j)},__wbg_setFullscreen_e90cb99172887c65:function(){return n(function(A,e){A.setFullscreen(e!==0)},arguments)},__wbg_setIndexBuffer_01327df91742b73e:function(A,e,j,a){A.setIndexBuffer(e,Tj[j],a)},__wbg_setIndexBuffer_241097e303986c14:function(A,e,j,a,t){A.setIndexBuffer(e,Tj[j],a,t)},__wbg_setMetadata_1a042489eb3699dd:function(A,e){A.setMetadata(e)},__wbg_setPipeline_b6f981027e02cd16:function(A,e){A.setPipeline(e)},__wbg_setPointerCapture_761aa655f9aebc1a:function(){return n(function(A,e){A.setPointerCapture(e)},arguments)},__wbg_setProperty_684ce273e28a7037:function(){return n(function(A,e,j,a,t){A.setProperty(d(e,j),d(a,t))},arguments)},__wbg_setScissorRect_889235eeb784732b:function(A,e,j,a,t){A.setScissorRect(e>>>0,j>>>0,a>>>0,t>>>0)},__wbg_setStencilReference_48705a9a2cceae02:function(A,e){A.setStencilReference(e>>>0)},__wbg_setTimeout_9d0a5393fa9dc61c:function(){return n(function(A,e){return A.setTimeout(e)},arguments)},__wbg_setTransform_790a24b61d963dff:function(A,e){A.setTransform(e)},__wbg_setTransform_af9c1fdc090e1259:function(){return n(function(A,e,j,a,t,o,f){A.setTransform(e,j,a,t,o,f)},arguments)},__wbg_setVertexBuffer_6db3b60e99280744:function(A,e,j,a){A.setVertexBuffer(e>>>0,j,a)},__wbg_setVertexBuffer_cbf4ca1627c02f4c:function(A,e,j,a,t){A.setVertexBuffer(e>>>0,j,a,t)},__wbg_set_6be42768c690e380:function(A,e,j){A[e]=j},__wbg_set_8155bb79a948541b:function(){return n(function(A,e,j){return Reflect.set(A,e,j)},arguments)},__wbg_set_862c439a342a8818:function(A,e,j){A.set(e,j>>>0)},__wbg_set_a80955eb93b145c6:function(A,e,j){A[e>>>0]=j},__wbg_set_a_82818effc94f6256:function(A,e){A.a=e},__wbg_set_a_e37f5dc6b60caf30:function(A,e){A.a=e},__wbg_set_accept_8be58cd585a9f2ae:function(A,e,j){A.accept=d(e,j)},__wbg_set_access_a099cfbbeec9b96f:function(A,e){A.access=Tt[e]},__wbg_set_action_80fce850115c6e52:function(A,e,j){A.action=d(e,j)},__wbg_set_address_mode_u_a68737cf5d288f95:function(A,e){A.addressModeU=Rj[e]},__wbg_set_address_mode_v_b1c3c45933f540d1:function(A,e){A.addressModeV=Rj[e]},__wbg_set_address_mode_w_889c31cf7022c764:function(A,e){A.addressModeW=Rj[e]},__wbg_set_alpha_106f21a936a85eba:function(A,e){A.alpha=e},__wbg_set_alpha_mode_5544568dbac50280:function(A,e){A.alphaMode=wt[e]},__wbg_set_alpha_to_coverage_enabled_3372ce329447b8f1:function(A,e){A.alphaToCoverageEnabled=e!==0},__wbg_set_array_layer_count_22afa0a979e4ad55:function(A,e){A.arrayLayerCount=e>>>0},__wbg_set_array_stride_f64_6816040e5e7598c3:function(A,e){A.arrayStride=e},__wbg_set_aspect_a48d046965270281:function(A,e){A.aspect=gr[e]},__wbg_set_aspect_b1a9909bf315433f:function(A,e){A.aspect=gr[e]},__wbg_set_attributes_9e38cb1dde387a5b:function(A,e,j){A.attributes=eA(e,j)},__wbg_set_b9b5b5cb7b495037:function(A,e,j){A.set(jA(e,j))},__wbg_set_b_a3297ee7e7cac3a8:function(A,e){A.b=e},__wbg_set_base_array_layer_2435ba92c80346ae:function(A,e){A.baseArrayLayer=e>>>0},__wbg_set_base_mip_level_8b6093e875e7c65d:function(A,e){A.baseMipLevel=e>>>0},__wbg_set_bc2d20c77f0cca90:function(){return n(function(A,e,j,a,t){A[d(e,j)]=d(a,t)},arguments)},__wbg_set_beginning_of_pass_write_index_e552c5e8b8bbf52f:function(A,e){A.beginningOfPassWriteIndex=e>>>0},__wbg_set_binaryType_b701908a03166a9f:function(A,e){A.binaryType=Gt[e]},__wbg_set_bind_group_layouts_458c44ba55100b82:function(A,e,j){A.bindGroupLayouts=eA(e,j)},__wbg_set_binding_81b3fac7f7acaf8d:function(A,e){A.binding=e>>>0},__wbg_set_binding_b6cee57f35ac5190:function(A,e){A.binding=e>>>0},__wbg_set_blend_1a801617945f7945:function(A,e){A.blend=e},__wbg_set_body_f301b68bff45f419:function(A,e){A.body=e},__wbg_set_buffer_1548ae88a9188037:function(A,e){A.buffer=e},__wbg_set_buffer_8d0ac64ad20dfc84:function(A,e){A.buffer=e},__wbg_set_buffer_910a40a90f97cfca:function(A,e){A.buffer=e},__wbg_set_buffer_ef94b43a403b11b5:function(A,e){A.buffer=e},__wbg_set_buffers_5d0e0c50791f710e:function(A,e,j){A.buffers=eA(e,j)},__wbg_set_bytes_per_row_1e824a5502b54b3d:function(A,e){A.bytesPerRow=e>>>0},__wbg_set_bytes_per_row_c28583f0063160f1:function(A,e){A.bytesPerRow=e>>>0},__wbg_set_capture_0fda5cbdb4353cff:function(A,e){A.capture=e!==0},__wbg_set_className_bc6ed54ffff19a12:function(A,e,j){A.className=d(e,j)},__wbg_set_clear_value_gpu_color_dict_a9f763e8372ac1de:function(A,e){A.clearValue=e},__wbg_set_code_5d5b0b9e2fd0dca7:function(A,e,j){A.code=d(e,j)},__wbg_set_code_e5db843dcd11dd81:function(A,e){A.code=e},__wbg_set_codec_de80f2ee1daf3868:function(A,e,j){A.codec=d(e,j)},__wbg_set_color_8ecace4011f47d2e:function(A,e){A.color=e},__wbg_set_color_attachments_622fe2d5997fda7a:function(A,e,j){A.colorAttachments=eA(e,j)},__wbg_set_compare_080c9e492ff36990:function(A,e){A.compare=Hj[e]},__wbg_set_compare_817cf3695599eaa6:function(A,e){A.compare=Hj[e]},__wbg_set_count_8ff0c9474e39a849:function(A,e){A.count=e>>>0},__wbg_set_count_d9dc88156fd05bc7:function(A,e){A.count=e>>>0},__wbg_set_credentials_d7f3b810cbf191e1:function(A,e){A.credentials=Vt[e]},__wbg_set_cull_mode_85d2b4ab0ce3a564:function(A,e){A.cullMode=It[e]},__wbg_set_d_9f19046da6420c83:function(A,e){A.d=e},__wbg_set_data_96c7b174a9034667:function(A,e){A.data=e},__wbg_set_depth_bias_95abf479cae3f3cd:function(A,e){A.depthBias=e},__wbg_set_depth_bias_clamp_ba3d0b8348151350:function(A,e){A.depthBiasClamp=e},__wbg_set_depth_bias_slope_scale_6b2584d93f5b9cd2:function(A,e){A.depthBiasSlopeScale=e},__wbg_set_depth_clear_value_e30a4c754c6b3b26:function(A,e){A.depthClearValue=e},__wbg_set_depth_compare_a90de4e3714397ab:function(A,e){A.depthCompare=Hj[e]},__wbg_set_depth_fail_op_b5c64541d1b6b482:function(A,e){A.depthFailOp=Kj[e]},__wbg_set_depth_load_op_932888016d762d3e:function(A,e){A.depthLoadOp=Sj[e]},__wbg_set_depth_or_array_layers_e2f074a0284e4806:function(A,e){A.depthOrArrayLayers=e>>>0},__wbg_set_depth_read_only_be790175a1c2db9a:function(A,e){A.depthReadOnly=e!==0},__wbg_set_depth_stencil_attachment_54a8922f5fbe08bf:function(A,e){A.depthStencilAttachment=e},__wbg_set_depth_stencil_b7cffc59ad4da529:function(A,e){A.depthStencil=e},__wbg_set_depth_store_op_9054814f164ab55d:function(A,e){A.depthStoreOp=Mj[e]},__wbg_set_depth_write_enabled_31a821ee1fb3b0b3:function(A,e){A.depthWriteEnabled=e!==0},__wbg_set_description_2c77102c025cc80b:function(A,e){A.description=e},__wbg_set_device_210484a77b675c9c:function(A,e){A.device=e},__wbg_set_dimension_3da9d03131a9f446:function(A,e){A.dimension=St[e]},__wbg_set_dimension_56332450afa3e0c0:function(A,e){A.dimension=Lj[e]},__wbg_set_download_602973d1dd39bdc8:function(A,e,j){A.download=d(e,j)},__wbg_set_dst_factor_865ba9aaf187890c:function(A,e){A.dstFactor=Er[e]},__wbg_set_e92392c4b44c5de1:function(){return n(function(A,e,j,a,t){A.set(d(e,j),d(a,t))},arguments)},__wbg_set_end_of_pass_write_index_8f164f9e60d4ad16:function(A,e){A.endOfPassWriteIndex=e>>>0},__wbg_set_entries_6f866302103b81e9:function(A,e,j){A.entries=eA(e,j)},__wbg_set_entries_f26b77ab9548e906:function(A,e,j){A.entries=eA(e,j)},__wbg_set_entry_point_71cef95c137b5774:function(A,e,j){A.entryPoint=d(e,j)},__wbg_set_entry_point_b70f98f5025a114d:function(A,e,j){A.entryPoint=d(e,j)},__wbg_set_error_413401f8612abd97:function(A,e){A.error=e},__wbg_set_external_texture_7f966c604c4f8098:function(A,e){A.externalTexture=e},__wbg_set_fail_op_d59d0187e4111dfe:function(A,e){A.failOp=Kj[e]},__wbg_set_fillStyle_0613e54d2aa04a75:function(A,e,j){A.fillStyle=d(e,j)},__wbg_set_fillStyle_392607276a67e12a:function(A,e){A.fillStyle=e},__wbg_set_fillStyle_52e75a25be60a3ff:function(A,e,j){A.fillStyle=d(e,j)},__wbg_set_fillStyle_9215db6210dfdee2:function(A,e){A.fillStyle=e},__wbg_set_filter_c05b047d621641d5:function(A,e,j){A.filter=d(e,j)},__wbg_set_font_cb31872ffc00c18f:function(A,e,j){A.font=d(e,j)},__wbg_set_format_23f7f32549751d43:function(A,e){A.format=yA[e]},__wbg_set_format_283dca56552f07a3:function(A,e){A.format=yA[e]},__wbg_set_format_5080a858117ad2c1:function(A,e){A.format=Mt[e]},__wbg_set_format_66735b94bd868ba2:function(A,e){A.format=yA[e]},__wbg_set_format_7f2bdbfb101b1ae1:function(A,e){A.format=yA[e]},__wbg_set_format_92732ea75d3b79f5:function(A,e){A.format=yA[e]},__wbg_set_format_f009e603f7d4c28e:function(A,e){A.format=yA[e]},__wbg_set_fragment_d2b0ec97d7cf8d47:function(A,e){A.fragment=e},__wbg_set_front_face_d3f8a2e07e7b25dd:function(A,e){A.frontFace=Dt[e]},__wbg_set_g_b527ee8a9bed553d:function(A,e){A.g=e},__wbg_set_globalAlpha_7990fab00eb6c8f2:function(A,e){A.globalAlpha=e},__wbg_set_globalCompositeOperation_1336df410cebd928:function(){return n(function(A,e,j){A.globalCompositeOperation=d(e,j)},arguments)},__wbg_set_has_dynamic_offset_0c72ffa900c5a269:function(A,e){A.hasDynamicOffset=e!==0},__wbg_set_height_ca39bd9597314f83:function(A,e){A.height=e>>>0},__wbg_set_height_d72f2b76484a44de:function(A,e){A.height=e>>>0},__wbg_set_height_f6619158e5735877:function(A,e){A.height=e>>>0},__wbg_set_href_4fab988857d37334:function(A,e,j){A.href=d(e,j)},__wbg_set_id_ce80620265c5de8d:function(A,e,j){A.id=d(e,j)},__wbg_set_imageSmoothingEnabled_cd98f777ac3af24f:function(A,e){A.imageSmoothingEnabled=e!==0},__wbg_set_innerHTML_7d84b81d6f2a9fdf:function(A,e,j){A.innerHTML=d(e,j)},__wbg_set_innerText_147c496ec424c079:function(A,e,j){A.innerText=d(e,j)},__wbg_set_label_17202740051e9722:function(A,e,j){A.label=d(e,j)},__wbg_set_label_2fefb39c0e0dbbe8:function(A,e,j){A.label=d(e,j)},__wbg_set_label_3cb2322e6f6db14c:function(A,e,j){A.label=d(e,j)},__wbg_set_label_3f2ccaafef5ff7c9:function(A,e,j){A.label=d(e,j)},__wbg_set_label_612add98a4398f92:function(A,e,j){A.label=d(e,j)},__wbg_set_label_6f69e25822616a1b:function(A,e,j){A.label=d(e,j)},__wbg_set_label_70a09ee68d6b1b26:function(A,e,j){A.label=d(e,j)},__wbg_set_label_92cd3811e96b487c:function(A,e,j){A.label=d(e,j)},__wbg_set_label_9c2a186152427ee0:function(A,e,j){A.label=d(e,j)},__wbg_set_label_c3eaf136aa464cba:function(A,e,j){A.label=d(e,j)},__wbg_set_label_c7987704d29f284b:function(A,e,j){A.label=d(e,j)},__wbg_set_label_cfe64bca8945ee30:function(A,e,j){A.label=d(e,j)},__wbg_set_label_e02179cf97e95763:function(A,e,j){A.label=d(e,j)},__wbg_set_label_ee172cd5f6a96961:function(A,e,j){A.label=d(e,j)},__wbg_set_layout_454e3a091b390cd4:function(A,e){A.layout=e},__wbg_set_layout_75dc1ca3f2421cff:function(A,e){A.layout=e},__wbg_set_layout_gpu_auto_layout_mode_06a2b95af1043098:function(A,e){A.layout=Ft[e]},__wbg_set_lineCap_ec484c1489fa48bc:function(A,e,j){A.lineCap=d(e,j)},__wbg_set_lineJoin_645744ec04386dd0:function(A,e,j){A.lineJoin=d(e,j)},__wbg_set_lineWidth_5f9aefcc32e60287:function(A,e){A.lineWidth=e},__wbg_set_load_op_c56b1269acc2d51f:function(A,e){A.loadOp=Sj[e]},__wbg_set_lod_max_clamp_db24179f67f3aa31:function(A,e){A.lodMaxClamp=e},__wbg_set_lod_min_clamp_2bbce566e9fefa04:function(A,e){A.lodMinClamp=e},__wbg_set_mag_filter_db8e6b42d4f8846d:function(A,e){A.magFilter=mr[e]},__wbg_set_mapped_at_creation_3f320fef6761b02c:function(A,e){A.mappedAtCreation=e!==0},__wbg_set_mask_c1079e551ec360dc:function(A,e){A.mask=e>>>0},__wbg_set_max_anisotropy_84749fdcec362dc4:function(A,e){A.maxAnisotropy=e},__wbg_set_method_cf2b992b9a610bc3:function(A,e,j){A.method=d(e,j)},__wbg_set_method_fd3992cb9c0b7760:function(A,e,j){A.method=d(e,j)},__wbg_set_min_binding_size_f64_897e3cd4496ddec9:function(A,e){A.minBindingSize=e},__wbg_set_min_filter_d435bbfc5a637757:function(A,e){A.minFilter=mr[e]},__wbg_set_mip_level_count_047936c630acee7b:function(A,e){A.mipLevelCount=e>>>0},__wbg_set_mip_level_count_44bc46a1ae6f6daa:function(A,e){A.mipLevelCount=e>>>0},__wbg_set_mip_level_f3745730372683d5:function(A,e){A.mipLevel=e>>>0},__wbg_set_mipmap_filter_62fb49a84b0747ff:function(A,e){A.mipmapFilter=Ot[e]},__wbg_set_miterLimit_db0797fa63d61672:function(A,e){A.miterLimit=e},__wbg_set_mode_7edfbc344ef9c650:function(A,e){A.mode=xt[e]},__wbg_set_module_392eeaa269f203b0:function(A,e){A.module=e},__wbg_set_module_715d37652c4998ec:function(A,e){A.module=e},__wbg_set_multiple_4a70bfda8eac6061:function(A,e){A.multiple=e!==0},__wbg_set_multisample_ff72a7a5456cbeb7:function(A,e){A.multisample=e},__wbg_set_multisampled_039f032dc4b67367:function(A,e){A.multisampled=e!==0},__wbg_set_name_ff6fb351f718f185:function(A,e,j){A.name=d(e,j)},__wbg_set_offset_f64_127e8a0aa5c5485a:function(A,e){A.offset=e},__wbg_set_offset_f64_457756429ede426d:function(A,e){A.offset=e},__wbg_set_offset_f64_a903425d5a8e5815:function(A,e){A.offset=e},__wbg_set_offset_f64_d1d115dd438165b5:function(A,e){A.offset=e},__wbg_set_once_7f65050c57557ff9:function(A,e){A.once=e!==0},__wbg_set_onclick_4d2a7dbf3f734065:function(A,e){A.onclick=e},__wbg_set_onended_c0d6e300da8b36ba:function(A,e){A.onended=e},__wbg_set_onload_a82519c1b28925a3:function(A,e){A.onload=e},__wbg_set_operation_00a77386523b88f9:function(A,e){A.operation=$t[e]},__wbg_set_optimize_for_latency_cbbb5776d26c5dca:function(A,e){A.optimizeForLatency=e!==0},__wbg_set_origin_gpu_origin_3d_dict_0619d4860adb4eb6:function(A,e){A.origin=e},__wbg_set_output_b3c608483e2b4d8e:function(A,e){A.output=e},__wbg_set_pass_op_3cf10feb3d76ab97:function(A,e){A.passOp=Kj[e]},__wbg_set_passive_acb4a6d8f5b98357:function(A,e){A.passive=e!==0},__wbg_set_power_preference_b42d00a8facfbade:function(A,e){A.powerPreference=Ct[e]},__wbg_set_prevent_scroll_012725f8a1602bdd:function(A,e){A.preventScroll=e!==0},__wbg_set_primitive_e796cf76f0ff89f3:function(A,e){A.primitive=e},__wbg_set_query_set_f030702f1b69199f:function(A,e){A.querySet=e},__wbg_set_r_6ece4d74af63364f:function(A,e){A.r=e},__wbg_set_reason_b72ca321818718aa:function(A,e,j){A.reason=d(e,j)},__wbg_set_required_features_bbab71414c45e621:function(A,e,j){A.requiredFeatures=eA(e,j)},__wbg_set_required_limits_837f62d865e7cfac:function(A,e){A.requiredLimits=e},__wbg_set_resolve_target_gpu_texture_view_e4c1e3bbb8c27d87:function(A,e){A.resolveTarget=e},__wbg_set_resource_8fd8658b30d86ecf:function(A,e){A.resource=e},__wbg_set_resource_gpu_buffer_binding_33099b25da65b610:function(A,e){A.resource=e},__wbg_set_resource_gpu_texture_view_4cffe7bc7c8e5cbe:function(A,e){A.resource=e},__wbg_set_rows_per_image_c6d50d227e634379:function(A,e){A.rowsPerImage=e>>>0},__wbg_set_rows_per_image_deb456502f23c260:function(A,e){A.rowsPerImage=e>>>0},__wbg_set_sample_count_481c255a12054e1d:function(A,e){A.sampleCount=e>>>0},__wbg_set_sample_rate_cf2746001d47fae8:function(A,e){A.sampleRate=e},__wbg_set_sample_type_ebc5fcd029513bda:function(A,e){A.sampleType=Kt[e]},__wbg_set_sampler_89cb4a7efcfc6005:function(A,e){A.sampler=e},__wbg_set_shader_location_3fb9f6a012eba494:function(A,e){A.shaderLocation=e>>>0},__wbg_set_size_f64_2f591b0654540477:function(A,e){A.size=e},__wbg_set_size_f64_e844c985b8f95261:function(A,e){A.size=e},__wbg_set_size_gpu_extent_3d_dict_adf57388ab1d4f18:function(A,e){A.size=e},__wbg_set_src_factor_6f2c9ec8e4d3d979:function(A,e){A.srcFactor=Er[e]},__wbg_set_stencil_back_c54d0443b8b6a957:function(A,e){A.stencilBack=e},__wbg_set_stencil_clear_value_a321b0e045bfd8c2:function(A,e){A.stencilClearValue=e>>>0},__wbg_set_stencil_front_3ff3f8385852efff:function(A,e){A.stencilFront=e},__wbg_set_stencil_load_op_37d20deccb26a0f1:function(A,e){A.stencilLoadOp=Sj[e]},__wbg_set_stencil_read_mask_021ef4271b24352c:function(A,e){A.stencilReadMask=e>>>0},__wbg_set_stencil_read_only_75fe66a2356d6e92:function(A,e){A.stencilReadOnly=e!==0},__wbg_set_stencil_store_op_501f91638dd386e6:function(A,e){A.stencilStoreOp=Mj[e]},__wbg_set_stencil_write_mask_ec1c12237e094bdd:function(A,e){A.stencilWriteMask=e>>>0},__wbg_set_step_mode_3cbbdeba1e5dfd62:function(A,e){A.stepMode=Lt[e]},__wbg_set_storage_texture_786aea7c5773b6c1:function(A,e){A.storageTexture=e},__wbg_set_store_op_678f33376d741711:function(A,e){A.storeOp=Mj[e]},__wbg_set_strip_index_format_70313df755145d5e:function(A,e){A.stripIndexFormat=Tj[e]},__wbg_set_strokeStyle_3b18520af1f47602:function(A,e){A.strokeStyle=e},__wbg_set_strokeStyle_cce50c69cecc2df7:function(A,e,j){A.strokeStyle=d(e,j)},__wbg_set_strokeStyle_cf68ead23facd1c2:function(A,e){A.strokeStyle=e},__wbg_set_tabIndex_a9b7f8d964a179f0:function(A,e){A.tabIndex=e},__wbg_set_target_e5c049109d0e2ff3:function(A,e,j){A.target=d(e,j)},__wbg_set_targets_674b33931e512fb1:function(A,e,j){A.targets=eA(e,j)},__wbg_set_texture_95f2bfdf7767e76f:function(A,e){A.texture=e},__wbg_set_texture_a33be3fe02ac6264:function(A,e){A.texture=e},__wbg_set_timestamp_b78581d700a08071:function(A,e){A.timestamp=e},__wbg_set_timestamp_writes_de6a09f299b71b76:function(A,e){A.timestampWrites=e},__wbg_set_tone_mapping_320c1aad31db2e7f:function(A,e){A.toneMapping=e},__wbg_set_topology_b92cfe523bd9653b:function(A,e){A.topology=Pt[e]},__wbg_set_type_062a978c6946048f:function(A,e,j){A.type=d(e,j)},__wbg_set_type_43e0092f16775979:function(A,e){A.type=Ht[e]},__wbg_set_type_79cec55caf4cdb6d:function(A,e){A.type=yt[e]},__wbg_set_type_7f7e54057b801caa:function(A,e){A.type=Rt[e]},__wbg_set_type_a170a1d376afa381:function(A,e,j){A.type=d(e,j)},__wbg_set_type_d27f05f3d41556ff:function(A,e){A.type=kt[e]},__wbg_set_unclipped_depth_32b7caf29fa5633d:function(A,e){A.unclippedDepth=e!==0},__wbg_set_usage_1ee33d98267e787d:function(A,e){A.usage=e>>>0},__wbg_set_usage_2365e2704b1fdb10:function(A,e){A.usage=e>>>0},__wbg_set_usage_d53ee6f0c7aedbfa:function(A,e){A.usage=e>>>0},__wbg_set_usage_f3e34822998d2147:function(A,e){A.usage=e>>>0},__wbg_set_value_22d56bead9380ee8:function(A,e,j){A.value=d(e,j)},__wbg_set_value_676e9d6f43f3c9e4:function(A,e,j){A.value=d(e,j)},__wbg_set_vertex_77ed7a1229239b5a:function(A,e){A.vertex=e},__wbg_set_view_dimension_893e2d16561e56e8:function(A,e){A.viewDimension=Lj[e]},__wbg_set_view_dimension_f2c5fe4bf927c3fe:function(A,e){A.viewDimension=Lj[e]},__wbg_set_view_formats_427069064d8b7139:function(A,e,j){A.viewFormats=eA(e,j)},__wbg_set_view_formats_9c2f01a6f3b365c7:function(A,e,j){A.viewFormats=eA(e,j)},__wbg_set_view_gpu_texture_view_35f4655788535c4d:function(A,e){A.view=e},__wbg_set_view_gpu_texture_view_a532c825c52042c0:function(A,e){A.view=e},__wbg_set_visibility_d8a6821789538c25:function(A,e){A.visibility=e>>>0},__wbg_set_width_36ef6630b22fc519:function(A,e){A.width=e>>>0},__wbg_set_width_661c95ea46b71eba:function(A,e){A.width=e>>>0},__wbg_set_width_b20525f5f4df4eb8:function(A,e){A.width=e>>>0},__wbg_set_write_mask_42d89f182ade6b2d:function(A,e){A.writeMask=e>>>0},__wbg_set_x_f470b03dd54724cd:function(A,e){A.x=e>>>0},__wbg_set_y_4c44eb40ebca5bfc:function(A,e){A.y=e>>>0},__wbg_set_z_2e6820ef0f5821ed:function(A,e){A.z=e>>>0},__wbg_shaderSource_7d3f360b4b626db7:function(A,e,j,a){A.shaderSource(e,d(j,a))},__wbg_shaderSource_dcba4cd3379b35bd:function(A,e,j,a){A.shaderSource(e,d(j,a))},__wbg_shiftKey_8eca009f693152b4:function(A){return A.shiftKey},__wbg_stack_3b0d974bbf31e44f:function(A,e){let j=e.stack,a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},__wbg_start_f2a1f4ed432f9992:function(){return n(function(A,e){A.start(e)},arguments)},__wbg_state_caf0b46b69f50923:function(A){let e=A.state;return(vt.indexOf(e)+1||4)-1},__wbg_static_accessor_GLOBAL_THIS_466428f93b4eaa76:function(){let A=typeof globalThis>"u"?null:globalThis;return p(A)?0:B(A)},__wbg_static_accessor_GLOBAL_c7aea38d4de089bc:function(){let A=typeof global>"u"?null:global;return p(A)?0:B(A)},__wbg_static_accessor_SELF_42d4fae05e59267a:function(){let A=typeof self>"u"?null:self;return p(A)?0:B(A)},__wbg_static_accessor_WINDOW_e0db14a0eba6a812:function(){let A=typeof window>"u"?null:window;return p(A)?0:B(A)},__wbg_statusText_fd389f44ebb1fc97:function(A,e){let j=e.statusText,a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},__wbg_status_b0de02a07fd7d927:function(A){return A.status},__wbg_stencilFuncSeparate_1455ac65895207da:function(A,e,j,a,t){A.stencilFuncSeparate(e>>>0,j>>>0,a,t>>>0)},__wbg_stencilFuncSeparate_55627e589746e09f:function(A,e,j,a,t){A.stencilFuncSeparate(e>>>0,j>>>0,a,t>>>0)},__wbg_stencilFunc_b5073faed00da15b:function(A,e,j,a){A.stencilFunc(e>>>0,j,a>>>0)},__wbg_stencilMaskSeparate_85d929ff95496631:function(A,e,j){A.stencilMaskSeparate(e>>>0,j>>>0)},__wbg_stencilMaskSeparate_8e37bf59a93afc15:function(A,e,j){A.stencilMaskSeparate(e>>>0,j>>>0)},__wbg_stencilMask_020d2d7ea8e4f640:function(A,e){A.stencilMask(e>>>0)},__wbg_stencilMask_967f16a89bfd056a:function(A,e){A.stencilMask(e>>>0)},__wbg_stencilOpSeparate_1f45c75c83dad8d5:function(A,e,j,a,t){A.stencilOpSeparate(e>>>0,j>>>0,a>>>0,t>>>0)},__wbg_stencilOpSeparate_33a6764dd0ce6e24:function(A,e,j,a,t){A.stencilOpSeparate(e>>>0,j>>>0,a>>>0,t>>>0)},__wbg_stencilOp_39d229912d4e6149:function(A,e,j,a){A.stencilOp(e>>>0,j>>>0,a>>>0)},__wbg_stringify_f93a4ebae9231922:function(){return n(function(A){return JSON.stringify(A)},arguments)},__wbg_stroke_7355965b9ad92428:function(A,e){A.stroke(e)},__wbg_style_f09d6445af3dd2c6:function(A){return A.style},__wbg_subgroupMaxSize_b43be0aa16182403:function(A){return A.subgroupMaxSize},__wbg_subgroupMinSize_03feb6ee0cda6775:function(A){return A.subgroupMinSize},__wbg_submit_077c85cc28e36892:function(A,e,j){A.submit(eA(e,j))},__wbg_submit_88800a9055f9a144:function(){return n(function(A){A.submit()},arguments)},__wbg_suppressContextMenu_4a707f9aedfbb422:function(A){A.suppressContextMenu()},__wbg_suspend_1a76515b500c012f:function(){return n(function(A){return A.suspend()},arguments)},__wbg_texImage2D_053488112c3d702f:function(){return n(function(A,e,j,a,t,o,f,s,i,b){A.texImage2D(e>>>0,j,a,t,o,f,s>>>0,i>>>0,b)},arguments)},__wbg_texImage2D_2854247ff7d047a1:function(){return n(function(A,e,j,a,t,o,f,s,i,b){A.texImage2D(e>>>0,j,a,t,o,f,s>>>0,i>>>0,b)},arguments)},__wbg_texImage2D_29d66757a5e1f95c:function(){return n(function(A,e,j,a,t,o,f,s,i,b,E){A.texImage2D(e>>>0,j,a,t,o,f,s>>>0,i>>>0,b===0?void 0:jA(b,E))},arguments)},__wbg_texImage2D_2d1f12e7c67a36d0:function(){return n(function(A,e,j,a,t,o,f,s,i,b,E){A.texImage2D(e>>>0,j,a,t,o,f,s>>>0,i>>>0,b===0?void 0:jA(b,E))},arguments)},__wbg_texImage2D_44740302c934daf1:function(){return n(function(A,e,j,a,t,o,f,s,i,b){A.texImage2D(e>>>0,j,a,t,o,f,s>>>0,i>>>0,b)},arguments)},__wbg_texImage3D_d23f7d2f9e66b916:function(){return n(function(A,e,j,a,t,o,f,s,i,b,E){A.texImage3D(e>>>0,j,a,t,o,f,s,i>>>0,b>>>0,E)},arguments)},__wbg_texImage3D_faae3ea3f2969ecc:function(){return n(function(A,e,j,a,t,o,f,s,i,b,E){A.texImage3D(e>>>0,j,a,t,o,f,s,i>>>0,b>>>0,E)},arguments)},__wbg_texParameteri_2bc38aa8e9964d77:function(A,e,j,a){A.texParameteri(e>>>0,j>>>0,a)},__wbg_texParameteri_dd4f56c2acbbe859:function(A,e,j,a){A.texParameteri(e>>>0,j>>>0,a)},__wbg_texStorage2D_d473a12d49d7deee:function(A,e,j,a,t,o){A.texStorage2D(e>>>0,j,a>>>0,t,o)},__wbg_texStorage3D_3ceb25ba9ad4b7ac:function(A,e,j,a,t,o,f){A.texStorage3D(e>>>0,j,a>>>0,t,o,f)},__wbg_texSubImage2D_1b383b66dfe35010:function(){return n(function(A,e,j,a,t,o,f,s,i,b){A.texSubImage2D(e>>>0,j,a,t,o,f,s>>>0,i>>>0,b)},arguments)},__wbg_texSubImage2D_205cfbaea80e77e6:function(){return n(function(A,e,j,a,t,o,f,s,i,b){A.texSubImage2D(e>>>0,j,a,t,o,f,s>>>0,i>>>0,b)},arguments)},__wbg_texSubImage2D_606540d3e650e0bb:function(){return n(function(A,e,j,a,t,o,f,s,i,b){A.texSubImage2D(e>>>0,j,a,t,o,f,s>>>0,i>>>0,b)},arguments)},__wbg_texSubImage2D_62ae3d4b2700f7cd:function(){return n(function(A,e,j,a,t,o,f,s,i,b){A.texSubImage2D(e>>>0,j,a,t,o,f,s>>>0,i>>>0,b)},arguments)},__wbg_texSubImage2D_6eb05d8f455f99ba:function(){return n(function(A,e,j,a,t,o,f,s,i,b){A.texSubImage2D(e>>>0,j,a,t,o,f,s>>>0,i>>>0,b)},arguments)},__wbg_texSubImage2D_a035d2307e014a73:function(){return n(function(A,e,j,a,t,o,f,s,i,b){A.texSubImage2D(e>>>0,j,a,t,o,f,s>>>0,i>>>0,b)},arguments)},__wbg_texSubImage2D_ad5a64d8f68a2d0d:function(){return n(function(A,e,j,a,t,o,f,s,i,b){A.texSubImage2D(e>>>0,j,a,t,o,f,s>>>0,i>>>0,b)},arguments)},__wbg_texSubImage2D_cb9ad676165c5da5:function(){return n(function(A,e,j,a,t,o,f,s,i,b){A.texSubImage2D(e>>>0,j,a,t,o,f,s>>>0,i>>>0,b)},arguments)},__wbg_texSubImage2D_db54df8f6445f113:function(){return n(function(A,e,j,a,t,o,f,s,i,b){A.texSubImage2D(e>>>0,j,a,t,o,f,s>>>0,i>>>0,b)},arguments)},__wbg_texSubImage3D_09e44c66b4ac6bc6:function(){return n(function(A,e,j,a,t,o,f,s,i,b,E,h){A.texSubImage3D(e>>>0,j,a,t,o,f,s,i,b>>>0,E>>>0,h)},arguments)},__wbg_texSubImage3D_16678785ac62fd6b:function(){return n(function(A,e,j,a,t,o,f,s,i,b,E,h){A.texSubImage3D(e>>>0,j,a,t,o,f,s,i,b>>>0,E>>>0,h)},arguments)},__wbg_texSubImage3D_3ee8764dfdcb6746:function(){return n(function(A,e,j,a,t,o,f,s,i,b,E,h){A.texSubImage3D(e>>>0,j,a,t,o,f,s,i,b>>>0,E>>>0,h)},arguments)},__wbg_texSubImage3D_53489be691cee78d:function(){return n(function(A,e,j,a,t,o,f,s,i,b,E,h){A.texSubImage3D(e>>>0,j,a,t,o,f,s,i,b>>>0,E>>>0,h)},arguments)},__wbg_texSubImage3D_73d365baf8dad003:function(){return n(function(A,e,j,a,t,o,f,s,i,b,E,h){A.texSubImage3D(e>>>0,j,a,t,o,f,s,i,b>>>0,E>>>0,h)},arguments)},__wbg_texSubImage3D_8a2331639ee1ee0e:function(){return n(function(A,e,j,a,t,o,f,s,i,b,E,h){A.texSubImage3D(e>>>0,j,a,t,o,f,s,i,b>>>0,E>>>0,h)},arguments)},__wbg_texSubImage3D_9b0bd9fd73d7bb1c:function(){return n(function(A,e,j,a,t,o,f,s,i,b,E,h){A.texSubImage3D(e>>>0,j,a,t,o,f,s,i,b>>>0,E>>>0,h)},arguments)},__wbg_texSubImage3D_fcc8b10e5c1a3b28:function(){return n(function(A,e,j,a,t,o,f,s,i,b,E,h){A.texSubImage3D(e>>>0,j,a,t,o,f,s,i,b>>>0,E>>>0,h)},arguments)},__wbg_then_7026b513a94278a8:function(A,e){return A.then(e)},__wbg_then_72819b8d4e081fb5:function(A,e,j){return A.then(e,j)},__wbg_toString_033acf19ce89359c:function(A){return A.toString()},__wbg_transform_62123174d9cd3977:function(){return n(function(A,e,j,a,t,o,f){A.transform(e,j,a,t,o,f)},arguments)},__wbg_unconfigure_835307f58dc68d80:function(A){A.unconfigure()},__wbg_uniform1f_e92095ce29c38424:function(A,e,j){A.uniform1f(e,j)},__wbg_uniform1f_e93503bc589b432d:function(A,e,j){A.uniform1f(e,j)},__wbg_uniform1fv_b3953ed7fd6bb740:function(A,e,j,a){A.uniform1fv(e,R(j,a))},__wbg_uniform1i_235dff1d94e0df95:function(A,e,j){A.uniform1i(e,j)},__wbg_uniform1i_d5db9c3184abbd04:function(A,e,j){A.uniform1i(e,j)},__wbg_uniform1ui_8bbaaa1161bfd433:function(A,e,j){A.uniform1ui(e,j>>>0)},__wbg_uniform2fv_1443080aaf9c1077:function(A,e,j,a){A.uniform2fv(e,R(j,a))},__wbg_uniform2fv_b039f28911c30526:function(A,e,j,a){A.uniform2fv(e,R(j,a))},__wbg_uniform2iv_9648a06d054a25aa:function(A,e,j,a){A.uniform2iv(e,_A(j,a))},__wbg_uniform2iv_e0496dc424dc25ec:function(A,e,j,a){A.uniform2iv(e,_A(j,a))},__wbg_uniform2uiv_935dfb31f50dfbe3:function(A,e,j,a){A.uniform2uiv(e,zA(j,a))},__wbg_uniform3fv_025760367cc4eed3:function(A,e,j,a){A.uniform3fv(e,R(j,a))},__wbg_uniform3fv_b985d45f54156d3b:function(A,e,j,a){A.uniform3fv(e,R(j,a))},__wbg_uniform3iv_193b7a0e1ae9ac9a:function(A,e,j,a){A.uniform3iv(e,_A(j,a))},__wbg_uniform3iv_63e82687b07e66fc:function(A,e,j,a){A.uniform3iv(e,_A(j,a))},__wbg_uniform3uiv_ccd86b78a5fb3077:function(A,e,j,a){A.uniform3uiv(e,zA(j,a))},__wbg_uniform4f_61192d516e9bede4:function(A,e,j,a,t,o){A.uniform4f(e,j,a,t,o)},__wbg_uniform4f_d9bb623add5d2541:function(A,e,j,a,t,o){A.uniform4f(e,j,a,t,o)},__wbg_uniform4fv_c39527800fc76c8e:function(A,e,j,a){A.uniform4fv(e,R(j,a))},__wbg_uniform4fv_fcff56a650906708:function(A,e,j,a){A.uniform4fv(e,R(j,a))},__wbg_uniform4iv_197c2f54a8dfb5c2:function(A,e,j,a){A.uniform4iv(e,_A(j,a))},__wbg_uniform4iv_9e6e36f0e1d1f84d:function(A,e,j,a){A.uniform4iv(e,_A(j,a))},__wbg_uniform4uiv_73fc9e298d02c948:function(A,e,j,a){A.uniform4uiv(e,zA(j,a))},__wbg_uniformBlockBinding_057177606c8b522f:function(A,e,j,a){A.uniformBlockBinding(e,j>>>0,a>>>0)},__wbg_uniformMatrix2fv_013723900a9cb65c:function(A,e,j,a,t){A.uniformMatrix2fv(e,j!==0,R(a,t))},__wbg_uniformMatrix2fv_fb61eccac67a8218:function(A,e,j,a,t){A.uniformMatrix2fv(e,j!==0,R(a,t))},__wbg_uniformMatrix2x3fv_de8b00219f47ffb4:function(A,e,j,a,t){A.uniformMatrix2x3fv(e,j!==0,R(a,t))},__wbg_uniformMatrix2x4fv_e659cc34e95fee5e:function(A,e,j,a,t){A.uniformMatrix2x4fv(e,j!==0,R(a,t))},__wbg_uniformMatrix3fv_3e548032fc28c3e2:function(A,e,j,a,t){A.uniformMatrix3fv(e,j!==0,R(a,t))},__wbg_uniformMatrix3fv_72ca83d3393e0364:function(A,e,j,a,t){A.uniformMatrix3fv(e,j!==0,R(a,t))},__wbg_uniformMatrix3x2fv_8598636e806d318d:function(A,e,j,a,t){A.uniformMatrix3x2fv(e,j!==0,R(a,t))},__wbg_uniformMatrix3x4fv_277fbf38db85e612:function(A,e,j,a,t){A.uniformMatrix3x4fv(e,j!==0,R(a,t))},__wbg_uniformMatrix4fv_20161efad644f822:function(A,e,j,a,t){A.uniformMatrix4fv(e,j!==0,R(a,t))},__wbg_uniformMatrix4fv_8689fd0481ac5ab4:function(A,e,j,a,t){A.uniformMatrix4fv(e,j!==0,R(a,t))},__wbg_uniformMatrix4x2fv_e91bd4e774f6266d:function(A,e,j,a,t){A.uniformMatrix4x2fv(e,j!==0,R(a,t))},__wbg_uniformMatrix4x3fv_a829c88dfd0c29d3:function(A,e,j,a,t){A.uniformMatrix4x3fv(e,j!==0,R(a,t))},__wbg_unmap_6a96b14c9ef5f7f5:function(A){A.unmap()},__wbg_url_82c95d5d2e2ba977:function(A,e){let j=e.url,a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g;m().setInt32(A+4,t,!0),m().setInt32(A+0,a,!0)},__wbg_useProgram_1c047de878f20b72:function(A,e){A.useProgram(e)},__wbg_useProgram_9edff145e073d3b1:function(A,e){A.useProgram(e)},__wbg_userActivation_7f2f2f2659ad0d1a:function(A){return A.userActivation},__wbg_value_1e2369fab29b420e:function(A){return A.value},__wbg_values_2a12fb5a6064a244:function(A){return A.values()},__wbg_vertexAttribDivisorANGLE_581f060f68a0c850:function(A,e,j){A.vertexAttribDivisorANGLE(e>>>0,j>>>0)},__wbg_vertexAttribDivisor_f910af52b19ce382:function(A,e,j){A.vertexAttribDivisor(e>>>0,j>>>0)},__wbg_vertexAttribIPointer_54e6be6fa5e39567:function(A,e,j,a,t,o){A.vertexAttribIPointer(e>>>0,j,a>>>0,t,o)},__wbg_vertexAttribPointer_7bc186aca7721b90:function(A,e,j,a,t,o,f){A.vertexAttribPointer(e>>>0,j,a>>>0,t!==0,o,f)},__wbg_vertexAttribPointer_b0838f8618a8c446:function(A,e,j,a,t,o,f){A.vertexAttribPointer(e>>>0,j,a>>>0,t!==0,o,f)},__wbg_view_7685fe4b2845c5b6:function(A){let e=A.view;return p(e)?0:B(e)},__wbg_viewport_07bb1829f0fe2245:function(A,e,j,a,t){A.viewport(e,j,a,t)},__wbg_viewport_dfe81d333ce7be86:function(A,e,j,a,t){A.viewport(e,j,a,t)},__wbg_visibleRect_0d5e95bfe9d464ca:function(A){let e=A.visibleRect;return p(e)?0:B(e)},__wbg_wasClean_76925d0fb8cf2795:function(A){return A.wasClean},__wbg_width_1952934caca67137:function(A){return A.width},__wbg_width_25247161d477c7d5:function(A){return A.width},__wbg_width_4bb073b449891b57:function(A){return A.width},__wbg_width_64eb09b40bf1526e:function(A){return A.width},__wbg_width_aeade399d283e83a:function(A){return A.width},__wbg_writeTexture_30e592e8c061c3d9:function(){return n(function(A,e,j,a,t,o){A.writeTexture(e,jA(j,a),t,o)},arguments)},__wbindgen_cast_0000000000000001:function(A,e){return K(A,e,mt)},__wbindgen_cast_0000000000000002:function(A,e){return K(A,e,qt)},__wbindgen_cast_0000000000000003:function(A,e){return K(A,e,ct)},__wbindgen_cast_0000000000000004:function(A,e){return K(A,e,ft)},__wbindgen_cast_0000000000000005:function(A,e){return Gr(A,e,nt)},__wbindgen_cast_0000000000000006:function(A,e){return K(A,e,st)},__wbindgen_cast_0000000000000007:function(A,e){return K(A,e,lt)},__wbindgen_cast_0000000000000008:function(A,e){return K(A,e,gt)},__wbindgen_cast_0000000000000009:function(A,e){return K(A,e,dt)},__wbindgen_cast_000000000000000a:function(A,e){return K(A,e,it)},__wbindgen_cast_000000000000000b:function(A,e){return K(A,e,bt)},__wbindgen_cast_000000000000000c:function(A,e){return K(A,e,ut)},__wbindgen_cast_000000000000000d:function(A,e){return Gr(A,e,_t)},__wbindgen_cast_000000000000000e:function(A,e){return K(A,e,pt)},__wbindgen_cast_000000000000000f:function(A,e){return K(A,e,Et)},__wbindgen_cast_0000000000000010:function(A,e){return K(A,e,Bt)},__wbindgen_cast_0000000000000011:function(A,e){return K(A,e,ht)},__wbindgen_cast_0000000000000012:function(A,e){return K(A,e,tt)},__wbindgen_cast_0000000000000013:function(A,e){return K(A,e,ot)},__wbindgen_cast_0000000000000014:function(A){return A},__wbindgen_cast_0000000000000015:function(A,e){return R(A,e)},__wbindgen_cast_0000000000000016:function(A,e){return Qt(A,e)},__wbindgen_cast_0000000000000017:function(A,e){return _A(A,e)},__wbindgen_cast_0000000000000018:function(A,e){return Yt(A,e)},__wbindgen_cast_0000000000000019:function(A,e){return Ao(A,e)},__wbindgen_cast_000000000000001a:function(A,e){return zA(A,e)},__wbindgen_cast_000000000000001b:function(A,e){return jA(A,e)},__wbindgen_cast_000000000000001c:function(A,e){return d(A,e)},__wbindgen_init_externref_table:function(){let A=c.__wbindgen_externrefs,e=A.grow(4);A.set(0,void 0),A.set(e+0,void 0),A.set(e+1,null),A.set(e+2,!0),A.set(e+3,!1)}}}}function tt(r,A){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke_______true_(r,A)}function ot(r,A){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke_______true__1_(r,A)}function ct(r,A,e){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___web_sys_4c1534065d961aca___features__gen_FocusEvent__FocusEvent______true_(r,A,e)}function ft(r,A,e){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___web_sys_4c1534065d961aca___features__gen_CloseEvent__CloseEvent______true_(r,A,e)}function nt(r,A,e){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___web_sys_4c1534065d961aca___features__gen_VideoFrame__VideoFrame______true_(r,A,e)}function st(r,A,e){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___web_sys_4c1534065d961aca___features__gen_CloseEvent__CloseEvent______true__5(r,A,e)}function lt(r,A,e){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___web_sys_4c1534065d961aca___features__gen_FocusEvent__FocusEvent______true__6(r,A,e)}function dt(r,A,e){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___web_sys_4c1534065d961aca___features__gen_FocusEvent__FocusEvent______true__8(r,A,e)}function it(r,A,e){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___web_sys_4c1534065d961aca___features__gen_CloseEvent__CloseEvent______true__9(r,A,e)}function bt(r,A,e){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___web_sys_4c1534065d961aca___features__gen_FocusEvent__FocusEvent______true__10(r,A,e)}function ut(r,A,e){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___web_sys_4c1534065d961aca___features__gen_FocusEvent__FocusEvent______true__11(r,A,e)}function _t(r,A,e){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___web_sys_4c1534065d961aca___features__gen_VideoFrame__VideoFrame______true__12(r,A,e)}function pt(r,A,e){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___web_sys_4c1534065d961aca___features__gen_FocusEvent__FocusEvent______true__13(r,A,e)}function Et(r,A,e){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___web_sys_4c1534065d961aca___features__gen_FocusEvent__FocusEvent______true__14(r,A,e)}function mt(r,A,e){let j=c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___wasm_bindgen_f05820bf1e6ccf5d___JsValue__core_f0fd674eaa06beef___result__Result_____wasm_bindgen_f05820bf1e6ccf5d___JsError___true_(r,A,e);if(j[1])throw xA(j[0])}function gt(r,A,e){let j=c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___wasm_bindgen_f05820bf1e6ccf5d___sys__JsNullable_wgpu_1dafc1c682c4dde2___backend__webgpu__webgpu_sys__gen_GpuError__GpuError___core_f0fd674eaa06beef___result__Result_____wasm_bindgen_f05820bf1e6ccf5d___JsError___true_(r,A,e);if(j[1])throw xA(j[0])}function Bt(r,A,e){let j=c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___wasm_bindgen_f05820bf1e6ccf5d___sys__JsNullable_wgpu_1dafc1c682c4dde2___backend__webgpu__webgpu_sys__gen_GpuError__GpuError___core_f0fd674eaa06beef___result__Result_____wasm_bindgen_f05820bf1e6ccf5d___JsError___true__15(r,A,e);if(j[1])throw xA(j[0])}function ht(r,A,e){let j=c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___wasm_bindgen_f05820bf1e6ccf5d___sys__JsNullable_wgpu_1dafc1c682c4dde2___backend__webgpu__webgpu_sys__gen_GpuError__GpuError___core_f0fd674eaa06beef___result__Result_____wasm_bindgen_f05820bf1e6ccf5d___JsError___true__16(r,A,e);if(j[1])throw xA(j[0])}function _r(r,A,e,j){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___js_sys_9c530fb7440210e4___Function_fn_wasm_bindgen_f05820bf1e6ccf5d___JsValue_____wasm_bindgen_f05820bf1e6ccf5d___sys__Undefined___js_sys_9c530fb7440210e4___Function_fn_wasm_bindgen_f05820bf1e6ccf5d___JsValue_____wasm_bindgen_f05820bf1e6ccf5d___sys__Undefined_______true_(r,A,e,j)}function qt(r,A,e){c.wasm_bindgen_f05820bf1e6ccf5d___convert__closures_____invoke___f64______true_(r,A,e)}function B(r){let A=c.__externref_table_alloc();return c.__wbindgen_externrefs.set(A,r),A}function Vj(r){let A=typeof r;if(A=="number"||A=="boolean"||r==null)return`${r}`;if(A=="string")return`"${r}"`;if(A=="symbol"){let a=r.description;return a==null?"Symbol":`Symbol(${a})`}if(A=="function"){let a=r.name;return typeof a=="string"&&a.length>0?`Function(${a})`:"Function"}if(Array.isArray(r)){let a=r.length,t="[";a>0&&(t+=Vj(r[0]));for(let o=1;o<a;o++)t+=", "+Vj(r[o]);return t+="]",t}let e=/\[object ([^\]]+)\]/.exec(toString.call(r)),j;if(e&&e.length>1)j=e[1];else return toString.call(r);if(j=="Object")try{return"Object("+JSON.stringify(r)+")"}catch{return"Object"}return r instanceof Error?`${r.name}: ${r.message}
${r.stack}`:j}function R(r,A){return r=r>>>0,jo().subarray(r/4,r/4+A)}function Jt(r,A){return r=r>>>0,ao().subarray(r/8,r/8+A)}function Qt(r,A){return r=r>>>0,ro().subarray(r/2,r/2+A)}function _A(r,A){return r=r>>>0,to().subarray(r/4,r/4+A)}function Yt(r,A){return r=r>>>0,oo().subarray(r/1,r/1+A)}function Zt(r,A){r=r>>>0;let e=m(),j=[];for(let a=r;a<r+4*A;a+=4)j.push(c.__wbindgen_externrefs.get(e.getUint32(a,!0)));return c.__externref_drop_slice(r,A),j}function eA(r,A){r=r>>>0;let e=m(),j=[];for(let a=r;a<r+4*A;a+=4)j.push(c.__wbindgen_externrefs.get(e.getUint32(a,!0)));return j}function Ao(r,A){return r=r>>>0,co().subarray(r/2,r/2+A)}function zA(r,A){return r=r>>>0,fo().subarray(r/4,r/4+A)}function jA(r,A){return r=r>>>0,WA().subarray(r/1,r/1+A)}function eo(r,A){return r=r>>>0,no().subarray(r/1,r/1+A)}function m(){return(wA===null||wA.buffer.detached===!0||wA.buffer.detached===void 0&&wA.buffer!==c.memory.buffer)&&(wA=new DataView(c.memory.buffer)),wA}function jo(){return(oe===null||oe.byteLength===0)&&(oe=new Float32Array(c.memory.buffer)),oe}function ao(){return(ce===null||ce.byteLength===0)&&(ce=new Float64Array(c.memory.buffer)),ce}function ro(){return(fe===null||fe.byteLength===0)&&(fe=new Int16Array(c.memory.buffer)),fe}function to(){return(ne===null||ne.byteLength===0)&&(ne=new Int32Array(c.memory.buffer)),ne}function oo(){return(se===null||se.byteLength===0)&&(se=new Int8Array(c.memory.buffer)),se}function d(r,A){return lo(r>>>0,A)}function co(){return(le===null||le.byteLength===0)&&(le=new Uint16Array(c.memory.buffer)),le}function fo(){return(de===null||de.byteLength===0)&&(de=new Uint32Array(c.memory.buffer)),de}function WA(){return(ie===null||ie.byteLength===0)&&(ie=new Uint8Array(c.memory.buffer)),ie}function no(){return(be===null||be.byteLength===0)&&(be=new Uint8ClampedArray(c.memory.buffer)),be}function n(r,A){try{return r.apply(this,A)}catch(e){let j=B(e);c.__wbindgen_exn_store(j)}}function p(r){return r==null}function Gr(r,A,e){let j={a:r,b:A,cnt:1},a=(...t)=>{j.cnt++;try{return e(j.a,j.b,...t)}finally{a._wbg_cb_unref()}};return a._wbg_cb_unref=()=>{--j.cnt===0&&(c.__wbindgen_destroy_closure(j.a,j.b),j.a=0,Aj.unregister(j))},Aj.register(a,j,j),a}function K(r,A,e){let j={a:r,b:A,cnt:1},a=(...t)=>{j.cnt++;let o=j.a;j.a=0;try{return e(o,j.b,...t)}finally{j.a=o,a._wbg_cb_unref()}};return a._wbg_cb_unref=()=>{--j.cnt===0&&(c.__wbindgen_destroy_closure(j.a,j.b),j.a=0,Aj.unregister(j))},Aj.register(a,j,j),a}function zj(r,A){let e=A(r.length*1,1)>>>0;return WA().set(r,e/1),g=r.length,e}function Xj(r,A){let e=A(r.length*4,4)>>>0;for(let j=0;j<r.length;j++){let a=B(r[j]);m().setUint32(e+4*j,a,!0)}return g=r.length,e}function G(r,A,e){if(e===void 0){let f=ue.encode(r),s=A(f.length,1)>>>0;return WA().subarray(s,s+f.length).set(f),g=f.length,s}let j=r.length,a=A(j,1)>>>0,t=WA(),o=0;for(;o<j;o++){let f=r.charCodeAt(o);if(f>127)break;t[a+o]=f}if(o!==j){o!==0&&(r=r.slice(o)),a=e(a,j,j=o+r.length*3,1)>>>0;let f=WA().subarray(a+o,a+j),s=ue.encodeInto(r,f);o+=s.written,a=e(a,j,o,1)>>>0}return g=o,a}function xA(r){let A=c.__wbindgen_externrefs.get(r);return c.__externref_table_dealloc(r),A}function lo(r,A){return Uj+=A,Uj>=so&&(Ze=new TextDecoder("utf-8",{ignoreBOM:!0,fatal:!0}),Ze.decode(),Uj=A),Ze.decode(WA().subarray(r,r+A))}function kr(r,A){return bo=r,c=r.exports,io=A,wA=null,oe=null,ce=null,fe=null,ne=null,se=null,le=null,de=null,ie=null,be=null,c.__wbindgen_start(),c}async function uo(r,A){if(typeof Response=="function"&&r instanceof Response){if(!r.ok)throw new Error(`failed to fetch Wasm: ${r.status} ${r.statusText} fetching '${r.url}'`);if(typeof WebAssembly.instantiateStreaming=="function")try{return await WebAssembly.instantiateStreaming(r,A)}catch(a){if(e(r.type)&&r.headers.get("Content-Type")!=="application/wasm")console.warn("`WebAssembly.instantiateStreaming` failed because your server does not serve Wasm with `application/wasm` MIME type. Falling back to `WebAssembly.instantiate` which is slower. Original error:\n",a);else throw a}let j=await r.arrayBuffer();return await WebAssembly.instantiate(j,A)}else{let j=await WebAssembly.instantiate(r,A);return j instanceof WebAssembly.Instance?{instance:j,module:r}:j}function e(j){switch(j){case"basic":case"cors":case"default":return!0}return!1}}function _o(r){if(c!==void 0)return c;r!==void 0&&(Object.getPrototypeOf(r)===Object.prototype?{module:r}=r:console.warn("using deprecated parameters for `initSync()`; pass a single object instead"));let A=vr();r instanceof WebAssembly.Module||(r=new WebAssembly.Module(r));let e=new WebAssembly.Instance(r,A);return kr(e,r)}async function po(r){if(c!==void 0)return c;r!==void 0&&(Object.getPrototypeOf(r)===Object.prototype?{module_or_path:r}=r:console.warn("using deprecated parameters for the initialization function; pass a single object instead"));let A=vr();(typeof r=="string"||typeof Request=="function"&&r instanceof Request||typeof URL=="function"&&r instanceof URL)&&(r=fetch(r));let{instance:e,module:j}=await uo(await r,A);return kr(e,j)}var _e,pe,Ee,NA,me,ge,rt,Gt,pr,vt,kt,Rj,Ft,Er,$t,yt,wt,xt,Hj,It,mr,Dt,Tj,Sj,Ot,Ct,Pt,Rt,Ht,Kj,Tt,Mj,gr,St,yA,Kt,Lj,Mt,Lt,Ut,Vt,Xt,zt,Wt,Nt,Br,hr,qr,Aj,wA,oe,ce,fe,ne,se,le,de,ie,be,Ze,so,Uj,ue,g,io,bo,c,Nj=De(()=>{"use strict";l();ur();_e=class{__destroy_into_raw(){let A=this.__wbg_ptr;return this.__wbg_ptr=0,zt.unregister(this),A}free(){let A=this.__destroy_into_raw();c.__wbg_intounderlyingbytesource_free(A,0)}get autoAllocateChunkSize(){return c.intounderlyingbytesource_autoAllocateChunkSize(this.__wbg_ptr)>>>0}cancel(){let A=this.__destroy_into_raw();c.intounderlyingbytesource_cancel(A)}pull(A){return c.intounderlyingbytesource_pull(this.__wbg_ptr,A)}start(A){c.intounderlyingbytesource_start(this.__wbg_ptr,A)}get type(){let A=c.intounderlyingbytesource_type(this.__wbg_ptr);return Ut[A]}};Symbol.dispose&&(_e.prototype[Symbol.dispose]=_e.prototype.free);pe=class{__destroy_into_raw(){let A=this.__wbg_ptr;return this.__wbg_ptr=0,Wt.unregister(this),A}free(){let A=this.__destroy_into_raw();c.__wbg_intounderlyingsink_free(A,0)}abort(A){let e=this.__destroy_into_raw();return c.intounderlyingsink_abort(e,A)}close(){let A=this.__destroy_into_raw();return c.intounderlyingsink_close(A)}write(A){return c.intounderlyingsink_write(this.__wbg_ptr,A)}};Symbol.dispose&&(pe.prototype[Symbol.dispose]=pe.prototype.free);Ee=class{__destroy_into_raw(){let A=this.__wbg_ptr;return this.__wbg_ptr=0,Nt.unregister(this),A}free(){let A=this.__destroy_into_raw();c.__wbg_intounderlyingsource_free(A,0)}cancel(){let A=this.__destroy_into_raw();c.intounderlyingsource_cancel(A)}pull(A){return c.intounderlyingsource_pull(this.__wbg_ptr,A)}};Symbol.dispose&&(Ee.prototype[Symbol.dispose]=Ee.prototype.free);NA=class r{static __wrap(A){let e=Object.create(r.prototype);return e.__wbg_ptr=A,Br.register(e,e.__wbg_ptr,e),e}__destroy_into_raw(){let A=this.__wbg_ptr;return this.__wbg_ptr=0,Br.unregister(this),A}free(){let A=this.__destroy_into_raw();c.__wbg_rufflehandle_free(A,0)}audio_context(){return c.rufflehandle_audio_context(this.__wbg_ptr)}call_exposed_callback(A,e){let j=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),a=g,t=Xj(e,c.__wbindgen_malloc),o=g;return c.rufflehandle_call_exposed_callback(this.__wbg_ptr,j,a,t,o)}clear_custom_menu_items(){c.rufflehandle_clear_custom_menu_items(this.__wbg_ptr)}destroy(){c.rufflehandle_destroy(this.__wbg_ptr)}enable_background_tick_mode(){c.rufflehandle_enable_background_tick_mode(this.__wbg_ptr)}has_focus(){return c.rufflehandle_has_focus(this.__wbg_ptr)!==0}is_playing(){return c.rufflehandle_is_playing(this.__wbg_ptr)!==0}static is_wasm_simd_used(){return c.rufflehandle_is_wasm_simd_used()!==0}load_data(A,e,j){let a=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g,o=c.rufflehandle_load_data(this.__wbg_ptr,A,e,a,t);if(o[1])throw xA(o[0])}pause(){c.rufflehandle_pause(this.__wbg_ptr)}play(){c.rufflehandle_play(this.__wbg_ptr)}prepare_context_menu(){return c.rufflehandle_prepare_context_menu(this.__wbg_ptr)}renderer_debug_info(){return c.rufflehandle_renderer_debug_info(this.__wbg_ptr)}renderer_name(){return c.rufflehandle_renderer_name(this.__wbg_ptr)}restart_animation_loop(){c.rufflehandle_restart_animation_loop(this.__wbg_ptr)}run_context_menu_callback(A){return c.rufflehandle_run_context_menu_callback(this.__wbg_ptr,A)}set_fullscreen(A){c.rufflehandle_set_fullscreen(this.__wbg_ptr,A)}set_trace_observer(A){c.rufflehandle_set_trace_observer(this.__wbg_ptr,A)}set_volume(A){c.rufflehandle_set_volume(this.__wbg_ptr,A)}stream_from(A,e){let j=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),a=g,t=c.rufflehandle_stream_from(this.__wbg_ptr,j,a,e);if(t[1])throw xA(t[0])}tick_for_background(A){c.rufflehandle_tick_for_background(this.__wbg_ptr,A)}volume(){return c.rufflehandle_volume(this.__wbg_ptr)}};Symbol.dispose&&(NA.prototype[Symbol.dispose]=NA.prototype.free);me=class{toJSON(){return{}}toString(){return JSON.stringify(this)}__destroy_into_raw(){let A=this.__wbg_ptr;return this.__wbg_ptr=0,hr.unregister(this),A}free(){let A=this.__destroy_into_raw();c.__wbg_ruffleinstancebuilder_free(A,0)}addFont(A,e){let j=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),a=g,t=zj(e,c.__wbindgen_malloc),o=g;c.ruffleinstancebuilder_addFont(this.__wbg_ptr,j,a,t,o)}addGamepadButtonMapping(A,e){let j=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),a=g;c.ruffleinstancebuilder_addGamepadButtonMapping(this.__wbg_ptr,j,a,e)}addSocketProxy(A,e,j){let a=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),t=g,o=G(j,c.__wbindgen_malloc,c.__wbindgen_realloc),f=g;c.ruffleinstancebuilder_addSocketProxy(this.__wbg_ptr,a,t,e,o,f)}addUrlRewriteRule(A,e){let j=G(e,c.__wbindgen_malloc,c.__wbindgen_realloc),a=g;c.ruffleinstancebuilder_addUrlRewriteRule(this.__wbg_ptr,A,j,a)}build(A,e){return c.ruffleinstancebuilder_build(this.__wbg_ptr,A,e)}constructor(){let A=c.ruffleinstancebuilder_new();return this.__wbg_ptr=A,hr.register(this,this.__wbg_ptr,this),this}setAllowFullscreen(A){c.ruffleinstancebuilder_setAllowFullscreen(this.__wbg_ptr,A)}setAllowNetworking(A){let e=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),j=g;c.ruffleinstancebuilder_setAllowNetworking(this.__wbg_ptr,e,j)}setAllowScriptAccess(A){c.ruffleinstancebuilder_setAllowScriptAccess(this.__wbg_ptr,A)}setBackgroundColor(A){c.ruffleinstancebuilder_setBackgroundColor(this.__wbg_ptr,p(A)?Number.MAX_SAFE_INTEGER:A>>>0)}setBaseUrl(A){var e=p(A)?0:G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),j=g;c.ruffleinstancebuilder_setBaseUrl(this.__wbg_ptr,e,j)}setCompatibilityRules(A){c.ruffleinstancebuilder_setCompatibilityRules(this.__wbg_ptr,A)}setCredentialAllowList(A){let e=Xj(A,c.__wbindgen_malloc),j=g;c.ruffleinstancebuilder_setCredentialAllowList(this.__wbg_ptr,e,j)}setDefaultFont(A,e){let j=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),a=g,t=Xj(e,c.__wbindgen_malloc),o=g;c.ruffleinstancebuilder_setDefaultFont(this.__wbg_ptr,j,a,t,o)}setDeviceFontRenderer(A){let e=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),j=g;c.ruffleinstancebuilder_setDeviceFontRenderer(this.__wbg_ptr,e,j)}setForceAlign(A){c.ruffleinstancebuilder_setForceAlign(this.__wbg_ptr,A)}setForceScale(A){c.ruffleinstancebuilder_setForceScale(this.__wbg_ptr,A)}setFrameRate(A){c.ruffleinstancebuilder_setFrameRate(this.__wbg_ptr,!p(A),p(A)?0:A)}setLetterbox(A){let e=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),j=g;c.ruffleinstancebuilder_setLetterbox(this.__wbg_ptr,e,j)}setLogLevel(A){let e=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),j=g;c.ruffleinstancebuilder_setLogLevel(this.__wbg_ptr,e,j)}setMaxExecutionDuration(A){c.ruffleinstancebuilder_setMaxExecutionDuration(this.__wbg_ptr,A)}setOpenUrlMode(A){let e=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),j=g;c.ruffleinstancebuilder_setOpenUrlMode(this.__wbg_ptr,e,j)}setPlayerRuntime(A){let e=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),j=g;c.ruffleinstancebuilder_setPlayerRuntime(this.__wbg_ptr,e,j)}setPlayerVersion(A){c.ruffleinstancebuilder_setPlayerVersion(this.__wbg_ptr,p(A)?16777215:A)}setPreferredRenderer(A){var e=p(A)?0:G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),j=g;c.ruffleinstancebuilder_setPreferredRenderer(this.__wbg_ptr,e,j)}setQuality(A){let e=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),j=g;c.ruffleinstancebuilder_setQuality(this.__wbg_ptr,e,j)}setScale(A){let e=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),j=g;c.ruffleinstancebuilder_setScale(this.__wbg_ptr,e,j)}setScrollingBehavior(A){let e=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),j=g;c.ruffleinstancebuilder_setScrollingBehavior(this.__wbg_ptr,e,j)}setShowMenu(A){c.ruffleinstancebuilder_setShowMenu(this.__wbg_ptr,A)}setStageAlign(A){let e=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),j=g;c.ruffleinstancebuilder_setStageAlign(this.__wbg_ptr,e,j)}setUpgradeToHttps(A){c.ruffleinstancebuilder_setUpgradeToHttps(this.__wbg_ptr,A)}setVolume(A){c.ruffleinstancebuilder_setVolume(this.__wbg_ptr,A)}setWmode(A){var e=p(A)?0:G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),j=g;c.ruffleinstancebuilder_setWmode(this.__wbg_ptr,e,j)}};Symbol.dispose&&(me.prototype[Symbol.dispose]=me.prototype.free);ge=class{__destroy_into_raw(){let A=this.__wbg_ptr;return this.__wbg_ptr=0,qr.unregister(this),A}free(){let A=this.__destroy_into_raw();c.__wbg_zipwriter_free(A,0)}addFile(A,e){let j=G(A,c.__wbindgen_malloc,c.__wbindgen_realloc),a=g,t=zj(e,c.__wbindgen_malloc),o=g;c.zipwriter_addFile(this.__wbg_ptr,j,a,t,o)}constructor(){let A=c.zipwriter_new();return this.__wbg_ptr=A,qr.register(this,this.__wbg_ptr,this),this}save(){let A=c.zipwriter_save(this.__wbg_ptr);if(A[3])throw xA(A[2]);var e=jA(A[0],A[1]).slice();return c.__wbindgen_free(A[0],A[1]*1,1),e}};Symbol.dispose&&(ge.prototype[Symbol.dispose]=ge.prototype.free);rt=typeof AudioContext<"u"?AudioContext:typeof webkitAudioContext<"u"?webkitAudioContext:void 0;Gt=["blob","arraybuffer"],pr=["nonzero","evenodd"],vt=["unconfigured","configured","closed"],kt=["key","delta"],Rj=["clamp-to-edge","repeat","mirror-repeat"],Ft=["auto"],Er=["zero","one","src","one-minus-src","src-alpha","one-minus-src-alpha","dst","one-minus-dst","dst-alpha","one-minus-dst-alpha","src-alpha-saturated","constant","one-minus-constant","src1","one-minus-src1","src1-alpha","one-minus-src1-alpha"],$t=["add","subtract","reverse-subtract","min","max"],yt=["uniform","storage","read-only-storage"],wt=["opaque","premultiplied"],xt=["standard","extended"],Hj=["never","less","equal","less-equal","greater","not-equal","greater-equal","always"],It=["none","front","back"],mr=["nearest","linear"],Dt=["ccw","cw"],Tj=["uint16","uint32"],Sj=["load","clear"],Ot=["nearest","linear"],Ct=["low-power","high-performance"],Pt=["point-list","line-list","line-strip","triangle-list","triangle-strip"],Rt=["occlusion","timestamp"],Ht=["filtering","non-filtering","comparison"],Kj=["keep","zero","replace","invert","increment-clamp","decrement-clamp","increment-wrap","decrement-wrap"],Tt=["write-only","read-only","read-write"],Mj=["store","discard"],gr=["all","stencil-only","depth-only"],St=["1d","2d","3d"],yA=["r8unorm","r8snorm","r8uint","r8sint","r16unorm","r16snorm","r16uint","r16sint","r16float","rg8unorm","rg8snorm","rg8uint","rg8sint","r32uint","r32sint","r32float","rg16unorm","rg16snorm","rg16uint","rg16sint","rg16float","rgba8unorm","rgba8unorm-srgb","rgba8snorm","rgba8uint","rgba8sint","bgra8unorm","bgra8unorm-srgb","rgb9e5ufloat","rgb10a2uint","rgb10a2unorm","rg11b10ufloat","rg32uint","rg32sint","rg32float","rgba16unorm","rgba16snorm","rgba16uint","rgba16sint","rgba16float","rgba32uint","rgba32sint","rgba32float","stencil8","depth16unorm","depth24plus","depth24plus-stencil8","depth32float","depth32float-stencil8","bc1-rgba-unorm","bc1-rgba-unorm-srgb","bc2-rgba-unorm","bc2-rgba-unorm-srgb","bc3-rgba-unorm","bc3-rgba-unorm-srgb","bc4-r-unorm","bc4-r-snorm","bc5-rg-unorm","bc5-rg-snorm","bc6h-rgb-ufloat","bc6h-rgb-float","bc7-rgba-unorm","bc7-rgba-unorm-srgb","etc2-rgb8unorm","etc2-rgb8unorm-srgb","etc2-rgb8a1unorm","etc2-rgb8a1unorm-srgb","etc2-rgba8unorm","etc2-rgba8unorm-srgb","eac-r11unorm","eac-r11snorm","eac-rg11unorm","eac-rg11snorm","astc-4x4-unorm","astc-4x4-unorm-srgb","astc-5x4-unorm","astc-5x4-unorm-srgb","astc-5x5-unorm","astc-5x5-unorm-srgb","astc-6x5-unorm","astc-6x5-unorm-srgb","astc-6x6-unorm","astc-6x6-unorm-srgb","astc-8x5-unorm","astc-8x5-unorm-srgb","astc-8x6-unorm","astc-8x6-unorm-srgb","astc-8x8-unorm","astc-8x8-unorm-srgb","astc-10x5-unorm","astc-10x5-unorm-srgb","astc-10x6-unorm","astc-10x6-unorm-srgb","astc-10x8-unorm","astc-10x8-unorm-srgb","astc-10x10-unorm","astc-10x10-unorm-srgb","astc-12x10-unorm","astc-12x10-unorm-srgb","astc-12x12-unorm","astc-12x12-unorm-srgb"],Kt=["float","unfilterable-float","depth","sint","uint"],Lj=["1d","2d","2d-array","cube","cube-array","3d"],Mt=["uint8","uint8x2","uint8x4","sint8","sint8x2","sint8x4","unorm8","unorm8x2","unorm8x4","snorm8","snorm8x2","snorm8x4","uint16","uint16x2","uint16x4","sint16","sint16x2","sint16x4","unorm16","unorm16x2","unorm16x4","snorm16","snorm16x2","snorm16x4","float16","float16x2","float16x4","float32","float32x2","float32x3","float32x4","uint32","uint32x2","uint32x3","uint32x4","sint32","sint32x2","sint32x3","sint32x4","unorm10-10-10-2","unorm8x4-bgra"],Lt=["vertex","instance"],Ut=["bytes"],Vt=["omit","same-origin","include"],Xt=["I420","I420P10","I420P12","I420A","I420AP10","I420AP12","I422","I422P10","I422P12","I422A","I422AP10","I422AP12","I444","I444P10","I444P12","I444A","I444AP10","I444AP12","NV12","RGBA","RGBX","BGRA","BGRX"],zt=typeof FinalizationRegistry>"u"?{register:()=>{},unregister:()=>{}}:new FinalizationRegistry(r=>c.__wbg_intounderlyingbytesource_free(r,1)),Wt=typeof FinalizationRegistry>"u"?{register:()=>{},unregister:()=>{}}:new FinalizationRegistry(r=>c.__wbg_intounderlyingsink_free(r,1)),Nt=typeof FinalizationRegistry>"u"?{register:()=>{},unregister:()=>{}}:new FinalizationRegistry(r=>c.__wbg_intounderlyingsource_free(r,1)),Br=typeof FinalizationRegistry>"u"?{register:()=>{},unregister:()=>{}}:new FinalizationRegistry(r=>c.__wbg_rufflehandle_free(r,1)),hr=typeof FinalizationRegistry>"u"?{register:()=>{},unregister:()=>{}}:new FinalizationRegistry(r=>c.__wbg_ruffleinstancebuilder_free(r,1)),qr=typeof FinalizationRegistry>"u"?{register:()=>{},unregister:()=>{}}:new FinalizationRegistry(r=>c.__wbg_zipwriter_free(r,1));Aj=typeof FinalizationRegistry>"u"?{register:()=>{},unregister:()=>{}}:new FinalizationRegistry(r=>c.__wbindgen_destroy_closure(r.a,r.b));wA=null;oe=null;ce=null;fe=null;ne=null;se=null;le=null;de=null;ie=null;be=null;Ze=new TextDecoder("utf-8",{ignoreBOM:!0,fatal:!0});Ze.decode();so=2146435072,Uj=0;ue=new TextEncoder;"encodeInto"in ue||(ue.encodeInto=function(r,A){let e=ue.encode(r);return A.set(e),{read:r.length,written:e.length}});g=0});l();l();var lj={};ca(lj,{PublicAPI:()=>RA,installRuffle:()=>Co});l();l();l();var sA=class r{constructor(A,e,j,a,t){this.major=A,this.minor=e,this.patch=j,this.prIdent=a,this.buildIdent=t}static fromSemver(A){let e=A.split("+"),j=e[0].split("-"),a=j[0].split("."),t=parseInt(a[0],10),o=0,f=0,s=null,i=null;return a[1]!==void 0&&(o=parseInt(a[1],10)),a[2]!==void 0&&(f=parseInt(a[2],10)),j[1]!==void 0&&(s=j[1].split(".")),e[1]!==void 0&&(i=e[1].split(".")),new r(t,o,f,s,i)}isCompatibleWith(A){return this.major!==0&&this.major===A.major||this.major===0&&A.major===0&&this.minor!==0&&this.minor===A.minor||this.major===0&&A.major===0&&this.minor===0&&A.minor===0&&this.patch!==0&&this.patch===A.patch}hasPrecedenceOver(A){if(this.major>A.major)return!0;if(this.major<A.major)return!1;if(this.minor>A.minor)return!0;if(this.minor<A.minor)return!1;if(this.patch>A.patch)return!0;if(this.patch<A.patch)return!1;if(this.prIdent===null&&A.prIdent!==null)return!0;if(this.prIdent!==null&&A.prIdent===null)return!1;if(this.prIdent!==null&&A.prIdent!==null){let e=/^[0-9]*$/;for(let j=0;j<this.prIdent.length&&j<A.prIdent.length;j+=1){let a=e.test(A.prIdent[j]),t=e.test(this.prIdent[j]);if(!t&&a)return!0;if(t&&a){let o=parseInt(this.prIdent[j],10),f=parseInt(A.prIdent[j],10);if(o>f)return!0;if(o<f)return!1}else{if(t&&!a)return!1;if(!t&&!a){if(this.prIdent[j]>A.prIdent[j])return!0;if(this.prIdent[j]<A.prIdent[j])return!1}}}if(this.prIdent.length>A.prIdent.length)return!0;if(this.prIdent.length<A.prIdent.length)return!1}if(this.buildIdent!==null&&A.buildIdent===null)return!0;if(this.buildIdent===null&&A.buildIdent!==null)return!1;if(this.buildIdent!==null&&A.buildIdent!==null){let e=/^[0-9]*$/;for(let j=0;j<this.buildIdent.length&&j<A.buildIdent.length;j+=1){let a=e.test(this.buildIdent[j]),t=e.test(A.buildIdent[j]);if(!a&&t)return!0;if(a&&t){let o=parseInt(this.buildIdent[j],10),f=parseInt(A.buildIdent[j],10);if(o>f)return!0;if(o<f)return!1}else{if(a&&!t)return!1;if(!a&&!t){if(this.buildIdent[j]>A.buildIdent[j])return!0;if(this.buildIdent[j]<A.buildIdent[j])return!1}}}return this.buildIdent.length>A.buildIdent.length}return!1}isEqual(A){return this.major===A.major&&this.minor===A.minor&&this.patch===A.patch}isStableOrCompatiblePrerelease(A){return A.prIdent===null?!0:this.major===A.major&&this.minor===A.minor&&this.patch===A.patch}};l();var Oe=class r{constructor(A){this.requirements=A}satisfiedBy(A){for(let e of this.requirements){let j=!0;for(let{comparator:a,version:t}of e)j=j&&t.isStableOrCompatiblePrerelease(A),a===""||a==="="?j=j&&t.isEqual(A):a===">"?j=j&&A.hasPrecedenceOver(t):a===">="?j=j&&(A.hasPrecedenceOver(t)||t.isEqual(A)):a==="<"?j=j&&t.hasPrecedenceOver(A):a==="<="?j=j&&(t.hasPrecedenceOver(A)||t.isEqual(A)):a==="^"&&(j=j&&t.isCompatibleWith(A));if(j)return!0}return!1}static fromRequirementString(A){let e=A.split(" "),j=[],a=[];for(let t of e)if(t==="||")j.length>0&&(a.push(j),j=[]);else if(t.length>0){let o=/[0-9]/.exec(t);if(o){let f=t.slice(0,o.index).trim(),s=sA.fromSemver(t.slice(o.index).trim());j.push({comparator:f,version:s})}}return j.length>0&&a.push(j),new r(a)}};var RA=class{constructor(A){this.sources=A?.sources||{},this.config=A?.config||{},this.invoked=A?.invoked||!1,this.newestName=A?.newestName||null,A?.superseded?.(),document.readyState==="loading"?document.addEventListener("readystatechange",this.init.bind(this)):window.setTimeout(this.init.bind(this),0)}get version(){return"0.1.0"}newestSourceName(){let A=null,e=sA.fromSemver("0.0.0");for(let j in this.sources)if(Object.prototype.hasOwnProperty.call(this.sources,j)){let a=sA.fromSemver(this.sources[j].version);a.hasPrecedenceOver(e)&&(A=j,e=a)}return A}init(){if(!this.invoked){if(this.invoked=!0,this.newestName=this.newestSourceName(),this.newestName===null)throw new Error("No registered Ruffle source!");("polyfills"in this.config?this.config.polyfills:!0)!==!1&&this.sources[this.newestName].polyfill()}}newest(){let A=this.newestSourceName();return A!==null?this.sources[A]:null}satisfying(A){let e=Oe.fromRequirementString(A),j=null;for(let a in this.sources)if(Object.prototype.hasOwnProperty.call(this.sources,a)){let t=sA.fromSemver(this.sources[a].version);e.satisfiedBy(t)&&(j=this.sources[a])}return j}localCompatible(){return this.sources.local!==void 0?this.satisfying("^"+this.sources.local.version):this.newest()}local(){return this.sources.local!==void 0?this.satisfying("="+this.sources.local.version):this.newest()}superseded(){this.invoked=!0}};l();l();l();var U={versionNumber:"0.7.1",versionName:"local 0.7.1",versionChannel:"local",buildDate:"2026-10-25T10:03:38.983Z",commitHash:"0e90c07cc81c5ca0ca5a52499518500024baad86"};l();l();l();l();var lA;(function(r){r[r.HaveNothing=0]="HaveNothing",r[r.Loading=1]="Loading",r[r.Loaded=2]="Loaded"})(lA||(lA={}));l();var D=X(M(),1);l();l();var GA;(function(r){r.On="on",r.Off="off",r.Auto="auto"})(GA||(GA={}));var Ce;(function(r){r.Off="off",r.Fullscreen="fullscreen",r.On="on"})(Ce||(Ce={}));var TA;(function(r){r.Visible="visible",r.Hidden="hidden"})(TA||(TA={}));var Pe;(function(r){r.Error="error",r.Warn="warn",r.Info="info",r.Debug="debug",r.Trace="trace"})(Pe||(Pe={}));var SA;(function(r){r.Window="window",r.Opaque="opaque",r.Transparent="transparent",r.Direct="direct",r.Gpu="gpu"})(SA||(SA={}));var je;(function(r){r.WebGpu="webgpu",r.WgpuWebgl="wgpu-webgl",r.Webgl="webgl",r.Canvas="canvas"})(je||(je={}));var dA;(function(r){r.On="on",r.RightClickOnly="rightClickOnly",r.Off="off"})(dA||(dA={}));var Re;(function(r){r.AIR="air",r.FlashPlayer="flashPlayer"})(Re||(Re={}));var He;(function(r){r.Allow="allow",r.Confirm="confirm",r.Deny="deny"})(He||(He={}));var Te;(function(r){r.All="all",r.Internal="internal",r.None="none"})(Te||(Te={}));var Se;(function(r){r.Always="always",r.Never="never",r.Smart="smart"})(Se||(Se={}));var Ke;(function(r){r.Embedded="embedded",r.Canvas="canvas"})(Ke||(Ke={}));var iA;(function(r){r.None="none",r.MainThread="mainThread"})(iA||(iA={}));var Ga;(function(r){r.South="south",r.East="east",r.North="north",r.West="west",r.LeftTrigger="left-trigger",r.LeftTrigger2="left-trigger-2",r.RightTrigger="right-trigger",r.RightTrigger2="right-trigger-2",r.Select="select",r.Start="start",r.DPadUp="dpad-up",r.DPadDown="dpad-down",r.DPadLeft="dpad-left",r.DPadRight="dpad-right"})(Ga||(Ga={}));var va={allowScriptAccess:!1,parameters:{},autoplay:GA.Auto,backgroundColor:null,letterbox:Ce.Fullscreen,unmuteOverlay:TA.Visible,upgradeToHttps:!0,compatibilityRules:!0,favorFlash:!0,warnOnUnsupportedContent:!0,logLevel:Pe.Error,showSwfDownload:!1,contextMenu:dA.On,preloader:!0,splashScreen:!0,maxExecutionDuration:15,base:null,menu:!0,allowFullscreen:!1,salign:"",fullScreenAspectRatio:"",forceAlign:!1,quality:null,scale:"showAll",forceScale:!1,frameRate:null,wmode:SA.Window,publicPath:null,polyfills:!0,playerVersion:null,preferredRenderer:null,openUrlMode:He.Allow,allowNetworking:Te.All,openInNewTab:null,socketProxy:[],fontSources:[],defaultFonts:{},credentialAllowList:[],playerRuntime:Re.FlashPlayer,gamepadButtonMapping:{},urlRewriteRules:[],scrollingBehavior:Se.Smart,deviceFontRenderer:Ke.Embedded,backgroundExecutionMode:iA.MainThread};l();var Z=X(M(),1);l();var ka=X(M(),1),i6=`:host{all:initial;pointer-events:inherit;--ruffle-blue:#37528c;--ruffle-blue-dark:#253559;--ruffle-orange:#ffad33;--modal-background:#fafafa;--modal-foreground-rgb:0, 0, 0;--modal-foreground-filter:none;display:inline-block;font-family:Arial,sans-serif;height:400px;letter-spacing:.4px;position:relative;touch-action:none;-webkit-user-select:none;-moz-user-select:none;user-select:none;width:550px;-webkit-tap-highlight-color:transparent}:host  (:-webkit-full-screen) {display:block;height:100%!important;width:100%!important}.hidden{display:none!important}#container,#message-overlay,#panic,#play-button,#splash-screen,#unmute-overlay,#unmute-overlay .background{inset:0;position:absolute}#container{outline:none;overflow:hidden}#container canvas{height:100%;width:100%}#play-button,#unmute-overlay{cursor:pointer;display:none}#unmute-overlay .background{background:#000;opacity:.7}#play-button .icon,#unmute-overlay .icon{height:50%;left:50%;max-height:384px;max-width:384px;opacity:.8;position:absolute;top:50%;transform:translate(-50%,-50%);width:50%}#play-button:hover .icon,#unmute-overlay:hover .icon{opacity:1}#unmute-overlay-svg{overflow:visible;scale:.8}#panic{align-items:center;background:linear-gradient(180deg,#fd3a40,#fda138);color:#fff;display:flex;flex-flow:column;font-size:15px;gap:8px;justify-content:center;overflow:auto;padding:16px;text-align:center}#panic a{color:#fff;text-underline-offset:2px}#panic-title{font-size:30px;font-weight:700;letter-spacing:-.5px}#panic-body{max-width:480px;opacity:.85;width:100%}#panic-details-modal{align-items:center;background:#0008;box-sizing:border-box;display:flex;inset:0;justify-content:center;padding:8px;position:absolute;z-index:1}#panic-details-content{background-color:var(--modal-background);border-radius:12px;box-shadow:0 2px 6px #0008;box-sizing:border-box;color:rgb(var(--modal-foreground-rgb));height:80%;max-width:720px;overflow:hidden;padding:44px 12px 12px;position:relative;width:100%}#panic-details-content .panic-copy-button{background-image:url('data:image/svg+xml;charset=utf-8,<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 -960 960 960"><path d="M360-240q-33 0-56.5-23.5T280-320v-480q0-33 23.5-56.5T360-880h360q33 0 56.5 23.5T800-800v480q0 33-23.5 56.5T720-240zm0-80h360v-480H360zM200-80q-33 0-56.5-23.5T120-160v-560h80v560h440v80zm160-240v-480z"/></svg>');border-radius:4px;cursor:pointer;filter:var(--modal-foreground-filter);height:16px;opacity:.6;position:absolute;right:40px;top:14px;transition:opacity .15s,background-image;width:16px}:is(#panic-details-content .panic-copy-button):hover{opacity:1}.copied:is(#panic-details-content .panic-copy-button){background-image:url('data:image/svg+xml;charset=utf-8,<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="%2322c55e" viewBox="0 -960 960 960"><path d="M382-240 154-468l57-57 171 171 367-367 57 57z"/></svg>');cursor:default;filter:none;opacity:1;pointer-events:none}#panic-details-content textarea{background:rgb(var(--modal-foreground-rgb),.07);border:none;border-radius:8px;box-sizing:border-box;color:rgb(var(--modal-foreground-rgb));font-family:monospace;font-size:12px;height:100%;outline:none;padding:10px;resize:none;width:100%}#panic-details-content textarea::-webkit-scrollbar{width:6px}#panic-details-content textarea::-webkit-scrollbar-thumb{background:rgb(var(--modal-foreground-rgb),.25);border-radius:3px}#panic-details-content textarea::-webkit-scrollbar-track{background:transparent}#message-overlay{align-items:center;background:var(--ruffle-blue);color:var(--ruffle-orange);display:flex;justify-content:center;opacity:1;overflow:auto;z-index:2}#message-overlay .message{font-size:20px;max-height:100%;max-width:100%;padding:5%;text-align:center}#message-overlay p{margin:.5em 0}#message-overlay .message div{-moz-column-gap:1em;column-gap:1em;display:flex;flex-wrap:wrap;justify-content:center}#message-overlay a,#message-overlay button{background:var(--ruffle-blue);border:2px solid var(--ruffle-orange);border-radius:8px;color:var(--ruffle-orange);cursor:pointer;font-family:inherit;font-size:16px;font-weight:700;margin:8px 0;padding:10px 16px;text-decoration:none;transition:background .15s}#panic ul{display:flex;flex-wrap:wrap;gap:8px;justify-content:center;list-style-type:none;margin:0;padding:0}:is(#panic ul) li a{background:transparent;border:1px solid hsla(0,0%,100%,.7);border-radius:8px;color:#fff;display:inline-block;font-family:inherit;font-size:13px;font-weight:700;padding:8px 16px;text-decoration:none;transition:background .15s}:is(:is(#panic ul) li a):hover{background:hsla(0,0%,100%,.2)}#message-overlay a:hover,#message-overlay button:hover{background:#ffffff4c}#context-menu-overlay,.modal{height:100%;position:absolute;width:100%;z-index:1}#context-menu{background-color:var(--modal-background);border-radius:8px;box-shadow:0 0 16px #0006;color:rgb(var(--modal-foreground-rgb));font-size:14px;list-style:none;margin:0;overflow:hidden;padding:5px 0;position:absolute;text-align:start;white-space:nowrap}#context-menu .menu-item{padding:7px 12px}#context-menu.has-checkmarks .menu-item{padding-inline-start:32px;position:relative}#context-menu.has-checkmarks .menu-item.checked:before{background-image:url('data:image/svg+xml;charset=utf-8,<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 -960 960 960"><path d="M382-240 154-468l57-57 171 171 367-367 57 57z"/></svg>');background-repeat:no-repeat;background-size:contain;content:"";filter:var(--modal-foreground-filter);height:16px;inset-inline-start:8px;position:absolute;top:50%;transform:translateY(-50%);width:16px}#context-menu .menu-item.disabled{color:rgb(var(--modal-foreground-rgb),.5);cursor:default}#context-menu .menu-item:not(.disabled):hover{background-color:rgb(var(--modal-foreground-rgb),.15)}#context-menu .menu-separator hr{border:none;border-bottom:1px solid rgb(var(--modal-foreground-rgb),.2);margin:4px 0}#splash-screen{align-items:center;background:var(--splash-screen-background,var(--preloader-background,var(--ruffle-blue)));display:flex;flex-direction:column;justify-content:center}.loadbar{background:var(--ruffle-blue-dark);height:20%;max-height:10px;max-width:316px;width:100%}.loadbar-inner{background:var(--ruffle-orange);height:100%;max-width:100%;width:0}.logo{display:var(--logo-display,block);max-height:150px;max-width:380px}.loading-animation{aspect-ratio:1;margin-bottom:2%;max-height:28px;max-width:28px;width:10%}.spinner{animation:a 1.5s linear infinite;stroke:var(--ruffle-orange);stroke-dasharray:180;stroke-dashoffset:135;transform-origin:50% 50%}@keyframes a{to{transform:rotate(1turn)}}#virtual-keyboard{height:1px;opacity:0;position:absolute;top:-100px;width:1px}.modal{background-color:#0008}.modal-area{background-color:var(--modal-background);border-radius:12px;box-shadow:0 2px 6px #0008;color:rgb(var(--modal-foreground-rgb));left:50%;padding:8px 12px;position:relative;transform:translateX(-50%);width:-moz-fit-content;width:fit-content}#modal-area{height:300px;width:450px}.close-modal{background-image:url("data:image/svg+xml;charset=utf-8,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20width%3D%2216%22%20height%3D%2216%22%20viewBox%3D%220%20-960%20960%20960%22%3E%3Cpath%20d%3D%22M480-392%20300-212q-18%2018-44%2018t-44-18-18-44%2018-44l180-180-180-180q-18-18-18-44t18-44%2044-18%2044%2018l180%20180%20180-180q18-18%2044-18t44%2018%2018%2044-18%2044L568-480l180%20180q18%2018%2018%2044t-18%2044-44%2018-44-18z%22%2F%3E%3C%2Fsvg%3E");cursor:pointer;filter:var(--modal-foreground-filter);height:16px;width:16px}.modal-button{background-color:rgb(var(--modal-foreground-rgb),.2);border-radius:6px;color:rgb(var(--modal-foreground-rgb));cursor:pointer;display:inline-block;padding:4px 8px;text-decoration:none}:not(#volume-controls)>.close-modal{position:absolute;right:16px;top:14px}.general-save-options{border-bottom:2px solid rgb(var(--modal-foreground-rgb),.3);padding-bottom:8px;text-align:center}#local-saves{border-collapse:collapse;color:inherit;display:block;height:calc(100% - 45px);min-height:30px;overflow-y:auto}#local-saves td{border-bottom:2px solid rgb(var(--modal-foreground-rgb),.15);height:30px}#local-saves td:first-child{width:100%;word-break:break-all}.save-option{cursor:pointer;display:inline-block;filter:var(--modal-foreground-filter);height:24px;opacity:.4;vertical-align:middle;width:24px}#local-saves>tr:hover .save-option{opacity:1}#download-save{background-image:url("data:image/svg+xml;charset=utf-8,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20width%3D%2224%22%20height%3D%2224%22%20viewBox%3D%220%20-960%20960%20960%22%3E%3Cpath%20d%3D%22M480-337q-8%200-15-2.5t-13-8.5L308-492q-12-12-11.5-28t11.5-28q12-12%2028.5-12.5T365-549l75%2075v-286q0-17%2011.5-28.5T480-800t28.5%2011.5T520-760v286l75-75q12-12%2028.5-11.5T652-548q11%2012%2011.5%2028T652-492L508-348q-6%206-13%208.5t-15%202.5M240-160q-33%200-56.5-23.5T160-240v-80q0-17%2011.5-28.5T200-360t28.5%2011.5T240-320v80h480v-80q0-17%2011.5-28.5T760-360t28.5%2011.5T800-320v80q0%2033-23.5%2056.5T720-160z%22%2F%3E%3C%2Fsvg%3E")}#replace-save{background-image:url("data:image/svg+xml;charset=utf-8,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20width%3D%2224%22%20height%3D%2224%22%20viewBox%3D%220%20-1080%20960%201200%22%3E%3Cpath%20d%3D%22M440-367v127q0%2017%2011.5%2028.5T480-200t28.5-11.5T520-240v-127l36%2036q6%206%2013.5%209t15%202.5T599-323t13-9q11-12%2011.5-28T612-388L508-492q-6-6-13-8.5t-15-2.5-15%202.5-13%208.5L348-388q-12%2012-11.5%2028t12.5%2028q12%2011%2028%2011.5t28-11.5zM240-80q-33%200-56.5-23.5T160-160v-640q0-33%2023.5-56.5T240-880h287q16%200%2030.5%206t25.5%2017l194%20194q11%2011%2017%2025.5t6%2030.5v447q0%2033-23.5%2056.5T720-80zm280-560q0%2017%2011.5%2028.5T560-600h160L520-800z%22%2F%3E%3C%2Fsvg%3E")}#delete-save{background-image:url("data:image/svg+xml;charset=utf-8,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20width%3D%2224%22%20height%3D%2224%22%20viewBox%3D%220%20-1020%20960%201080%22%3E%3Cpath%20d%3D%22M280-120q-33%200-56.5-23.5T200-200v-520q-17%200-28.5-11.5T160-760t11.5-28.5T200-800h160q0-17%2011.5-28.5T400-840h160q17%200%2028.5%2011.5T600-800h160q17%200%2028.5%2011.5T800-760t-11.5%2028.5T760-720v520q0%2033-23.5%2056.5T680-120zm120-160q17%200%2028.5-11.5T440-320v-280q0-17-11.5-28.5T400-640t-28.5%2011.5T360-600v280q0%2017%2011.5%2028.5T400-280m160%200q17%200%2028.5-11.5T600-320v-280q0-17-11.5-28.5T560-640t-28.5%2011.5T520-600v280q0%2017%2011.5%2028.5T560-280%22%2F%3E%3C%2Fsvg%3E")}.replace-save{display:none}#video-modal .modal-area{box-sizing:border-box;height:95%;width:95%}#video-holder{box-sizing:border-box;height:100%;padding:36px 4px 6px}#video-holder video{background-color:#000;height:100%;width:100%}#volume-controls{align-items:center;display:flex;gap:6px}#mute-checkbox{display:none}label[for=mute-checkbox]{cursor:pointer;filter:var(--modal-foreground-filter);height:24px;line-height:0;width:24px}#volume-mute{background-image:url("data:image/svg+xml;charset=utf-8,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20width%3D%2224%22%20height%3D%2224%22%20viewBox%3D%220%20-960%20960%20960%22%3E%3Cpath%20d%3D%22m719.13-419.35-71.67%2071.68Q634.78-335%20617.13-335t-30.33-12.67q-12.67-12.68-12.67-30.33t12.67-30.33L658.48-480l-71.68-71.67q-12.67-12.68-12.67-30.33t12.67-30.33Q599.48-625%20617.13-625t30.33%2012.67l71.67%2071.68%2071.67-71.68Q803.48-625%20821.13-625t30.33%2012.67q12.67%2012.68%2012.67%2030.33t-12.67%2030.33L779.78-480l71.68%2071.67q12.67%2012.68%2012.67%2030.33t-12.67%2030.33Q838.78-335%20821.13-335t-30.33-12.67zM278-357.87H161.22q-17.66%200-30.33-12.67-12.67-12.68-12.67-30.33v-158.26q0-17.65%2012.67-30.33%2012.67-12.67%2030.33-12.67H278l130.15-129.91q20.63-20.63%2046.98-9.45%2026.35%2011.19%2026.35%2039.77v443.44q0%2028.58-26.35%2039.77-26.35%2011.18-46.98-9.45z%22%2F%3E%3C%2Fsvg%3E")}#volume-min{background-image:url("data:image/svg+xml;charset=utf-8,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20width%3D%2224%22%20height%3D%2224%22%20viewBox%3D%22161%20-960%20960%20960%22%3E%3Cpath%20d%3D%22M438.65-357.87H321.87q-17.65%200-30.33-12.67-12.67-12.68-12.67-30.33v-158.26q0-17.65%2012.67-30.33%2012.68-12.67%2030.33-12.67h116.78L568.8-732.04q20.63-20.63%2046.98-9.45%2026.35%2011.19%2026.35%2039.77v443.44q0%2028.58-26.35%2039.77-26.35%2011.18-46.98-9.45z%22%2F%3E%3C%2Fsvg%3E")}#volume-mid{background-image:url("data:image/svg+xml;charset=utf-8,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20width%3D%2224%22%20height%3D%2224%22%20viewBox%3D%2280%20-960%20960%20960%22%3E%3Cpath%20d%3D%22M357.98-357.87H241.2q-17.66%200-30.33-12.67-12.67-12.68-12.67-30.33v-158.26q0-17.65%2012.67-30.33%2012.67-12.67%2030.33-12.67h116.78L487.65-731.8q20.63-20.64%2047.1-9.57t26.47%2039.65v443.44q0%2028.58-26.47%2039.65t-47.1-9.57zM741.8-480q0%2042.48-20.47%2080.09-20.48%2037.61-54.94%2060.82-10.22%205.98-20.19.25-9.98-5.73-9.98-17.44v-248.44q0-11.71%209.98-17.32%209.97-5.61%2020.19.37%2034.46%2023.71%2054.94%2061.45Q741.8-522.48%20741.8-480%22%2F%3E%3C%2Fsvg%3E")}#volume-max{background-image:url("data:image/svg+xml;charset=utf-8,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20width%3D%2224%22%20height%3D%2224%22%20viewBox%3D%229%20-960%20960%20960%22%3E%3Cpath%20d%3D%22M754.22-480.5q0-78.52-41.88-143.9t-111.91-98.62q-14.47-6.74-20.47-20.96t-.53-28.93q5.74-15.72%2020.34-22.46t29.58%200q92.48%2042.46%20147.97%20127.05%2055.48%2084.6%2055.48%20187.82t-55.48%20187.82q-55.49%2084.59-147.97%20127.05-14.98%206.74-29.58%200t-20.34-22.46q-5.47-14.71.53-28.93t20.47-20.96q70.03-33.24%20111.91-98.62t41.88-143.9M286.98-357.87H170.2q-17.66%200-30.33-12.67-12.67-12.68-12.67-30.33v-158.26q0-17.65%2012.67-30.33%2012.67-12.67%2030.33-12.67h116.78L416.65-731.8q20.63-20.64%2047.1-9.57t26.47%2039.65v443.44q0%2028.58-26.47%2039.65t-47.1-9.57zM670.8-480q0%2042.48-20.47%2080.09-20.48%2037.61-54.94%2060.82-10.22%205.98-20.19.25-9.98-5.73-9.98-17.44v-248.44q0-11.71%209.98-17.32%209.97-5.61%2020.19.37%2034.46%2023.71%2054.94%2061.45Q670.8-522.48%20670.8-480%22%2F%3E%3C%2Fsvg%3E")}#volume-slider-text{text-align:center;-webkit-user-select:none;-moz-user-select:none;user-select:none;width:4.8ch}#hardware-acceleration-modal .modal-area{box-sizing:border-box;padding:16px 48px;text-align:center;width:95%}#acceleration-text{display:block;margin-bottom:8px}#clipboard-modal h2{margin-right:36px;margin-top:4px}#clipboard-modal p:last-child{margin-bottom:2px}@media(prefers-color-scheme:light){:host{--modal-background:#fafafa;--modal-foreground-rgb:0, 0, 0;--modal-foreground-filter:none}}@media(prefers-color-scheme:dark){:host{--modal-background:#282828;--modal-foreground-rgb:221, 221, 221;--modal-foreground-filter:invert(90%)}}`;function Fa(){return(0,ka.jsx)("style",{children:i6})}l();var $a=X(M(),1);function ya(){return(0,$a.jsx)("style",{id:"dynamic-styles"})}l();var O=X(M(),1);function wa(){return(0,O.jsxs)("div",{id:"container",children:[(0,O.jsx)("div",{id:"play-button",children:(0,O.jsx)("div",{class:"icon",children:(0,O.jsxs)("svg",{xmlns:"http://www.w3.org/2000/svg",preserveAspectRatio:"xMidYMid",viewBox:"0 0 250 250",width:"100%",height:"100%",children:[(0,O.jsxs)("defs",{xmlns:"http://www.w3.org/2000/svg",children:[(0,O.jsxs)("linearGradient",{xmlns:"http://www.w3.org/2000/svg",id:"a",gradientUnits:"userSpaceOnUse",x1:"125",y1:"0",x2:"125",y2:"250",spreadMethod:"pad",children:[(0,O.jsx)("stop",{xmlns:"http://www.w3.org/2000/svg",offset:"0%","stop-color":"#FDA138"}),(0,O.jsx)("stop",{xmlns:"http://www.w3.org/2000/svg",offset:"100%","stop-color":"#FD3A40"})]}),(0,O.jsxs)("g",{xmlns:"http://www.w3.org/2000/svg",id:"b",children:[(0,O.jsx)("path",{xmlns:"http://www.w3.org/2000/svg",fill:"url(#a)",d:"M250 125q0-52-37-88-36-37-88-37T37 37Q0 73 0 125t37 88q36 37 88 37t88-37q37-36 37-88M87 195V55l100 70-100 70z"}),(0,O.jsx)("path",{xmlns:"http://www.w3.org/2000/svg",fill:"#FFF",d:"M87 55v140l100-70L87 55z"})]})]}),(0,O.jsx)("use",{xmlns:"http://www.w3.org/2000/svg",href:"#b"})]})})}),(0,O.jsxs)("div",{id:"unmute-overlay",children:[(0,O.jsx)("div",{class:"background"}),(0,O.jsx)("div",{class:"icon",children:(0,O.jsxs)("svg",{id:"unmute-overlay-svg",xmlns:"http://www.w3.org/2000/svg",preserveAspectRatio:"xMidYMid",viewBox:"0 0 512 584",width:"100%",height:"100%",children:[(0,O.jsx)("path",{xmlns:"http://www.w3.org/2000/svg",fill:"#FFF",stroke:"#FFF",d:"m457.941 256 47.029-47.029c9.372-9.373 9.372-24.568 0-33.941-9.373-9.373-24.568-9.373-33.941 0l-47.029 47.029-47.029-47.029c-9.373-9.373-24.568-9.373-33.941 0-9.372 9.373-9.372 24.568 0 33.941l47.029 47.029-47.029 47.029c-9.372 9.373-9.372 24.568 0 33.941 4.686 4.687 10.827 7.03 16.97 7.03s12.284-2.343 16.971-7.029l47.029-47.03 47.029 47.029c4.687 4.687 10.828 7.03 16.971 7.03s12.284-2.343 16.971-7.029c9.372-9.373 9.372-24.568 0-33.941z"}),(0,O.jsx)("path",{xmlns:"http://www.w3.org/2000/svg",fill:"#FFF",stroke:"#FFF",d:"m99 160h-55c-24.301 0-44 19.699-44 44v104c0 24.301 19.699 44 44 44h55c2.761 0 5-2.239 5-5v-182c0-2.761-2.239-5-5-5z"}),(0,O.jsx)("path",{xmlns:"http://www.w3.org/2000/svg",fill:"#FFF",stroke:"#FFF",d:"m280 56h-24c-5.269 0-10.392 1.734-14.578 4.935l-103.459 79.116c-1.237.946-1.963 2.414-1.963 3.972v223.955c0 1.557.726 3.026 1.963 3.972l103.459 79.115c4.186 3.201 9.309 4.936 14.579 4.936h23.999c13.255 0 24-10.745 24-24v-352.001c0-13.255-10.745-24-24-24z"}),(0,O.jsx)("text",{xmlns:"http://www.w3.org/2000/svg",id:"unmute-text",x:"256",y:"560","text-anchor":"middle","font-size":"60px",fill:"#FFF",stroke:"#FFF","data-i18n-key":"click-to-unmute"})]})})]}),(0,O.jsx)("input",{"aria-hidden":"true",id:"virtual-keyboard",type:"text",autocomplete:"off",autocorrect:"off",autocapitalize:"none"})]})}l();var Y=X(M(),1);function xa(){return(0,Y.jsxs)("div",{id:"splash-screen",class:"hidden",children:[(0,Y.jsx)("svg",{xmlns:"http://www.w3.org/2000/svg",class:"logo",preserveAspectRatio:"xMidYMid",viewBox:"0 0 380 150",children:(0,Y.jsxs)("g",{xmlns:"http://www.w3.org/2000/svg",children:[(0,Y.jsx)("path",{xmlns:"http://www.w3.org/2000/svg",fill:"#966214",d:"M58.75 85.6q.75-.1 1.5-.35.85-.25 1.65-.75.55-.35 1.05-.8.5-.45.95-1 .5-.5.75-1.2-.05.05-.15.1-.1.15-.25.25l-.1.2q-.15.05-.25.1-.4 0-.8.05-.5-.25-.9-.5-.3-.1-.55-.3l-.6-.6-4.25-6.45-1.5 11.25h3.45m83.15-.2h3.45q.75-.1 1.5-.35.25-.05.45-.15.35-.15.65-.3l.5-.3q.25-.15.5-.35.45-.35.9-.75.45-.35.75-.85l.1-.1q.1-.2.2-.35.2-.3.35-.6l-.3.4-.15.15q-.5.15-1.1.1-.25 0-.4-.05-.5-.15-.8-.4-.15-.1-.25-.25-.3-.3-.55-.6l-.05-.05v-.05l-4.25-6.4-1.5 11.25m-21.15-3.95q-.3-.3-.55-.6l-.05-.05v-.05l-4.25-6.4-1.5 11.25h3.45q.75-.1 1.5-.35.85-.25 1.6-.75.75-.5 1.4-1.1.45-.35.75-.85.35-.5.65-1.05l-.45.55q-.5.15-1.1.1-.9 0-1.45-.7m59.15.3q-.75-.5-1.4-1-3.15-2.55-3.5-6.4l-1.5 11.25h21q-3.1-.25-5.7-.75-5.6-1.05-8.9-3.1m94.2 3.85h3.45q.6-.1 1.2-.3.4-.1.75-.2.35-.15.65-.3.7-.35 1.35-.8.75-.55 1.3-1.25.1-.15.25-.3-2.55-.25-3.25-1.8l-4.2-6.3-1.5 11.25m-45.3-4.85q-.5-.4-.9-.8-2.3-2.35-2.6-5.6l-1.5 11.25h21q-11.25-.95-16-4.85m97.7 4.85q-.3-.05-.6-.05-10.8-1-15.4-4.8-3.15-2.55-3.5-6.35l-1.5 11.2h21Z"}),(0,Y.jsx)("path",{xmlns:"http://www.w3.org/2000/svg",fill:"var(--ruffle-orange)",d:"M92.6 54.8q-1.95-1.4-4.5-1.4H60.35q-1.35 0-2.6.45-1.65.55-3.15 1.8-2.75 2.25-3.25 5.25l-1.65 12h.05v.3l5.85 1.15h-9.5q-.5.05-1 .15-.5.15-1 .35-.5.2-.95.45-.5.3-.95.7-.45.35-.85.8-.35.4-.65.85-.3.45-.5.9-.15.45-.3.95l-5.85 41.6H50.3l5-35.5 1.5-11.25 4.25 6.45.6.6q.25.2.55.3.4.25.9.5.4-.05.8-.05.1-.05.25-.1l.1-.2q.15-.1.25-.25.1-.05.15-.1l.3-1.05 1.75-12.3h11.15L75.8 82.6h16.5l2.3-16.25h-.05l.8-5.7q.4-2.45-1-4.2-.35-.4-.75-.8-.25-.25-.55-.5-.2-.2-.45-.35m16.2 18.1h.05l-.05.3 5.85 1.15H105.2q-.5.05-1 .15-.5.15-1 .35-.5.2-.95.45-.5.3-1 .65-.4.4-.8.85-.25.3-.55.65-.05.1-.15.2-.25.45-.4.9-.2.45-.3.95-.1.65-.2 1.25-.2 1.15-.4 2.25l-4.3 30.6q-.25 3 1.75 5.25 1.6 1.8 4 2.15.6.1 1.25.1h27.35q3.25 0 6-2.25.35-.35.7-.55l.3-.2q2-2 2.25-4.5l1.65-11.6q.05-.05.1-.05l1.65-11.35h.05l.7-5.2 1.5-11.25 4.25 6.4v.05l.05.05q.25.3.55.6.1.15.25.25.3.25.8.4.15.05.4.05.6.05 1.1-.1l.15-.15.3-.4.3-1.05 1.3-9.05h-.05l.7-5.05h-.05l.15-1.25h-.05l1.65-11.7h-16.25l-2.65 19.5h.05v.2l-.05.1h.05l5.8 1.15H132.7q-.5.05-1 .15-.5.15-1 .35-.15.05-.3.15-.3.1-.55.25-.05 0-.1.05-.5.3-1 .65-.4.35-.7.7-.55.7-.95 1.45-.35.65-.55 1.4-.15.7-.25 1.4v.05q-.15 1.05-.35 2.05l-1.2 8.75v.1l-2.1 14.7H111.4l2.25-15.55h.05l.7-5.2 1.5-11.25 4.25 6.4v.05l.05.05q.25.3.55.6.55.7 1.45.7.6.05 1.1-.1l.45-.55.3-1.05 1.3-9.05h-.05l.7-5.05h-.05l.15-1.25h-.05l1.65-11.7h-16.25l-2.65 19.5m106.5-41.75q-2.25-2.25-5.5-2.25h-27.75q-3 0-5.75 2.25-1.3.95-2.05 2.1-.45.6-.7 1.2-.2.5-.35 1-.1.45-.15.95l-4.15 29.95h-.05l-.7 5.2h-.05l-.2 1.35h.05l-.05.3 5.85 1.15h-9.45q-2.1.05-3.95 1.6-1.9 1.55-2.25 3.55l-.5 3.5h-.05l-5.3 38.1h16.25l5-35.5 1.5-11.25q.35 3.85 3.5 6.4.65.5 1.4 1 3.3 2.05 8.9 3.1 2.6.5 5.7.75l1.75-11.25h-12.2l.4-2.95h-.05l.7-5.05h-.05q.1-.9.3-1.9.1-.75.2-1.6.85-5.9 2.15-14.9 0-.15.05-.25l.1-.9q.2-1.55.45-3.15h11.25l-3.1 20.8h16.5l4.1-28.05q.15-1.7-.4-3.15-.5-1.1-1.35-2.1m46.65 44.15q-.5.3-1 .65-.4.4-.8.85-.35.4-.7.85-.25.45-.45.9-.15.45-.3.95l-5.85 41.6h16.25l5-35.5 1.5-11.25 4.2 6.3q.7 1.55 3.25 1.8l.05-.1q.25-.4.35-.85l.3-1.05 1.8-14.05v-.05l5.35-37.45h-16.25l-6.15 44.3 5.85 1.15h-9.45q-.5.05-1 .15-.5.15-1 .35-.5.2-.95.45m5.4-38.9q.15-1.7-.4-3.15-.5-1.1-1.35-2.1-2.25-2.25-5.5-2.25h-27.75q-2.3 0-4.45 1.35-.65.35-1.3.9-1.3.95-2.05 2.1-.45.6-.7 1.2-.4.9-.5 1.95l-4.15 29.95h-.05l-.7 5.2h-.05l-.2 1.35h.05l-.05.3 5.85 1.15h-9.45q-2.1.05-3.95 1.6-1.9 1.55-2.25 3.55l-.5 3.5h-.05l-1.2 8.75v.1l-4.1 29.25h16.25l5-35.5 1.5-11.25q.3 3.25 2.6 5.6.4.4.9.8 4.75 3.9 16 4.85l1.75-11.25h-12.2l.4-2.95h-.05l.7-5.05h-.05q.15-.9.3-1.9.1-.75.25-1.6.15-1.25.35-2.65v-.05q.95-6.7 2.35-16.5h11.25l-3.1 20.8h16.5l4.1-28.05M345 66.35h-.05l1.15-8.2q.5-3-1.75-5.25-1.25-1.25-3-1.75-1-.5-2.25-.5h-27.95q-.65 0-1.3.1-2.5.35-4.7 2.15-2.75 2.25-3.25 5.25l-1.95 14.7v.05l-.05.3 5.85 1.15h-9.45q-1.9.05-3.6 1.35-.2.1-.35.25-1.9 1.55-2.25 3.55l-4.85 34.1q-.25 3 1.75 5.25 1.25 1.4 3 1.95 1.05.3 2.25.3H320q3.25 0 6-2.25 2.75-2 3.25-5l2.75-18.5h-16.5l-1.75 11H302.5l2.1-14.75h.05l.85-6 1.5-11.2q.35 3.8 3.5 6.35 4.6 3.8 15.4 4.8.3 0 .6.05h15.75L345 66.35m-16.4-.95-1.25 8.95h-11.3l.4-2.95h-.05l.7-5.05h-.1l.15-.95h11.45Z"})]})}),(0,Y.jsx)("svg",{xmlns:"http://www.w3.org/2000/svg",class:"loading-animation",viewBox:"0 0 66 66",children:(0,Y.jsx)("circle",{xmlns:"http://www.w3.org/2000/svg",class:"spinner",fill:"none","stroke-width":"6","stroke-linecap":"round",cx:"33",cy:"33",r:"30"})}),(0,Y.jsx)("div",{class:"loadbar",children:(0,Y.jsx)("div",{class:"loadbar-inner"})})]})}l();var bA=X(M(),1);function Ia(){return(0,bA.jsx)("div",{id:"save-manager",class:"modal hidden",children:(0,bA.jsxs)("div",{id:"modal-area",class:"modal-area",children:[(0,bA.jsx)("span",{class:"close-modal"}),(0,bA.jsx)("div",{class:"general-save-options",children:(0,bA.jsx)("span",{class:"modal-button","data-i18n-key":"save-backup-all"})}),(0,bA.jsx)("table",{id:"local-saves"})]})})}l();var W=X(M(),1);function Da(){return(0,W.jsx)("div",{id:"volume-controls-modal",class:"modal hidden",children:(0,W.jsx)("div",{class:"modal-area",children:(0,W.jsxs)("div",{id:"volume-controls",children:[(0,W.jsx)("input",{id:"mute-checkbox",type:"checkbox"}),(0,W.jsx)("label",{id:"volume-mute",for:"mute-checkbox","data-i18n-title-key":"volume-controls-unmute"}),(0,W.jsx)("label",{id:"volume-min",for:"mute-checkbox","data-i18n-title-key":"volume-controls-mute"}),(0,W.jsx)("label",{id:"volume-mid",for:"mute-checkbox","data-i18n-title-key":"volume-controls-mute"}),(0,W.jsx)("label",{id:"volume-max",for:"mute-checkbox","data-i18n-title-key":"volume-controls-mute"}),(0,W.jsx)("input",{id:"volume-slider",type:"range",min:"0",max:"100",step:"1"}),(0,W.jsx)("span",{id:"volume-slider-text"}),(0,W.jsx)("span",{class:"close-modal"})]})})})}l();var KA=X(M(),1);function Oa(){return(0,KA.jsx)("div",{id:"video-modal",class:"modal hidden",children:(0,KA.jsxs)("div",{class:"modal-area",children:[(0,KA.jsx)("span",{class:"close-modal"}),(0,KA.jsx)("div",{id:"video-holder"})]})})}l();var vA=X(M(),1);function Ca(){return(0,vA.jsx)("div",{id:"hardware-acceleration-modal",class:"modal hidden",children:(0,vA.jsxs)("div",{class:"modal-area",children:[(0,vA.jsx)("span",{class:"close-modal"}),(0,vA.jsx)("span",{id:"acceleration-text","data-i18n-key":"enable-hardware-acceleration"}),(0,vA.jsx)("a",{href:"https://github.com/ruffle-rs/ruffle/wiki/Frequently-Asked-Questions-For-Users#chrome-hardware-acceleration",target:"_blank",class:"modal-button","data-i18n-key":"enable-hardware-acceleration-link"})]})})}l();var L=X(M(),1),hj=navigator.userAgent.includes("Mac OS X")?"Command":"Ctrl";function Pa(){return(0,L.jsx)("div",{id:"clipboard-modal",class:"modal hidden",children:(0,L.jsxs)("div",{class:"modal-area",children:[(0,L.jsx)("span",{class:"close-modal"}),(0,L.jsx)("h2",{"data-i18n-key":"clipboard-message-title"}),(0,L.jsx)("p",{id:"clipboard-modal-description"}),(0,L.jsxs)("p",{children:[(0,L.jsxs)("b",{children:[hj,"+C"]}),(0,L.jsx)("span",{"data-i18n-key":"clipboard-message-copy"})]}),(0,L.jsxs)("p",{children:[(0,L.jsxs)("b",{children:[hj,"+X"]}),(0,L.jsx)("span",{"data-i18n-key":"clipboard-message-cut"})]}),(0,L.jsxs)("p",{children:[(0,L.jsxs)("b",{children:[hj,"+V"]}),(0,L.jsx)("span",{"data-i18n-key":"clipboard-message-paste"})]})]})})}l();var qj=X(M(),1);function Ra(){return(0,qj.jsx)("div",{id:"context-menu-overlay",class:"hidden",children:(0,qj.jsx)("ul",{id:"context-menu"})})}var J=document.createElement("template");J.content.appendChild((0,Z.jsx)(Fa,{}));J.content.appendChild((0,Z.jsx)(ya,{}));J.content.appendChild((0,Z.jsx)(wa,{}));J.content.appendChild((0,Z.jsx)(xa,{}));J.content.appendChild((0,Z.jsx)(Ia,{}));J.content.appendChild((0,Z.jsx)(Da,{}));J.content.appendChild((0,Z.jsx)(Oa,{}));J.content.appendChild((0,Z.jsx)(Ca,{}));J.content.appendChild((0,Z.jsx)(Pa,{}));J.content.appendChild((0,Z.jsx)(Ra,{}));l();l();l();l();l();var rA=class{constructor(A){this.value=A}valueOf(){return this.value}},w=class extends rA{constructor(A="???"){super(A)}toString(A){return`{${this.value}}`}},z=class extends rA{constructor(A,e={}){super(A),this.opts=e}toString(A){if(A)try{return A.memoizeIntlObject(Intl.NumberFormat,this.opts).format(this.value)}catch(e){A.reportError(e)}return this.value.toString(10)}},tA=class r extends rA{static supportsValue(A){if(typeof A=="number"||A instanceof Date)return!0;if(A instanceof rA)return r.supportsValue(A.valueOf());if("Temporal"in globalThis){let e=globalThis.Temporal;if(A instanceof e.Instant||A instanceof e.PlainDateTime||A instanceof e.PlainDate||A instanceof e.PlainMonthDay||A instanceof e.PlainTime||A instanceof e.PlainYearMonth)return!0}return!1}constructor(A,e={}){A instanceof r?(e={...A.opts,...e},A=A.value):A instanceof rA&&(A=A.valueOf()),typeof A=="object"&&"calendarId"in A&&e.calendar===void 0&&(e={...e,calendar:A.calendarId}),super(A),this.opts=e}[Symbol.toPrimitive](A){return A==="string"?this.toString():this.toNumber()}toNumber(){let A=this.value;if(typeof A=="number")return A;if(A instanceof Date)return A.getTime();if("epochMilliseconds"in A)return A.epochMilliseconds;if("toZonedDateTime"in A)return A.toZonedDateTime("UTC").epochMilliseconds;throw new TypeError("Unwrapping a non-number value as a number")}toString(A){if(A)try{return A.memoizeIntlObject(Intl.DateTimeFormat,this.opts).format(this.value)}catch(e){A.reportError(e)}return typeof this.value=="number"||this.value instanceof Date?new Date(this.value).toISOString():this.value.toString()}};var Ha=100,b6="\u2068",u6="\u2069";function _6(r,A,e){if(e===A||e instanceof z&&A instanceof z&&e.value===A.value)return!0;if(A instanceof z&&typeof e=="string"){let j=r.memoizeIntlObject(Intl.PluralRules,A.opts).select(A.value);if(e===j)return!0}return!1}function Ta(r,A,e){return A[e]?MA(r,A[e].value):(r.reportError(new RangeError("No default")),new w)}function Gj(r,A){let e=[],j=Object.create(null);for(let a of A)a.type==="narg"?j[a.name]=ae(r,a.value):e.push(ae(r,a));return{positional:e,named:j}}function ae(r,A){switch(A.type){case"str":return A.value;case"num":return new z(A.value,{minimumFractionDigits:A.precision});case"var":return p6(r,A);case"mesg":return E6(r,A);case"term":return m6(r,A);case"func":return g6(r,A);case"select":return B6(r,A);default:return new w}}function p6(r,{name:A}){let e;if(r.params)if(Object.prototype.hasOwnProperty.call(r.params,A))e=r.params[A];else return new w(`$${A}`);else if(r.args&&Object.prototype.hasOwnProperty.call(r.args,A))e=r.args[A];else return r.reportError(new ReferenceError(`Unknown variable: $${A}`)),new w(`$${A}`);if(e instanceof rA)return e;switch(typeof e){case"string":return e;case"number":return new z(e);case"object":if(tA.supportsValue(e))return new tA(e);default:return r.reportError(new TypeError(`Variable type not supported: $${A}, ${typeof e}`)),new w(`$${A}`)}}function E6(r,{name:A,attr:e}){let j=r.bundle._messages.get(A);if(!j)return r.reportError(new ReferenceError(`Unknown message: ${A}`)),new w(A);if(e){let a=j.attributes[e];return a?MA(r,a):(r.reportError(new ReferenceError(`Unknown attribute: ${e}`)),new w(`${A}.${e}`))}return j.value?MA(r,j.value):(r.reportError(new ReferenceError(`No value: ${A}`)),new w(A))}function m6(r,{name:A,attr:e,args:j}){let a=`-${A}`,t=r.bundle._terms.get(a);if(!t)return r.reportError(new ReferenceError(`Unknown term: ${a}`)),new w(a);if(e){let f=t.attributes[e];if(f){r.params=Gj(r,j).named;let s=MA(r,f);return r.params=null,s}return r.reportError(new ReferenceError(`Unknown attribute: ${e}`)),new w(`${a}.${e}`)}r.params=Gj(r,j).named;let o=MA(r,t.value);return r.params=null,o}function g6(r,{name:A,args:e}){let j=r.bundle._functions[A];if(!j)return r.reportError(new ReferenceError(`Unknown function: ${A}()`)),new w(`${A}()`);if(typeof j!="function")return r.reportError(new TypeError(`Function ${A}() is not callable`)),new w(`${A}()`);try{let a=Gj(r,e);return j(a.positional,a.named)}catch(a){return r.reportError(a),new w(`${A}()`)}}function B6(r,{selector:A,variants:e,star:j}){let a=ae(r,A);if(a instanceof w)return Ta(r,e,j);for(let t of e){let o=ae(r,t.key);if(_6(r,a,o))return MA(r,t.value)}return Ta(r,e,j)}function vj(r,A){if(r.dirty.has(A))return r.reportError(new RangeError("Cyclic reference")),new w;r.dirty.add(A);let e=[],j=r.bundle._useIsolating&&A.length>1;for(let a of A){if(typeof a=="string"){e.push(r.bundle._transform(a));continue}if(r.placeables++,r.placeables>Ha)throw r.dirty.delete(A),new RangeError(`Too many placeables expanded: ${r.placeables}, max allowed is ${Ha}`);j&&e.push(b6),e.push(ae(r,a).toString(r)),j&&e.push(u6)}return r.dirty.delete(A),e.join("")}function MA(r,A){return typeof A=="string"?r.bundle._transform(A):vj(r,A)}l();var Me=class{constructor(A,e,j){this.dirty=new WeakSet,this.params=null,this.placeables=0,this.bundle=A,this.errors=e,this.args=j}reportError(A){if(!this.errors||!(A instanceof Error))throw A;this.errors.push(A)}memoizeIntlObject(A,e){let j=this.bundle._intls.get(A);j||(j={},this.bundle._intls.set(A,j));let a=JSON.stringify(e);return j[a]||(j[a]=new A(this.bundle.locales,e)),j[a]}};l();function kj(r,A){let e=Object.create(null);for(let[j,a]of Object.entries(r))A.includes(j)&&(e[j]=a.valueOf());return e}var Sa=["unitDisplay","currencyDisplay","useGrouping","minimumIntegerDigits","minimumFractionDigits","maximumFractionDigits","minimumSignificantDigits","maximumSignificantDigits"];function Ka(r,A){let e=r[0];if(e instanceof w)return new w(`NUMBER(${e.valueOf()})`);if(e instanceof z)return new z(e.valueOf(),{...e.opts,...kj(A,Sa)});if(e instanceof tA)return new z(e.toNumber(),{...kj(A,Sa)});throw new TypeError("Invalid argument to NUMBER")}var h6=["dateStyle","timeStyle","fractionalSecondDigits","dayPeriod","hour12","weekday","era","year","month","day","hour","minute","second","timeZoneName"];function Ma(r,A){let e=r[0];if(e instanceof w)return new w(`DATETIME(${e.valueOf()})`);if(e instanceof tA||e instanceof z)return new tA(e,kj(A,h6));throw new TypeError("Invalid argument to DATETIME")}l();var La=new _;function Ua(r){let A=Array.isArray(r)?r.join(" "):r,e=La.get(A);return e===void 0&&(e=new _,La.set(A,e)),e}var re=class{constructor(A,{functions:e,useIsolating:j=!0,transform:a=t=>t}={}){this._terms=new _,this._messages=new _,this.locales=Array.isArray(A)?A:[A],this._functions={NUMBER:Ka,DATETIME:Ma,...e},this._useIsolating=j,this._transform=a,this._intls=Ua(A)}hasMessage(A){return this._messages.has(A)}getMessage(A){return this._messages.get(A)}addResource(A,{allowOverrides:e=!1}={}){let j=[];for(let a=0;a<A.body.length;a++){let t=A.body[a];if(t.id.startsWith("-")){if(e===!1&&this._terms.has(t.id)){j.push(new Error(`Attempt to override an existing term: "${t.id}"`));continue}this._terms.set(t.id,t)}else{if(e===!1&&this._messages.has(t.id)){j.push(new Error(`Attempt to override an existing message: "${t.id}"`));continue}this._messages.set(t.id,t)}}return j}formatPattern(A,e=null,j=null){if(typeof A=="string")return this._transform(A);let a=new Me(this,j,e);try{return vj(a,A).toString(a)}catch(t){if(a.errors&&t instanceof Error)return a.errors.push(t),new w().toString(a);throw t}}};l();var Fj=/^(-?[a-zA-Z][\w-]*) *= */gm,Va=/\.([a-zA-Z][\w-]*) *= */y,q6=/\*?\[/y,$j=/(-?[0-9]+(?:\.([0-9]+))?)/y,G6=/([a-zA-Z][\w-]*)/y,Xa=/([$-])?([a-zA-Z][\w-]*)(?:\.([a-zA-Z][\w-]*))?/y,v6=/^[A-Z][A-Z0-9_-]*$/,Le=/([^{}\n\r]+)/y,k6=/([^\\"\n\r]*)/y,za=/\\([\\"])/y,Wa=/\\u([a-fA-F0-9]{4})|\\U([a-fA-F0-9]{6})/y,F6=/^\n+/,Na=/ +$/,$6=/ *\r?\n/g,y6=/( *)$/,w6=/{\s*/y,Ja=/\s*}/y,x6=/\[\s*/y,I6=/\s*] */y,D6=/\s*\(\s*/y,O6=/\s*->\s*/y,C6=/\s*:\s*/y,P6=/\s*,?\s*/y,R6=/\s+/y,te=class{constructor(A){this.body=[],Fj.lastIndex=0;let e=0;for(;;){let u=Fj.exec(A);if(u===null)break;e=Fj.lastIndex;try{this.body.push(s(u[1]))}catch(q){if(q instanceof SyntaxError)continue;throw q}}function j(u){return u.lastIndex=e,u.test(A)}function a(u,q){if(A[e]===u)return e++,!0;if(q)throw new q(`Expected ${u}`);return!1}function t(u,q){if(j(u))return e=u.lastIndex,!0;if(q)throw new q(`Expected ${u.toString()}`);return!1}function o(u){u.lastIndex=e;let q=u.exec(A);if(q===null)throw new SyntaxError(`Expected ${u.toString()}`);return e=u.lastIndex,q}function f(u){return o(u)[1]}function s(u){let q=b(),I=i();if(q===null&&Object.keys(I).length===0)throw new SyntaxError("Expected message value or attributes");return{id:u,value:q,attributes:I}}function i(){let u=Object.create(null);for(;j(Va);){let q=f(Va),I=b();if(I===null)throw new SyntaxError("Expected attribute value");u[q]=I}return u}function b(){let u;if(j(Le)&&(u=f(Le)),A[e]==="{"||A[e]==="}")return E(u?[u]:[],1/0);let q=CA();return q?u?E([u,q],q.length):(q.value=hA(q.value,F6),E([q],q.length)):u?hA(u,Na):null}function E(u=[],q){for(;;){if(j(Le)){u.push(f(Le));continue}if(A[e]==="{"){u.push(h());continue}if(A[e]==="}")throw new SyntaxError("Unbalanced closing brace");let aA=CA();if(aA){u.push(aA),q=Math.min(q,aA.length);continue}break}let I=u.length-1,qA=u[I];typeof qA=="string"&&(u[I]=hA(qA,Na));let ZA=[];for(let aA of u)aA instanceof Ue&&(aA=aA.value.slice(0,aA.value.length-q)),aA&&ZA.push(aA);return ZA}function h(){t(w6,SyntaxError);let u=T();if(t(Ja))return u;if(t(O6)){let q=BA();return t(Ja,SyntaxError),{type:"select",selector:u,...q}}throw new SyntaxError("Unclosed placeable")}function T(){if(A[e]==="{")return h();if(j(Xa)){let[,u,q,I=null]=o(Xa);if(u==="$")return{type:"var",name:q};if(t(D6)){let qA=Q();if(u==="-")return{type:"term",name:q,attr:I,args:qA};if(v6.test(q))return{type:"func",name:q,args:qA};throw new SyntaxError("Function names must be all upper-case")}return u==="-"?{type:"term",name:q,attr:I,args:[]}:{type:"mesg",name:q,attr:I}}return Ie()}function Q(){let u=[];for(;;){switch(A[e]){case")":return e++,u;case void 0:throw new SyntaxError("Unclosed argument list")}u.push(fA()),t(P6)}}function fA(){let u=T();return u.type!=="mesg"?u:t(C6)?{type:"narg",name:u.name,value:Ie()}:u}function BA(){let u=[],q=0,I;for(;j(q6);){a("*")&&(I=q);let qA=xe(),ZA=b();if(ZA===null)throw new SyntaxError("Expected variant value");u[q++]={key:qA,value:ZA}}if(q===0)return null;if(I===void 0)throw new SyntaxError("Expected default variant");return{variants:u,star:I}}function xe(){t(x6,SyntaxError);let u;return j($j)?u=YA():u={type:"str",value:f(G6)},t(I6,SyntaxError),u}function Ie(){if(j($j))return YA();if(A[e]==='"')return dj();throw new SyntaxError("Invalid expression")}function YA(){let[,u,q=""]=o($j),I=q.length;return{type:"num",value:parseFloat(u),precision:I}}function dj(){a('"',SyntaxError);let u="";for(;;){if(u+=f(k6),A[e]==="\\"){u+=OA();continue}if(a('"'))return{type:"str",value:u};throw new SyntaxError("Unclosed string literal")}}function OA(){if(j(za))return f(za);if(j(Wa)){let[,u,q]=o(Wa),I=parseInt(u||q,16);return I<=55295||57344<=I?String.fromCodePoint(I):"\uFFFD"}throw new SyntaxError("Unknown escape sequence")}function CA(){let u=e;switch(t(R6),A[e]){case".":case"[":case"*":case"}":case void 0:return!1;case"{":return oa(A.slice(u,e))}return A[e-1]===" "?oa(A.slice(u,e)):!1}function hA(u,q){return u.replace(q,"")}function oa(u){let q=u.replace($6,`
`),I=y6.exec(u)[1].length;return new Ue(q,I)}}},Ue=class{constructor(A,e){this.value=A,this.length=e}};l();l();l();l();var H6="([a-z]{2,3}|\\*)",T6="(?:-([a-z]{4}|\\*))",S6="(?:-([a-z]{2}|\\*))",K6="(?:-(([0-9][a-z0-9]{3}|[a-z0-9]{5,8})|\\*))",M6=new RegExp(`^${H6}${T6}?${S6}?${K6}?$`,"i"),uA=class{constructor(A){let e=M6.exec(A.replace(/_/g,"-"));if(!e){this.isWellFormed=!1;return}let[,j,a,t,o]=e;j&&(this.language=j.toLowerCase()),a&&(this.script=a[0].toUpperCase()+a.slice(1)),t&&(this.region=t.toUpperCase()),this.variant=o,this.isWellFormed=!0}isEqual(A){return this.language===A.language&&this.script===A.script&&this.region===A.region&&this.variant===A.variant}matches(A,e=!1,j=!1){return(this.language===A.language||e&&this.language===void 0||j&&A.language===void 0)&&(this.script===A.script||e&&this.script===void 0||j&&A.script===void 0)&&(this.region===A.region||e&&this.region===void 0||j&&A.region===void 0)&&(this.variant===A.variant||e&&this.variant===void 0||j&&A.variant===void 0)}toString(){return[this.language,this.script,this.region,this.variant].filter(A=>A!==void 0).join("-")}clearVariants(){this.variant=void 0}clearRegion(){this.region=void 0}addLikelySubtags(){let A=U6(this.toString().toLowerCase());return A?(this.language=A.language,this.script=A.script,this.region=A.region,this.variant=A.variant,!0):!1}},Qa={ar:"ar-arab-eg","az-arab":"az-arab-ir","az-ir":"az-arab-ir",be:"be-cyrl-by",da:"da-latn-dk",el:"el-grek-gr",en:"en-latn-us",fa:"fa-arab-ir",ja:"ja-jpan-jp",ko:"ko-kore-kr",pt:"pt-latn-br",sr:"sr-cyrl-rs","sr-ru":"sr-latn-ru",sv:"sv-latn-se",ta:"ta-taml-in",uk:"uk-cyrl-ua",zh:"zh-hans-cn","zh-hant":"zh-hant-tw","zh-hk":"zh-hant-hk","zh-mo":"zh-hant-mo","zh-tw":"zh-hant-tw","zh-gb":"zh-hant-gb","zh-us":"zh-hant-us"},L6=["az","bg","cs","de","es","fi","fr","hu","it","lt","lv","nl","pl","ro","ru"];function U6(r){if(Object.prototype.hasOwnProperty.call(Qa,r))return new uA(Qa[r]);let A=new uA(r);return A.language&&L6.includes(A.language)?(A.region=A.language.toUpperCase(),A):null}function yj(r,A,e){let j=new Set,a=new _;for(let t of A)new uA(t).isWellFormed&&a.set(t,new uA(t));A:for(let t of r){let o=t.toLowerCase(),f=new uA(o);if(f.language!==void 0){for(let s of a.keys())if(o===s.toLowerCase()){if(j.add(s),a.delete(s),e==="lookup")return Array.from(j);if(e==="filtering")continue;continue A}for(let[s,i]of a.entries())if(i.matches(f,!0,!1)){if(j.add(s),a.delete(s),e==="lookup")return Array.from(j);if(e==="filtering")continue;continue A}if(f.addLikelySubtags()){for(let[s,i]of a.entries())if(i.matches(f,!0,!1)){if(j.add(s),a.delete(s),e==="lookup")return Array.from(j);if(e==="filtering")continue;continue A}}f.clearVariants();for(let[s,i]of a.entries())if(i.matches(f,!0,!0)){if(j.add(s),a.delete(s),e==="lookup")return Array.from(j);if(e==="filtering")continue;continue A}if(f.clearRegion(),f.addLikelySubtags()){for(let[s,i]of a.entries())if(i.matches(f,!0,!1)){if(j.add(s),a.delete(s),e==="lookup")return Array.from(j);if(e==="filtering")continue;continue A}}f.clearRegion();for(let[s,i]of a.entries())if(i.matches(f,!0,!0)){if(j.add(s),a.delete(s),e==="lookup")return Array.from(j);if(e==="filtering")continue;continue A}}}return Array.from(j)}function wj(r,A,{strategy:e="filtering",defaultLocale:j}={}){let a=yj(Array.from(r??[]).map(String),Array.from(A??[]).map(String),e);if(e==="lookup"){if(j===void 0)throw new Error("defaultLocale cannot be undefined for strategy `lookup`");a.length===0&&a.push(j)}else j&&!a.includes(j)&&a.push(j);return a}l();var V6={"ar-SA":{"context_menu.ftl":`context-menu-download-swf = \u062D\u0645\u0651\u0650\u0644 .swf
context-menu-copy-debug-info = \u0627\u0646\u0633\u062E \u0645\u0639\u0644\u0648\u0645\u0627\u062A \u0627\u0644\u062A\u0646\u0642\u064A\u062D
context-menu-open-save-manager = \u0627\u0641\u062A\u062D \u0645\u062F\u064A\u0631 \u0627\u0644\u062D\u0641\u0638
context-menu-about-ruffle =
    { $flavor ->
        [extension] \u0639\u0646 \u0645\u0644\u062D\u0642 \u0631\u064E\u0641\u0644 ({ $version })
       *[other] \u0639\u0646 \u0631\u064E\u0641\u0644 ({ $version })
    }
context-menu-hide = \u0623\u062E\u0641\u0650 \u0647\u0630\u0647 \u0627\u0644\u0642\u0627\u0626\u0645\u0629
context-menu-exit-fullscreen = \u0627\u062E\u0631\u062C \u0645\u0646 \u0648\u0636\u0639\u064A\u0629 \u0627\u0644\u0634\u0627\u0634\u0629 \u0627\u0644\u0643\u0627\u0645\u0644\u0629
context-menu-enter-fullscreen = \u0627\u062F\u062E\u0644 \u0648\u0636\u0639\u064A\u0629 \u0627\u0644\u0634\u0627\u0634\u0629 \u0627\u0644\u0643\u0627\u0645\u0644\u0629
context-menu-volume-controls = \u0639\u0646\u0627\u0635\u0631 \u0627\u0644\u062A\u062D\u0643\u0645 \u0628\u0627\u0644\u0635\u0648\u062A
`,"messages.ftl":`message-cant-embed =
    \u0644\u0645 \u064A\u0643\u0646 \u0631\u0641\u0644 \u0642\u0627\u062F\u0631\u064B\u0627 \u0639\u0644\u0649 \u062A\u0634\u063A\u064A\u0644 \u0627\u0644\u0641\u0644\u0627\u0634 \u0627\u0644\u0645\u0636\u0645\u0646\u0629 \u0641\u064A \u0647\u0630\u0647 \u0627\u0644\u0635\u0641\u062D\u0629.
    \u064A\u0645\u0643\u0646\u0643 \u0645\u062D\u0627\u0648\u0644\u0629 \u0641\u062A\u062D \u0627\u0644\u0645\u0644\u0641 \u0641\u064A \u0639\u0644\u0627\u0645\u0629 \u062A\u0628\u0648\u064A\u0628 \u0645\u0646\u0641\u0635\u0644\u0629 \u0644\u062A\u062C\u0627\u0648\u0632 \u0647\u0630\u0647 \u0627\u0644\u0645\u0634\u0643\u0644\u0629.
message-restored-from-bfcache =
    \u0627\u0633\u062A\u0639\u0627\u062F \u0645\u062A\u0635\u0641\u062D\u0643 \u0645\u062D\u062A\u0648\u0649 \u0641\u0644\u0627\u0634 \u0647\u0630\u0627 \u0645\u0646 \u062C\u0644\u0633\u0629 \u0633\u0627\u0628\u0642\u0629.
    \u0644\u0644\u0628\u062F\u0621 \u0645\u0646 \u062C\u062F\u064A\u062F\u060C \u0623\u0639\u062F \u062A\u062D\u0645\u064A\u0644 \u0627\u0644\u0635\u0641\u062D\u0629.
panic-title = \u0644\u0642\u062F \u062D\u062F\u062B \u062E\u0637\u0623 \u0645\u0627 :(
more-info = \u0645\u0639\u0644\u0648\u0645\u0627\u062A \u0623\u0643\u062B\u0631
run-anyway = \u0634\u063A\u0651\u0650\u0644 \u0639\u0644\u0649 \u0623\u064A \u062D\u0627\u0644
continue = \u0627\u0633\u062A\u0645\u0631
report-bug = \u0628\u0644\u0651\u0650\u063A \u0639\u0646 \u0639\u0644\u0629
update-ruffle = \u062A\u062D\u062F\u064A\u062B \u0631\u0641\u0644
ruffle-demo = \u0627\u0644\u0639\u0631\u0636 \u0627\u0644\u062A\u062C\u0631\u064A\u0628\u064A
ruffle-desktop = \u0628\u0631\u0646\u0627\u0645\u062C \u0633\u0637\u062D \u0627\u0644\u0645\u0643\u062A\u0628
ruffle-wiki = \u0627\u0639\u0631\u0636 \u0648\u064A\u0643\u064A \u0631\u0641\u0644
enable-hardware-acceleration = \u064A\u0628\u062F\u0648 \u0623\u0646 \u062A\u0633\u0631\u064A\u0639 \u0627\u0644\u062C\u0647\u0627\u0632 \u0645\u0639\u0637\u0644. \u0639\u0644\u0649 \u0627\u0644\u0631\u063A\u0645 \u0645\u0646 \u0623\u0646 \u0631\u0641\u0644 \u0642\u062F \u064A\u0639\u0645\u0644\u060C \u0625\u0644\u0627 \u0623\u0646\u0647 \u0642\u062F \u064A\u0643\u0648\u0646 \u0628\u0637\u064A\u0626\u064B\u0627 \u062C\u062F\u064B\u0627. \u064A\u0645\u0643\u0646\u0643 \u0645\u0639\u0631\u0641\u0629 \u0643\u064A\u0641\u064A\u0629 \u062A\u0645\u0643\u064A\u0646 \u062A\u0633\u0631\u064A\u0639 \u0627\u0644\u0623\u062C\u0647\u0632\u0629 \u0628\u0627\u0644\u0646\u0642\u0631 \u0639\u0644\u0649 \u0627\u0644\u0631\u0627\u0628\u0637 \u0623\u062F\u0646\u0627\u0647:
enable-hardware-acceleration-link = \u0627\u0644\u0623\u0633\u0626\u0644\u0629 \u0627\u0644\u0634\u0627\u0626\u0639\u0629 - \u062A\u0633\u0631\u064A\u0639 \u0623\u062C\u0647\u0632\u0629 \u0643\u0631\u0648\u0645
view-error-details = \u0625\u0639\u0631\u0636 \u062A\u0641\u0627\u0635\u064A\u0644 \u0627\u0644\u062E\u0637\u0623
open-in-new-tab = \u0625\u0641\u062A\u062D \u0641\u064A \u0639\u0644\u0627\u0645\u0629 \u062A\u0628\u0648\u064A\u0628 \u062C\u062F\u064A\u062F\u0629
click-to-unmute = \u0625\u0646\u0642\u0631 \u0644\u0625\u0644\u063A\u0627\u0621 \u0627\u0644\u0643\u062A\u0645
clipboard-message-title = \u0627\u0644\u0646\u0633\u062E \u0648\u0627\u0644\u0644\u0635\u0642 \u0641\u064A \u0631\u0641\u0644
clipboard-message-description =
    {$variant ->
       *[unsupported] \u0645\u062A\u0635\u0641\u062D\u0643 \u0644\u0627 \u064A\u062F\u0639\u0645 \u0627\u0644\u0648\u0635\u0648\u0644 \u0644\u0644\u062D\u0627\u0641\u0638\u0629 \u0627\u0644\u0643\u0627\u0645\u0644\u0629\u060C
        [access-denied] \u062A\u0645 \u0631\u0641\u0636 \u0627\u0644\u0648\u0635\u0648\u0644 \u0644\u0644\u062D\u0627\u0641\u0638\u0629\u060C
    } \u0644\u0643\u0646 \u064A\u0645\u0643\u0646\u0643 \u0625\u0633\u062A\u062E\u062F\u0627\u0645 \u0647\u0630\u0647 \u0627\u0644\u0627\u062E\u062A\u0635\u0627\u0631\u0627\u062A \u062F\u0627\u0626\u0645\u064B\u0627:
clipboard-message-copy = { " " } \u0644\u0644\u0646\u0633\u062E
clipboard-message-cut = { " " } \u0644\u0644\u0642\u0635
clipboard-message-paste = { " " } \u0644\u0644\u0635\u0642
error-canvas-reload = \u062A\u0639\u0630\u0631 \u0625\u0639\u0627\u062F\u0629 \u0627\u0644\u062A\u062D\u0645\u064A\u0644 \u0645\u0639 \u0645\u064F\u0635\u064E\u064A\u0631 \u0627\u0644\u0644\u0648\u062D\u0629 \u0639\u0646\u062F\u0645\u0627 \u0627\u0633\u062A\u064F\u062E\u062F\u0650\u0645 \u0645\u064F\u0635\u064E\u064A\u0631 \u0627\u0644\u0644\u0648\u062D\u0629 \u0645\u0633\u0628\u0642\u064B\u0627.
error-file-protocol =
    \u064A\u0628\u062F\u0648 \u0623\u0646\u0643 \u062A\u0634\u063A\u0651\u0650\u0644 \u0631\u0641\u0644 \u0639\u0644\u0649 \u0627\u0644\u0628\u0631\u0648\u062A\u0648\u0643\u0648\u0644 "file:".
    \u0644\u0627 \u064A\u0639\u0645\u0644 \u0647\u0630\u0627 \u0625\u0630 \u062A\u0645\u0646\u0639 \u0627\u0644\u0645\u062A\u0635\u0641\u062D\u0627\u062A \u0627\u0644\u0643\u062B\u064A\u0631 \u0645\u0646 \u0627\u0644\u0645\u064A\u0632\u0627\u062A \u0645\u0646 \u0627\u0644\u0639\u0645\u0644 \u0644\u0623\u0633\u0628\u0627\u0628 \u0623\u0645\u0646\u064A\u0629.
    \u0628\u062F\u0644\u064B\u0627 \u0645\u0646 \u0630\u0644\u0643\u060C \u0646\u062F\u0639\u0648\u0643 \u0625\u0644\u0649 \u0625\u0639\u062F\u0627\u062F \u062E\u0627\u062F\u0648\u0645 \u0645\u062D\u0644\u064A \u0623\u0648 \u0627\u0633\u062A\u062E\u062F\u0627\u0645 \u0639\u0627\u0631\u0636 \u0627\u0644\u0648\u064A\u0628 \u0623\u0648 \u062A\u0637\u0628\u064A\u0642 \u0633\u0637\u062D \u0627\u0644\u0645\u0643\u062A\u0628.
error-javascript-config =
    \u062A\u0639\u0631\u0636 \u0631\u0641\u0644 \u0625\u0644\u0649 \u0645\u0634\u0643\u0644\u0629 \u0643\u0628\u064A\u0631\u0629 \u0628\u0633\u0628\u0628 \u0627\u0644\u0625\u0639\u062F\u0627\u062F\u0627\u062A \u0627\u0644\u062E\u0627\u0637\u0626\u0629 \u0644\u062C\u0627\u0641\u0627 \u0633\u0643\u0631\u0650\u0628\u062A.
    \u0625\u0630\u0627 \u0643\u0646\u062A \u0645\u0633\u0624\u0648\u0644 \u0627\u0644\u062E\u0627\u062F\u0648\u0645\u060C \u0646\u062D\u0646 \u0646\u062F\u0639\u0648\u0643 \u0625\u0644\u0649 \u0627\u0644\u062A\u062D\u0642\u0642 \u0645\u0646 \u062A\u0641\u0627\u0635\u064A\u0644 \u0627\u0644\u062E\u0637\u0623 \u0644\u0645\u0639\u0631\u0641\u0629 \u0633\u0628\u0628 \u0627\u0644\u0645\u0634\u0643\u0644\u0629.
    \u064A\u0645\u0643\u0646\u0643 \u0623\u064A\u0636\u064B\u0627 \u0627\u0644\u0631\u062C\u0648\u0639 \u0625\u0644\u0649 \u0648\u064A\u0643\u064A \u0631\u0641\u0644 \u0644\u0644\u062D\u0635\u0648\u0644 \u0639\u0644\u0649 \u0627\u0644\u0645\u0633\u0627\u0639\u062F\u0629.
error-wasm-not-found =
    \u0641\u0634\u0644 \u0631\u0641\u0644 \u0641\u064A \u062A\u062D\u0645\u064A\u0644 \u0645\u0643\u0648\u0646 \u0627\u0644\u0645\u0644\u0641 ".wasm" \u0627\u0644\u0645\u0637\u0644\u0648\u0628.
    \u0625\u0630\u0627 \u0643\u0646\u062A \u0645\u0633\u0624\u0648\u0644 \u0627\u0644\u062E\u0627\u062F\u0648\u0645\u060C \u064A\u0631\u062C\u0649 \u0627\u0644\u062A\u0623\u0643\u062F \u0645\u0646 \u0623\u0646 \u0627\u0644\u0645\u0644\u0641 \u0642\u062F \u0631\u064F\u0641\u0650\u0639 \u0628\u0634\u0643\u0644 \u0635\u062D\u064A\u062D.
    \u0625\u0630\u0627 \u0627\u0633\u062A\u0645\u0631\u062A \u0627\u0644\u0645\u0634\u0643\u0644\u0629\u060C \u0642\u062F \u062A\u062D\u062A\u0627\u062C \u0625\u0644\u0649 \u0627\u0633\u062A\u062E\u062F\u0627\u0645 \u0625\u0639\u062F\u0627\u062F "publicPath": \u0631\u062C\u0627\u0621\u064B \u0631\u0627\u062C\u0639 \u0648\u064A\u0643\u064A \u0631\u0641\u0644 \u0644\u0644\u062D\u0635\u0648\u0644 \u0639\u0644\u0649 \u0627\u0644\u0645\u0633\u0627\u0639\u062F\u0629.
error-wasm-mime-type =
    \u0648\u0627\u062C\u0647 \u0631\u0641\u0644 \u0645\u0634\u0643\u0644\u0629 \u0643\u0628\u064A\u0631\u0629 \u0623\u062B\u0646\u0627\u0621 \u0645\u062D\u0627\u0648\u0644\u0629 \u0627\u0644\u062A\u0647\u064A\u0626\u0629.
    \u062E\u0627\u062F\u0648\u0645 \u0627\u0644\u0648\u064A\u0628 \u0647\u0630\u0627 \u0644\u0627 \u064A\u062E\u062F\u0645 \u0645\u0644\u0641\u0627\u062A ". wasm" \u0645\u0639 \u0646\u0648\u0639 MIME \u0627\u0644\u0635\u062D\u064A\u062D.
    \u0625\u0630\u0627 \u0643\u0646\u062A \u0645\u0633\u0624\u0648\u0644 \u0627\u0644\u062E\u0627\u062F\u0645\u060C \u064A\u0631\u062C\u0649 \u0645\u0631\u0627\u062C\u0639\u0629 \u0648\u064A\u0643\u064A \u0631\u0641\u0644 \u0644\u0644\u062D\u0635\u0648\u0644 \u0639\u0644\u0649 \u0627\u0644\u0645\u0633\u0627\u0639\u062F\u0629.
error-invalid-swf =
    \u0644\u0627 \u064A\u0645\u0643\u0646 \u0644\u0631\u0641\u0644 \u062A\u062D\u0644\u064A\u0644 \u0627\u0644\u0645\u0644\u0641 \u0627\u0644\u0645\u0637\u0644\u0648\u0628.
    \u0627\u0644\u0633\u0628\u0628 \u0627\u0644\u0623\u0643\u062B\u0631 \u0625\u062D\u062A\u0645\u0627\u0644\u0627\u064B \u0647\u0648 \u0623\u0646 \u0627\u0644\u0645\u0644\u0641 \u0627\u0644\u0645\u0637\u0644\u0648\u0628 \u0644\u064A\u0633 \u0635\u0627\u0644\u062D\u064B\u0627.
error-swf-fetch =
    \u0641\u0634\u0644 \u0631\u0641\u0644 \u0641\u064A \u062A\u062D\u0645\u064A\u0644 \u0645\u0644\u0641 \u0641\u0644\u0627\u0634 SWF.
    \u0627\u0644\u0633\u0628\u0628 \u0627\u0644\u0623\u0643\u062B\u0631 \u0627\u062D\u062A\u0645\u0627\u0644\u064B\u0627 \u0647\u0648 \u0623\u0646 \u0627\u0644\u0645\u0644\u0641 \u0644\u0645 \u064A\u0639\u062F \u0645\u0648\u062C\u0648\u062F\u064B\u0627\u060C \u0644\u0630\u0644\u0643 \u0644\u0627 \u064A\u0648\u062C\u062F \u0634\u064A\u0621 \u0644\u064A\u062D\u0645\u0644\u0647 \u0631\u0641\u0644.
    \u062D\u0627\u0648\u0644 \u0627\u0644\u0627\u062A\u0635\u0627\u0644 \u0628\u0645\u0633\u0624\u0648\u0644 \u0627\u0644\u0645\u0648\u0642\u0639 \u0644\u0644\u062D\u0635\u0648\u0644 \u0639\u0644\u0649 \u0627\u0644\u0645\u0633\u0627\u0639\u062F\u0629.
error-swf-cors =
    \u0641\u0634\u0644 \u0631\u0641\u0644 \u0641\u064A \u062A\u062D\u0645\u064A\u0644 \u0645\u0644\u0641 \u0641\u0644\u0627\u0634 SWF.
    \u0645\u0646 \u0627\u0644\u0645\u062D\u062A\u0645\u0644 \u0623\u0646 \u0625\u062D\u0636\u0627\u0631 \u0627\u0644\u0645\u0644\u0641 \u0642\u062F \u062D\u064F\u0638\u0650\u0631 \u0628\u0648\u0627\u0633\u0637\u0629 \u0633\u064A\u0627\u0633\u0629 CORS.
    \u0625\u0630\u0627 \u0643\u0646\u062A \u0645\u0633\u0624\u0648\u0644 \u0627\u0644\u062E\u0627\u062F\u0645\u060C \u064A\u0631\u062C\u0649 \u0645\u0631\u0627\u062C\u0639\u0629 \u0631\u0641\u0644 \u0648\u064A\u0643\u064A \u0644\u0644\u062D\u0635\u0648\u0644 \u0639\u0644\u0649 \u0627\u0644\u0645\u0633\u0627\u0639\u062F\u0629.
error-wasm-cors =
    \u0641\u0634\u0644 \u0631\u0641\u0644 \u0641\u064A \u062A\u062D\u0645\u064A\u0644 \u0645\u0643\u0648\u0646 \u0645\u0644\u0641 ".wasm" \u0627\u0644\u0645\u0637\u0644\u0648\u0628.
    \u0645\u0646 \u0627\u0644\u0645\u062D\u062A\u0645\u0644 \u0623\u0646 \u0625\u062D\u0636\u0627\u0631 \u0627\u0644\u0645\u0644\u0641 \u0642\u062F \u062D\u064F\u0638\u0650\u0631 \u0628\u0648\u0627\u0633\u0637\u0629 \u0633\u064A\u0627\u0633\u0629 CORS.
    \u0625\u0630\u0627 \u0643\u0646\u062A \u0645\u0633\u0624\u0648\u0644 \u0627\u0644\u062E\u0627\u062F\u0645\u060C \u064A\u0631\u062C\u0649 \u0645\u0631\u0627\u062C\u0639\u0629 \u0631\u0641\u0644 \u0648\u064A\u0643\u064A \u0644\u0644\u062D\u0635\u0648\u0644 \u0639\u0644\u0649 \u0627\u0644\u0645\u0633\u0627\u0639\u062F\u0629.
error-wasm-invalid =
    \u0648\u0627\u062C\u0647 \u0631\u0641\u0644 \u0645\u0634\u0643\u0644\u0629 \u0643\u0628\u064A\u0631\u0629 \u0623\u062B\u0646\u0627\u0621 \u0645\u062D\u0627\u0648\u0644\u0629 \u0627\u0644\u062A\u0647\u064A\u0626\u0629.
    \u064A\u0628\u062F\u0648 \u0623\u0646 \u0647\u0630\u0647 \u0627\u0644\u0635\u0641\u062D\u0629 \u062A\u062D\u062A\u0648\u064A \u0639\u0644\u0649 \u0645\u0644\u0641\u0627\u062A \u0645\u0641\u0642\u0648\u062F\u0629 \u0623\u0648 \u063A\u064A\u0631 \u0635\u0627\u0644\u062D\u0629 \u0644\u062A\u0634\u063A\u064A\u0644 \u0631\u0641\u0644.
    \u0625\u0630\u0627 \u0643\u0646\u062A \u0645\u0633\u0624\u0648\u0644 \u0627\u0644\u062E\u0627\u062F\u0645\u060C \u064A\u0631\u062C\u0649 \u0645\u0631\u0627\u062C\u0639\u0629 \u0648\u064A\u0643\u064A \u0631\u0641\u0644 \u0644\u0644\u062D\u0635\u0648\u0644 \u0639\u0644\u0649 \u0627\u0644\u0645\u0633\u0627\u0639\u062F\u0629.
error-wasm-download =
    \u0648\u0627\u062C\u0647 \u0631\u0641\u0644 \u0645\u0634\u0643\u0644\u0629 \u0643\u0628\u064A\u0631\u0629 \u0623\u062B\u0646\u0627\u0621 \u0645\u062D\u0627\u0648\u0644\u062A\u0647\u0627 \u0627\u0644\u062A\u0647\u064A\u0626\u0629.
    \u0647\u0630\u0627 \u064A\u0645\u0643\u0646 \u0623\u0646 \u064A\u062D\u0644 \u0646\u0641\u0633\u0647 \u0641\u064A \u0643\u062B\u064A\u0631 \u0645\u0646 \u0627\u0644\u0623\u062D\u064A\u0627\u0646\u060C \u0644\u0630\u0644\u0643 \u064A\u0645\u0643\u0646\u0643 \u0645\u062D\u0627\u0648\u0644\u0629 \u0625\u0639\u0627\u062F\u0629 \u062A\u062D\u0645\u064A\u0644 \u0627\u0644\u0635\u0641\u062D\u0629.
    \u0648\u0625\u0644\u0627 \u064A\u0631\u062C\u0649 \u0627\u0644\u0627\u062A\u0635\u0627\u0644 \u0628\u0645\u062F\u064A\u0631 \u0627\u0644\u0645\u0648\u0642\u0639.
error-wasm-disabled-on-edge =
    \u0641\u0634\u0644 Ruffle \u0641\u064A \u062A\u062D\u0645\u064A\u0644 \u0645\u0643\u0648\u0646 \u0627\u0644\u0645\u0644\u0641 ".wasm" \u0627\u0644\u0645\u0637\u0644\u0648\u0628.
    \u0644\u0625\u0635\u0644\u0627\u062D \u0647\u0630\u0647 \u0627\u0644\u0645\u0634\u0643\u0644\u0629\u060C \u062D\u0627\u0648\u0644 \u0641\u062A\u062D \u0625\u0639\u062F\u0627\u062F\u0627\u062A \u0645\u062A\u0635\u0641\u062D\u0643\u060C \u062B\u0645 \u0625\u0646\u0642\u0631 \u0641\u0648\u0642 "\u0627\u0644\u062E\u0635\u0648\u0635\u064A\u0629\u060C \u0627\u0644\u0628\u062D\u062B\u060C \u0627\u0644\u062E\u062F\u0645\u0627\u062A"\u060C \u0648\u0627\u0644\u062A\u0645\u0631\u064A\u0631 \u0644\u0623\u0633\u0641\u0644\u060C \u0648\u0625\u064A\u0642\u0627\u0641 "\u062A\u0639\u0632\u064A\u0632 \u0623\u0645\u0627\u0646\u0643 \u0639\u0644\u0649 \u0627\u0644\u0648\u064A\u0628".
    \u0647\u0630\u0627 \u0633\u064A\u0633\u0645\u062D \u0644\u0645\u062A\u0635\u0641\u062D\u0643 \u0628\u062A\u062D\u0645\u064A\u0644 \u0627\u0644\u0645\u0644\u0641\u0627\u062A ".wasm" \u0627\u0644\u0645\u0637\u0644\u0648\u0628\u0629.
    \u0625\u0630\u0627 \u0625\u0633\u062A\u0645\u0631\u062A \u0627\u0644\u0645\u0634\u0643\u0644\u0629\u060C \u0642\u062F \u062A\u062D\u062A\u0627\u062C \u0625\u0644\u0649 \u0625\u0633\u062A\u062E\u062F\u0627\u0645 \u0645\u062A\u0635\u0641\u062D \u0623\u062E\u0631.
error-wasm-unsupported-browser =
    \u0644\u0627 \u064A\u062F\u0639\u0645 \u0627\u0644\u0645\u062A\u0635\u0641\u062D \u0627\u0644\u0630\u064A \u062A\u0633\u062A\u062E\u062F\u0645\u0647 \u0627\u0645\u062A\u062F\u0627\u062F\u0627\u062A WebAssembly \u0627\u0644\u0630\u064A \u064A\u062A\u0637\u0644\u0628\u0647 \u0631\u0641\u0644 \u0644\u062A\u0634\u063A\u064A\u0644\u0647.
    \u0631\u062C\u0627\u0621\u064B \u0627\u0646\u062A\u0642\u0644 \u0644\u0645\u062A\u0635\u0641\u062D \u062F\u0627\u0639\u0645.
    \u064A\u0645\u0643\u0646\u0643 \u0625\u064A\u062C\u0627\u062F \u0644\u0627\u0626\u062D\u0629 \u0644\u0644\u0645\u062A\u0635\u0641\u062D\u0627\u062A \u0627\u0644\u062F\u0627\u0639\u0645\u0629 \u0641\u064A \u0627\u0644\u0648\u064A\u0643\u064A.
error-javascript-conflict =
    \u0648\u0627\u062C\u0647 \u0631\u0641\u0644 \u0645\u0634\u0643\u0644\u0629 \u0643\u0628\u064A\u0631\u0629 \u0623\u062B\u0646\u0627\u0621 \u0645\u062D\u0627\u0648\u0644\u0629 \u0627\u0644\u062A\u0647\u064A\u0626\u0629.
    \u064A\u0628\u062F\u0648 \u0623\u0646 \u0647\u0630\u0647 \u0627\u0644\u0635\u0641\u062D\u0629 \u062A\u0633\u062A\u062E\u062F\u0645 \u0643\u0648\u062F \u062C\u0627\u0641\u0627 \u0633\u0643\u0631\u064A\u0628\u062A \u0627\u0644\u0630\u064A \u064A\u062A\u0639\u0627\u0631\u0636 \u0645\u0639 \u0631\u0641\u0644.
    \u0625\u0630\u0627 \u0643\u0646\u062A \u0645\u0633\u0624\u0648\u0644 \u0627\u0644\u062E\u0627\u062F\u0645\u060C \u0641\u0625\u0646\u0646\u0627 \u0646\u062F\u0639\u0648\u0643 \u0625\u0644\u0649 \u0645\u062D\u0627\u0648\u0644\u0629 \u062A\u062D\u0645\u064A\u0644 \u0627\u0644\u0645\u0644\u0641 \u0639\u0644\u0649 \u0635\u0641\u062D\u0629 \u0641\u0627\u0631\u063A\u0629.
error-javascript-conflict-outdated = \u064A\u0645\u0643\u0646\u0643 \u0623\u064A\u0636\u064B\u0627 \u0645\u062D\u0627\u0648\u0644\u0629 \u062A\u062D\u0645\u064A\u0644 \u0646\u0633\u062E\u0629 \u0623\u062D\u062F\u062B \u0645\u0646 \u0631\u0641\u0644 \u0627\u0644\u062A\u064A \u0642\u062F \u062A\u062D\u0644 \u0627\u0644\u0645\u0634\u0643\u0644\u0629 (\u0627\u0644\u0646\u0633\u062E\u0629 \u0627\u0644\u062D\u0627\u0644\u064A\u0629 \u0642\u062F\u064A\u0645\u0629: { $buildDate }).
error-csp-conflict =
    \u0648\u0627\u062C\u0647 Ruffle \u0645\u0634\u0643\u0644\u0629 \u0643\u0628\u064A\u0631\u0629 \u0623\u062B\u0646\u0627\u0621 \u0645\u062D\u0627\u0648\u0644\u0629 \u0627\u0644\u062A\u0647\u064A\u0626\u0629.
    \u0644\u0627 \u062A\u0633\u0645\u062D \u0633\u064A\u0627\u0633\u0629 \u0623\u0645\u0627\u0646 \u0627\u0644\u0645\u062D\u062A\u0648\u0649 \u0644\u062E\u0627\u062F\u0645 \u0627\u0644\u0648\u064A\u0628 \u0647\u0630\u0627 \u0628\u062A\u0634\u063A\u064A\u0644 \u0645\u0643\u0648\u0646 ".wasm" \u0627\u0644\u0645\u0637\u0644\u0648\u0628.
    \u0625\u0630\u0627 \u0643\u0646\u062A \u0645\u0633\u0624\u0648\u0644 \u0627\u0644\u062E\u0627\u062F\u0645\u060C \u064A\u0631\u062C\u0649 \u0627\u0644\u0631\u062C\u0648\u0639 \u0625\u0644\u0649 \u0648\u064A\u0643\u064A Ruffle \u0644\u0644\u062D\u0635\u0648\u0644 \u0639\u0644\u0649 \u0627\u0644\u0645\u0633\u0627\u0639\u062F\u0629.
error-unknown =
    \u0648\u0627\u062C\u0647 \u0631\u0641\u0644 \u0645\u0634\u0643\u0644\u0629 \u0643\u0628\u064A\u0631\u0629 \u0623\u062B\u0646\u0627\u0621 \u0645\u062D\u0627\u0648\u0644\u0629 \u0639\u0631\u0636 \u0645\u062D\u062A\u0648\u0649 \u0627\u0644\u0641\u0644\u0627\u0634 \u0647\u0630\u0627.
    { $outdated ->
        [true] \u0625\u0630\u0627 \u0643\u0646\u062A \u0645\u0633\u0624\u0648\u0644 \u0627\u0644\u062E\u0627\u062F\u0645\u060C \u064A\u0631\u062C\u0649 \u0645\u062D\u0627\u0648\u0644\u0629 \u062A\u062D\u0645\u064A\u0644 \u0625\u0635\u062F\u0627\u0631 \u0623\u062D\u062F\u062B \u0645\u0646 \u0631\u0641\u0644 (\u0627\u0644\u0646\u0633\u062E\u0629 \u0627\u0644\u062D\u0627\u0644\u064A\u0629 \u0642\u062F\u064A\u0645\u0629: { $buildDate }).
       *[false] \u0644\u064A\u0633 \u0645\u0646 \u0627\u0644\u0645\u0641\u062A\u0631\u0636 \u0623\u0646 \u064A\u062D\u062F\u062B \u0647\u0630\u0627\u060C \u0644\u0630\u0644\u0643 \u0646\u062D\u0646 \u0646\u0642\u062F\u0631 \u062D\u0642\u064B\u0627 \u0625\u0630\u0627 \u0628\u0644\u063A\u062A \u0639\u0646 \u0627\u0644\u062E\u0637\u0623!
    }
`,"save-manager.ftl":`save-delete-prompt = \u0647\u0644 \u0623\u0646\u062A \u0645\u062A\u0623\u0643\u062F \u0623\u0646\u0643 \u062A\u0631\u064A\u062F \u062D\u0630\u0641 \u0645\u0644\u0641 \u0627\u0644\u062D\u0641\u0638 \u0647\u0630\u0627\u061F
save-reload-prompt =
    \u0627\u0644\u0637\u0631\u064A\u0642\u0629 \u0627\u0644\u0648\u062D\u064A\u062F\u0629 \u0644\u0640 { $action ->
        [delete] \u062D\u0630\u0641
       *[replace] \u0625\u0633\u062A\u0628\u062F\u0627\u0644
    } \u0645\u0644\u0641 \u0627\u0644\u062D\u0641\u0638 \u0647\u0630\u0627 \u062F\u0648\u0646 \u062A\u0639\u0627\u0631\u0636 \u0645\u062D\u062A\u0645\u0644 \u0647\u064A \u0625\u0639\u0627\u062F\u0629 \u062A\u062D\u0645\u064A\u0644 \u0647\u0630\u0647 \u0627\u0644\u0635\u0641\u062D\u0629. \u0647\u0644 \u062A\u0631\u063A\u0628 \u0641\u064A \u0627\u0644\u0645\u062A\u0627\u0628\u0639\u0629 \u0639\u0644\u0649 \u0623\u064A \u062D\u0627\u0644\u061F
save-download = \u062D\u0645\u0651\u0644
save-replace = \u0625\u0633\u062A\u0628\u062F\u0644
save-delete = \u0625\u062D\u0630\u0641
save-backup-all = \u062D\u0645\u0651\u0644 \u062C\u0645\u064A\u0639 \u0645\u0644\u0641\u0627\u062A \u0627\u0644\u062D\u0641\u0638
`,"volume-controls.ftl":`volume-controls-mute = \u0625\u0643\u062A\u0645
volume-controls-unmute = \u0623\u0644\u063A\u0650 \u0627\u0644\u0643\u062A\u0645
`},"bs-BA":{"context_menu.ftl":`context-menu-download-swf = Preuzmite SWF datoteku
context-menu-copy-debug-info = Kopiraj informacije o otklanjanju gre\u0161aka
context-menu-open-save-manager = Otvori upravitelj spremanja
context-menu-about-ruffle =
    { $flavor ->
    [extension] O ekstenziji Ruffle-a ({ $version })
    *[other] O Ruffle-u ({ $version })
    }
context-menu-hide = Sakrij ovaj meni
context-menu-exit-fullscreen = Izlaz iz re\u017Eima punog ekrana
context-menu-enter-fullscreen = Pre\u0111i na cijeli ekran
context-menu-volume-controls = Kontrole ja\u010Dine zvuka
`,"messages.ftl":`message-cant-embed =
    Ruffle nije mogao pokrenuti Flash ugra\u0111en na ovoj stranici.
    Mo\u017Eete poku\u0161ati otvoriti datoteku u zasebnoj kartici kako biste izbjegli ovaj problem.
message-restored-from-bfcache =
    Va\u0161 preglednik je vratio ovaj Flash sadr\u017Eaj iz prethodne sesije.
    Molimo vas da ponovo u\u010Ditate stranicu za novi po\u010Detak.
panic-title = Ne\u0161to je po\u0161lo po zlu :(
more-info = Dodatne informacije
run-anyway = Ipak pokreni
continue = Nastavi
report-bug = Prijavi gre\u0161ku
update-ruffle = A\u017Euriraj Ruffle
ruffle-demo = Web probna verzija
ruffle-desktop = Desktop aplikacija
ruffle-wiki = Pogledaj Ruffle Wiki
enable-hardware-acceleration = Izgleda da je hardversko ubrzanje onemogu\u0107eno. Iako Ruffle mo\u017Eda radi, mogu\u0107e je da je vrlo spor. Mo\u017Eete saznati kako omogu\u0107iti hardversko ubrzanje slijede\u0107i link ispod:
enable-hardware-acceleration-link = \u010Cesto postavljana pitanja - Hardversko ubrzanje u Chromeu
view-error-details = Prika\u017Ei detalje gre\u0161ke
open-in-new-tab = Otvori u novoj kartici
click-to-unmute = Kliknite da biste uklju\u010Dili zvuk
clipboard-message-title = Kopiranje i naljepljivanje u Ruffle-u
clipboard-message-description =
    { $variant ->
    *[unsupported] Va\u0161 preglednik ne podr\u017Eava potpuni pristup me\u0111uspremniku,
    [access-denied] Pristup me\u0111uspremniku je odbijen,
    } ali uvijek mo\u017Eete koristiti ove pre\u010Dice:
clipboard-message-copy = { " " } za kopiranje
clipboard-message-cut = { " " } za isijecanje
clipboard-message-paste = { " " } za lijepljenje
error-canvas-reload = Nije mogu\u0107e ponovo u\u010Ditati renderer kada je renderer ve\u0107 u upotrebi.
error-file-protocol =
    Izgleda da koristite Ruffle na protokolu "file:".
    Ovo ne funkcioni\u0161e jer preglednici blokiraju mnoge funkcije iz sigurnosnih razloga.
    Umjesto toga, preporu\u010Dujemo vam da postavite lokalni server ili koristite web probnu verziju ili aplikaciju.
error-javascript-config =
    Ruffle je nai\u0161ao na ozbiljan problem zbog pogre\u0161ne konfiguracije JavaScript-a.
    Ako ste administrator servera, preporu\u010Dujemo vam da provjerite detalje gre\u0161ke kako biste saznali koji parametar uzrokuje problem. Tako\u0111er mo\u017Eete konsultovati Ruffle wiki za pomo\u0107.
error-wasm-not-found =
    Ruffle nije uspio u\u010Ditati potrebnu komponentu datoteke ".wasm".
    Ako ste administrator servera, provjerite je li datoteka ispravno otpremljena.
    Ako problem i dalje postoji, mo\u017Eda \u0107ete morati koristiti postavku "publicPath": obratite se Ruffle wiki stranici za pomo\u0107.
error-wasm-mime-type =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja inicijalizacije.
    Ovaj web server ne poslu\u017Euje ".wasm" datoteke s ispravnim MIME tipom.
    Ako ste administrator servera, molimo vas da se obratite Ruffle wiki stranici za pomo\u0107.
error-invalid-swf =
    Ruffle ne mo\u017Ee analizirati tra\u017Eenu datoteku.
    Najvjerovatniji razlog je taj \u0161to tra\u017Eena datoteka nije va\u017Ee\u0107i SWF.
error-swf-fetch =
    Ruffle nije uspio u\u010Ditati Flash SWF datoteku.
    Najvjerovatniji razlog je taj \u0161to datoteka vi\u0161e ne postoji, tako da Ruffle nema \u0161ta u\u010Ditati.
    Poku\u0161ajte kontaktirati administratora web stranice za pomo\u0107.
error-swf-cors =
    Ruffle nije uspio u\u010Ditati Flash SWF datoteku.
    Pristup za preuzimanje je vjerovatno blokiran CORS politikom.
    Ako ste administrator servera, obratite se Ruffle wiki stranici za pomo\u0107.
error-wasm-cors =
    Ruffle nije uspio u\u010Ditati potrebnu komponentu datoteke ".wasm".
    Pristup dohvatu je vjerovatno blokiran CORS politikom.
    Ako ste administrator servera, obratite se Ruffle wiki stranici za pomo\u0107.
error-wasm-invalid =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja inicijalizacije.
    Izgleda da ovoj stranici nedostaju ili su datoteke neva\u017Ee\u0107e za pokretanje Rufflea.
    Ako ste administrator servera, pogledajte Ruffle wiki za pomo\u0107.
error-wasm-download =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja inicijalizacije.
    Ovo se \u010Desto mo\u017Ee rije\u0161iti jednostavnim ponovnim u\u010Ditavanjem stranice.
    U suprotnom, kontaktirajte administratora stranice.
error-wasm-disabled-on-edge =
    Ruffle nije uspio u\u010Ditati potrebnu komponentu datoteke ".wasm".
    Da biste rije\u0161ili ovaj problem, poku\u0161ajte otvoriti postavke preglednika, kliknuti na "Privatnost, pretraga i usluge", pomaknuti se prema dolje i isklju\u010Diti "Pobolj\u0161anje web sigurnosti".
    Ovo \u0107e omogu\u0107iti va\u0161em pregledniku da u\u010Dita potrebne datoteke ".wasm".
    Ako problem i dalje postoji, mo\u017Eda \u0107ete morati koristiti drugi preglednik.
error-wasm-unsupported-browser =
    Preglednik koji koristite ne podr\u017Eava WebAssembly ekstenzije potrebne za rad Ruffle-a.
    Molimo vas da pre\u0111ete na podr\u017Eani preglednik.
    Popis podr\u017Eanih preglednika mo\u017Eete prona\u0107i na Wiki stranici.
error-javascript-conflict =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja inicijalizacije.
    Izgleda da ova stranica koristi JavaScript kod koji je u sukobu sa Ruffleom.
    Ako ste administrator servera, pozivamo vas da poku\u0161ate otpremiti datoteku na praznu stranicu.
error-javascript-conflict-outdated = Tako\u0111er mo\u017Eete poku\u0161ati prenijeti noviju verziju Rufflea koja bi mogla rije\u0161iti problem (trenutna verzija je zastarjela: { $buildDate }).
error-csp-conflict =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja inicijalizacije.
    Politike sigurnosti sadr\u017Eaja ovog web servera ne dozvoljavaju pokretanje potrebne komponente ".wasm".
    Ako ste administrator servera, obratite se Ruffle wiki stranici za pomo\u0107.
error-unknown =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja prikazivanja ovog Flash sadr\u017Eaja.
    { $outdated ->
    [true] Ako ste administrator servera, poku\u0161ajte prenijeti noviju verziju Rufflea (trenutna verzija je zastarjela: { $buildDate }).
    *[false] Ovo se ne bi trebalo dogoditi, pa bismo vam bili jako zahvalni ako biste prijavili gre\u0161ku!
    }
`,"save-manager.ftl":`save-delete-prompt = Jeste li sigurni da \u017Eelite izbrisati ovu sa\u010Duvanu datoteku?
save-reload-prompt =
    Jedini na\u010Din da { $action ->
    [delete] izbri\u0161ete
    *[replace] zamijenite
    } ovu sa\u010Duvanu datoteku bez potencijalnog konflikta je da ponovo u\u010Ditate ovaj sadr\u017Eaj. \u017Delite li ipak nastaviti?
save-download = Preuzmite
save-replace = Zamijeni
save-delete = Izbri\u0161i
save-backup-all = Preuzmi sve sa\u010Duvane datoteke
`,"volume-controls.ftl":`volume-controls-mute = Isklju\u010Di zvuk
volume-controls-unmute = Uklju\u010Di zvuk
`},"ca-ES":{"context_menu.ftl":`context-menu-download-swf = Baixa el fitxer SWF
context-menu-copy-debug-info = Copia la informaci\xF3 de depuraci\xF3
context-menu-open-save-manager = Obre el gestor d'emmagatzematge
context-menu-about-ruffle =
    { $flavor ->
        [extension] Quant a l'extensi\xF3 de Ruffle ({ $version })
       *[other] Quant a Ruffle ({ $version })
    }
context-menu-hide = Amaga aquest men\xFA
context-menu-exit-fullscreen = Surt de la pantalla completa
context-menu-enter-fullscreen = Pantalla completa
context-menu-volume-controls = Controls de volum
`,"messages.ftl":`message-cant-embed =
    Ruffle no ha pogut executar el contingut Flash incrustat en aquesta p\xE0gina.
    Podeu provar d'obrir el fitxer en una pestanya a part per evitar aquest problema.
panic-title = Alguna cosa ha fallat :(
more-info = M\xE9s informaci\xF3
run-anyway = Reprodueix igualment
continue = Continua
report-bug = Informa d'un error
update-ruffle = Actualitza Ruffle
ruffle-demo = Demostraci\xF3 web
ruffle-desktop = Aplicaci\xF3 d'escriptori
ruffle-wiki = Obre la wiki de Ruffle
enable-hardware-acceleration-link = FAQ - Acceleraci\xF3 per Hardware a Chrome
view-error-details = Mostra detalls de l'error
open-in-new-tab = Obre en una pestanya nova
click-to-unmute = Feu clic per activar el so
clipboard-message-title = Copiar i enganxar en Ruffle
error-file-protocol =
    Sembla que esteu executant Ruffle al protocol "file:".
    Aix\xF2 no funcionar\xE0 perqu\xE8 els navegadors bloquegen moltes caracter\xEDstiques per raons de seguretat. En comptes d'aix\xF2, us suggerim que configureu un servidor local o b\xE9 utilitzeu la demostraci\xF3 web o l'aplicaci\xF3 d'escriptori.
error-javascript-config =
    Ruffle ha topat amb un problema greu a causa d'una configuraci\xF3 JavaScript err\xF2nia.
    Si sou l'administrador del servidor, us suggerim que comproveu els detalls de l'error per determinar el par\xE0metre culpable.
    Tamb\xE9 podeu consultar la wiki del Ruffle per obtenir ajuda.
error-wasm-not-found =
    Ruffle no ha pogut carregar el component de fitxer ".wasm" necessari.
    Si sou l'administrador del servidor, si us plau, comproveu que el fitxer ha estat carregat correctament.
    Si el problema continua, \xE9s possible que h\xE0giu d'utilitzar el par\xE1metre "publicPath": us preguem que consulteu la wiki de Ruffle per obtenir ajuda.
error-wasm-mime-type =
    Ruffle ha topat amb un problema greu mentre provava d'inicialitzar-se.
    Aquest servidor no est\xE0 servint els fitxers ".wasm" amb el tipus MIME adequat.
    Si sou l'administrador del servidor, us preguem que consulteu la wiki de Ruffle per obtenir ajuda.
error-invalid-swf =
    Ruffle no ha pogut llegir el fitxer sol\xB7licitat.
    La ra\xF3 m\xE9s probable \xE9s que no sigui un fitxer SWF v\xE0lid.
error-swf-fetch =
    Ruffle no ha pogut carregar el fitxer SWF Flash.
    La ra\xF3 m\xE9s probable \xE9s que el fitxer ja no existeixi, aix\xED que no hi ha res que el Ruffle pugui carregar.
    Proveu de contactar a l'administrador del lloc per obtenir ajuda.
error-swf-cors =
    Ruffle no ha pogut carregar el fitxer SWF Flash.
    \xC9s probable que l'acc\xE9s a la c\xE0rrega hagi estat denegat per una pol\xEDtica CORS.
    Si sou l'administrador del servidor, us preguem que consulteu la wiki del Ruffle per obtenir ajuda.
error-wasm-cors =
    Ruffle no ha pogut carregar el component de fitxer ".wasm" necessari.
    \xC9s probable que l'acc\xE9s a la c\xE0rrega hagi estat denegat per una pol\xEDtica CORS.
    Si sou l'administrador del servidor, us preguem que consulteu la wiki del Ruffle per obtenir ajuda.
error-wasm-invalid =
    Ruffle ha topat amb un problema greu mentre provava d'inicialitzar-se.
    Sembla que a aquest lloc li manquen fitxers o aquests no s\xF3n v\xE0lids per a l'execuci\xF3 de Ruffle.
    Si sou l'administrador del servidor, us preguem que consulteu la wiki de Ruffle per obtenir ajuda.
error-wasm-download =
    Ruffle ha topat amb un problema greu mentre provava d'inicialitzar-se.
    Aix\xF2 sovint aix\xF2 pot resoldre's sol, aix\xED que podeu provar de recarregar la p\xE0gina.
    En cas contrari, us preguem que contacteu l'administrador del lloc.
error-wasm-disabled-on-edge =
    Ruffle no ha pogut carregar el component de fitxer ".wasm" necessari.
    Per a arreglar-ho, proveu d'obrir els par\xE0metres del navegador, feu clic sobre "Privadesa, cerca i serveis", i desactiveu "Prevenci\xF3 de seguiment".
    Aix\xF2 permetr\xE0 que el vostre navegador carregui els fitxers ".wasm" necessaris.
    Si el problema continua, possiblement haureu d'utilitzar un altre navegador.
error-javascript-conflict =
    Ruffle ha topat amb un problema greu mentre provava d'inicialitzar-se.
    Sembla que aquest lloc fa servir codi JavaScript que entra en conflicte amb Ruffle.
    Si sou l'administrador del servidor, us preguem que consulteu la wiki de Ruffle per obtenir ajuda.
error-javascript-conflict-outdated = Tamb\xE9 podeu provar de carregar una versi\xF3 m\xE9s recent de Ruffle que podria resoldre el problema (la compilaci\xF3 actual est\xE0 desactualitzada: { $buildDate }).
error-csp-conflict =
    Ruffle ha topat amb un problema greu mentre provava d'inicialitzar-se.
    La pol\xEDtica de seguretat del contingut (CSP) no permet l'execuci\xF3 del component ".wasm" necessari.
    Si sou l'administrador del servidor, us preguem que consulteu la wiki de Ruffle per obtenir ajuda.
error-unknown =
    Ruffle ha topat amb un problema greu mentre provava de mostrar aquest contingut Flash.
    { $outdated ->
        [true] Si sou l'administrador del servidor, us preguem que proveu de carregar una versi\xF3 m\xE9s recent de Ruffle (la compilaci\xF3 actual est\xE0 desactualitzada: { $buildDate }).
       *[false] Aix\xF2 no hauria d'haver passat, aix\xED que us agrair\xEDem molt que n'inform\xE9ssiu l'error!
    }
`,"save-manager.ftl":`save-delete-prompt = Segur que vols esborrar aquest fitxer desat?
save-reload-prompt =
    L'\xFAnica forma d{ $action ->
        [delete] 'eliminar
       *[replace] e substituir
    } aquest fitxer desat sense crear un potencial conflicte \xE9s recarregant el contingut. Voleu continuar igualment?
save-download = Baixa
save-replace = Substitueix
save-delete = Elimina
save-backup-all = Baixa tots els fitxers desats
`,"volume-controls.ftl":`volume-controls-mute = Silenci
`},"cs-CZ":{"context_menu.ftl":`context-menu-download-swf = St\xE1hnout SWF
context-menu-copy-debug-info = Zkop\xEDrovat debug info
context-menu-open-save-manager = Otev\u0159\xEDt spr\xE1vce ulo\u017Een\xED
context-menu-about-ruffle =
    { $flavor ->
         [extension] O Ruffle roz\u0161\xED\u0159en\xED ({ $version })
        *[other] O Ruffle ({ $version })
    }
context-menu-hide = Skr\xFDt menu
context-menu-exit-fullscreen = Ukon\u010Dit re\u017Eim cel\xE9 obrazovky
context-menu-enter-fullscreen = P\u0159ej\xEDt do re\u017Eimu cel\xE9 obrazovky
context-menu-volume-controls = Ovl\xE1d\xE1n\xED hlasitosti
`,"messages.ftl":`message-cant-embed =
    Ruffle nemohl spustit Flash vlo\u017Een\xFD na t\xE9to str\xE1nce.
    M\u016F\u017Eete se pokusit otev\u0159\xEDt soubor na samostatn\xE9 kart\u011B, abyste se vyhnuli tomuto probl\xE9mu.
message-restored-from-bfcache =
    V\xE1\u0161 prohl\xED\u017Ee\u010D obnovil tento Flash obsah z p\u0159edchoz\xED relace.
    Chcete-li za\u010D\xEDt znovu, znovu na\u010Dt\u011Bte str\xE1nku.
panic-title = N\u011Bco se pokazilo :(
more-info = Dal\u0161\xED informace
run-anyway = P\u0159esto spustit
continue = Pokra\u010Dovat
report-bug = Nahl\xE1sit chybu
update-ruffle = Aktualizovat Ruffle
ruffle-demo = Web Demo
ruffle-desktop = Desktopov\xE1 aplikace
ruffle-wiki = Zobrazit Ruffle Wiki
enable-hardware-acceleration = Zd\xE1 se, \u017Ee hardwarov\xE1 akcelerace je vypnut\xE1. I kdy\u017E Ruffle funguje spr\xE1vn\u011B, m\u016F\u017Ee b\xFDt nep\u0159im\u011B\u0159en\u011B pomal\xFD. Jak povolit hardwarovou akceleraci zjist\xEDte na tomto odkazu:
enable-hardware-acceleration-link = \u010Cast\xE9 dotazy - Hardwarov\xE1 akcelerace Chrome
view-error-details = Zobrazit podrobnosti o chyb\u011B
open-in-new-tab = Otev\u0159\xEDt na nov\xE9 kart\u011B
click-to-unmute = Kliknut\xEDm zru\u0161\xEDte ztlumen\xED
clipboard-message-title = Kop\xEDrov\xE1n\xED a vkl\xE1d\xE1n\xED v Ruffle
clipboard-message-description =
    { $variant ->
       *[unsupported] V\xE1\u0161 prohl\xED\u017Ee\u010D nepodporuje pln\xFD p\u0159\xEDstup ke schr\xE1nce,
        [access-denied] P\u0159\xEDstup ke schr\xE1nce byl odep\u0159en,
    } ale m\xEDsto toho m\u016F\u017Eete v\u017Edy pou\u017E\xEDt tyto zkratky:
clipboard-message-copy = { " " } pro kop\xEDrov\xE1n\xED
clipboard-message-cut = { " " } pro vyst\u0159ihov\xE1n\xED
clipboard-message-paste = { " " } pro vkl\xE1d\xE1n\xED
error-canvas-reload = Nelze znovu na\u010D\xEDst pomoc\xED vykreslova\u010De pl\xE1tna, pokud je vykreslova\u010D pl\xE1tna ji\u017E pou\u017E\xEDv\xE1n.
error-file-protocol =
    Zd\xE1 se, \u017Ee pou\u017E\xEDv\xE1te Ruffle na protokolu "file:".
    To nen\xED mo\u017En\xE9, proto\u017Ee prohl\xED\u017Ee\u010De blokuj\xED fungov\xE1n\xED mnoha funkc\xED z bezpe\u010Dnostn\xEDch d\u016Fvod\u016F.
    Nam\xEDsto toho v\xE1m doporu\u010Dujeme nastavit lok\xE1ln\xED server nebo pou\u017E\xEDt web demo \u010Di desktopovou aplikaci.
error-javascript-config =
    Ruffle narazil na probl\xE9m v d\u016Fsledku nespr\xE1vn\xE9 konfigurace JavaScriptu.
    Pokud jste spr\xE1vcem serveru, doporu\u010Dujeme v\xE1m zkontrolovat podrobnosti o chyb\u011B, abyste zjistili, kter\xFD parametr je vadn\xFD.
    Pomoc m\u016F\u017Eete z\xEDskat tak\xE9 na wiki Ruffle.
error-wasm-not-found =
    Ruffle se nepoda\u0159ilo na\u010D\xEDst po\u017Eadovanou komponentu souboru \u201E.wasm\u201C.
    Pokud jste spr\xE1vcem serveru, zkontrolujte, zda byl soubor spr\xE1vn\u011B nahr\xE1n.
    Pokud probl\xE9m p\u0159etrv\xE1v\xE1, mo\u017En\xE1 budete muset pou\u017E\xEDt nastaven\xED \u201EpublicPath\u201C: pomoc naleznete na wiki Ruffle.
error-wasm-mime-type =
    Ruffle narazil na probl\xE9m p\u0159i pokusu o inicializaci.
    Tento webov\xFD server neposkytuje soubory \u201E.wasm\u201C se spr\xE1vn\xFDm typem MIME.
    Pokud jste spr\xE1vcem serveru, n\xE1pov\u011Bdu najdete na Ruffle wiki.
error-invalid-swf =
    Ruffle nem\u016F\u017Ee zpracovat po\u017Eadovan\xFD soubor.
    Nejpravd\u011Bpodobn\u011Bj\u0161\xEDm d\u016Fvodem je, \u017Ee po\u017Eadovan\xFD soubor nen\xED platn\xFDm souborem SWF.
error-swf-fetch =
    Ruffle se nepoda\u0159ilo na\u010D\xEDst SWF soubor Flash.
    Nejpravd\u011Bpodobn\u011Bj\u0161\xEDm d\u016Fvodem je, \u017Ee soubor ji\u017E neexistuje, tak\u017Ee Ruffle nem\xE1 co na\u010D\xEDst.
    Zkuste po\u017E\xE1dat o pomoc spr\xE1vce webu.
error-swf-cors =
    Ruffle se nepoda\u0159ilo na\u010D\xEDst SWF soubor Flash.
    P\u0159\xEDstup k na\u010D\xEDt\xE1n\xED byl pravd\u011Bpodobn\u011B zablokov\xE1n politikou CORS.
    Pokud jste spr\xE1vcem serveru, n\xE1pov\u011Bdu najdete na Ruffle wiki.
error-wasm-cors =
    Ruffle se nepoda\u0159ilo na\u010D\xEDst po\u017Eadovanou komponentu souboru \u201E.wasm\u201C.
    P\u0159\xEDstup k na\u010D\xEDt\xE1n\xED byl pravd\u011Bpodobn\u011B zablokov\xE1n politikou CORS.
    Pokud jste spr\xE1vcem serveru, n\xE1pov\u011Bdu najdete na Ruffle wiki.
error-wasm-invalid =
    Ruffle narazil na probl\xE9m p\u0159i pokusu o inicializaci.
    Zd\xE1 se, \u017Ee na t\xE9to str\xE1nce chyb\xED nebo jsou neplatn\xE9 soubory ke spu\u0161t\u011Bn\xED Ruffle.
    Pokud jste spr\xE1vcem serveru, n\xE1pov\u011Bdu najdete na Ruffle wiki.
error-wasm-download =
    Ruffle narazil na probl\xE9m p\u0159i pokusu o inicializaci.
    Probl\xE9m se m\u016F\u017Ee vy\u0159e\u0161it i s\xE1m, tak\u017Ee m\u016F\u017Eete zkusit str\xE1nku na\u010D\xEDst znovu.
    V opa\u010Dn\xE9m p\u0159\xEDpad\u011B kontaktujte administr\xE1tora str\xE1nky.
error-wasm-disabled-on-edge =
    Ruffle se nepoda\u0159ilo na\u010D\xEDst po\u017Eadovanou komponentu souboru \u201E.wasm\u201C.
    Chcete-li tento probl\xE9m vy\u0159e\u0161it, zkuste otev\u0159\xEDt nastaven\xED prohl\xED\u017Ee\u010De, klikn\u011Bte na polo\u017Eku \u201EOchrana osobn\xEDch \xFAdaj\u016F, vyhled\xE1v\xE1n\xED a slu\u017Eby\u201C, p\u0159ejd\u011Bte dol\u016F a vypn\u011Bte mo\u017Enost \u201EZvy\u0161te svou bezpe\u010Dnost na webu\u201C.
    Va\u0161emu prohl\xED\u017Ee\u010Di to umo\u017En\xED na\u010D\xEDst po\u017Eadovan\xE9 soubory \u201E.wasm\u201C.
    Pokud probl\xE9m p\u0159etrv\xE1v\xE1, budete mo\u017En\xE1 muset pou\u017E\xEDt jin\xFD prohl\xED\u017Ee\u010D.
error-wasm-unsupported-browser =
    Prohl\xED\u017Ee\u010D, kter\xFD pou\u017E\xEDv\xE1te, nepodporuje roz\u0161\xED\u0159en\xED WebAssembly, kter\xE9 Ruffle vy\u017Eaduje ke spu\u0161t\u011Bn\xED.
    P\u0159ejd\u011Bte na podporovan\xFD prohl\xED\u017Ee\u010D.
    Seznam podporovan\xFDch prohl\xED\u017Ee\u010D\u016F naleznete na Wiki.
error-javascript-conflict =
    Ruffle narazil na probl\xE9m p\u0159i pokusu o inicializaci.
    Zd\xE1 se, \u017Ee tato str\xE1nka pou\u017E\xEDv\xE1 k\xF3d JavaScript, kter\xFD je v konfliktu s Ruffle.
    Pokud jste spr\xE1vcem serveru, doporu\u010Dujeme v\xE1m zkusit na\u010D\xEDst soubor na pr\xE1zdnou str\xE1nku.
error-javascript-conflict-outdated = M\u016F\u017Eete se tak\xE9 pokusit nahr\xE1t nov\u011Bj\u0161\xED verzi Ruffle, kter\xE1 m\u016F\u017Ee dan\xFD probl\xE9m vy\u0159e\u0161it (aktu\xE1ln\xED build je zastaral\xFD: { $buildDate }).
error-csp-conflict =
    Ruffle narazil na probl\xE9m p\u0159i pokusu o inicializaci.
    Z\xE1sady zabezpe\u010Den\xED obsahu tohoto webov\xE9ho serveru nepovoluj\xED spu\u0161t\u011Bn\xED po\u017Eadovan\xE9 komponenty \u201E.wasm\u201C.
    Pokud jste spr\xE1vcem serveru, n\xE1pov\u011Bdu najdete na Ruffle wiki.
error-unknown =
    Ruffle narazil na probl\xE9m p\u0159i pokusu zobrazit tento Flash obsah.
    { $outdated ->
          [true] Pokud jste spr\xE1vcem serveru, zkuste nahr\xE1t nov\u011Bj\u0161\xED verzi Ruffle (aktu\xE1ln\xED build je zastaral\xFD: { $buildDate }).
         *[false] Toto by se nem\u011Blo st\xE1t, tak\u017Ee bychom opravdu ocenili, kdybyste mohli nahl\xE1sit chybu!
    }
`,"save-manager.ftl":`save-delete-prompt = Opravdu chcete odstranit tento soubor s ulo\u017Een\xFDmi pozicemi?
save-reload-prompt =
    Jedin\xFD zp\u016Fsob, jak { $action ->
          [delete] vymazat
         *[replace] nahradit
    } tento soubor s ulo\u017Een\xFDmi pozicemi bez potenci\xE1ln\xEDho konfliktu je op\u011Btovn\xE9 na\u010Dten\xED tohoto obsahu. Chcete p\u0159esto pokra\u010Dovat?
save-download = St\xE1hnout
save-replace = Nahradit
save-delete = Vymazat
save-backup-all = St\xE1hnout v\u0161echny soubory s ulo\u017Een\xFDmi pozicemi
`,"volume-controls.ftl":`volume-controls-mute = Ztlumit
volume-controls-unmute = Zru\u0161it ztlumen\xED
`},"de-DE":{"context_menu.ftl":`context-menu-download-swf = SWF herunterladen
context-menu-copy-debug-info = Debug-Info kopieren
context-menu-open-save-manager = Dateimanager \xF6ffnen
context-menu-about-ruffle =
    { $flavor ->
        [extension] \xDCber Ruffle Erweiterung ({ $version })
       *[other] \xDCber Ruffle ({ $version })
    }
context-menu-hide = Men\xFC ausblenden
context-menu-exit-fullscreen = Vollbild verlassen
context-menu-enter-fullscreen = Vollbildmodus aktivieren
context-menu-volume-controls = Lautst\xE4rke einstellen
`,"messages.ftl":`message-cant-embed =
    Ruffle konnte das in diese Seite eingebettete Flash-Element nicht ausf\xFChren.
    Sie k\xF6nnen versuchen, die Datei in einem separaten Tab zu \xF6ffnen, um dieses Problem zu umgehen.
message-restored-from-bfcache =
    Ihr Browser hat diesen Flash-Inhalt aus einer vorherigen Sitzung wiederhergestellt.
    Laden Sie die Seite neu, um neu zu starten.
panic-title = Etwas ist schiefgelaufen :(
more-info = Weitere Informationen
run-anyway = Trotzdem ausf\xFChren
continue = Fortfahren
report-bug = Fehler melden
update-ruffle = Ruffle aktualisieren
ruffle-demo = Web-Demo
ruffle-desktop = Desktop-Anwendung
ruffle-wiki = Ruffle-Wiki anzeigen
enable-hardware-acceleration = Es sieht so aus, als sei die Hardwarebeschleunigung deaktiviert. Ruffle funktioniert zwar m\xF6glicherweise, k\xF6nnte aber sehr langsam sein. Unter dem folgenden Link erfahren Sie, wie Sie die Hardwarebeschleunigung aktivieren k\xF6nnen:
enable-hardware-acceleration-link = FAQ - Chrome Hardwarebeschleunigung
view-error-details = Fehlerdetails anzeigen
open-in-new-tab = In einem neuen Tab \xF6ffnen
click-to-unmute = Zum Aktivieren des Tons klicken
clipboard-message-title = Kopieren und Einf\xFCgen in Ruffle
clipboard-message-description =
    { $variant ->
       *[unsupported] Ihr Browser unterst\xFCtzt keinen vollst\xE4ndigen Zugriff auf die Zwischenablage,
        [access-denied] Der Zugriff auf die Zwischenablage wurde verweigert,
    } Sie k\xF6nnen jedoch stattdessen jederzeit diese Tastenkombinationen verwenden:
clipboard-message-copy = { " " } zum Kopieren
clipboard-message-cut = { " " } zum Ausschneiden
clipboard-message-paste = { " " } zum Einf\xFCgen
error-canvas-reload = Das Neuladen mit dem Canvas-Renderer ist nicht m\xF6glich, wenn dieser bereits verwendet wird.
error-file-protocol =
    Es scheint, als w\xFCrden Sie Ruffle \xFCber das "file:"-Protokoll ausf\xFChren.
    Dies funktioniert nicht, da Browser aus Sicherheitsgr\xFCnden viele Funktionen blockieren.
    Wir empfehlen Ihnen stattdessen, einen lokalen Server einzurichten oder entweder die Web-Demo oder die Desktop-Anwendung zu nutzen.
error-javascript-config =
    Bei Ruffle ist aufgrund einer fehlerhaften JavaScript-Konfiguration ein schwerwiegendes Problem aufgetreten.
    Wenn Sie der Serveradministrator sind, bitten wir Sie, die Fehlerdetails zu \xFCberpr\xFCfen, um festzustellen, welcher Parameter die Ursache ist.
    Sie k\xF6nnen auch im Ruffle-Wiki nach Hilfe suchen.
error-wasm-not-found =
    Ruffle konnte die erforderliche ".wasm"-Datei-Komponente nicht laden.
    Wenn Sie der Server-Administrator sind, stellen Sie bitte sicher, dass die Datei korrekt hochgeladen wurde.
    Wenn das Problem weiterhin besteht, m\xFCssen Sie unter Umst\xE4nden die "publicPath"-Einstellung verwenden: Bitte konsultieren Sie das Ruffle-Wiki f\xFCr Hilfe.
error-wasm-mime-type =
    Bei der Initialisierung von Ruffle ist ein schwerwiegendes Problem aufgetreten.
    Dieser Webserver stellt ".wasm"-Dateien nicht mit dem richtigen MIME-Typ bereit.
    Wenn Sie der Serveradministrator sind, finden Sie Hilfe im Ruffle-Wiki.
error-invalid-swf =
    Ruffle kann die angeforderte Datei nicht verarbeiten.
    Der wahrscheinlichste Grund daf\xFCr ist, dass die angeforderte Datei keine g\xFCltige SWF-Datei ist.
error-swf-fetch =
    Ruffle konnte die Flash-SWF-Datei nicht laden.
    Der wahrscheinlichste Grund ist, dass die Datei nicht mehr vorhanden ist und Ruffle daher nichts laden kann.
    Wenden Sie sich bitte an den Administrator der Website, um Hilfe zu erhalten.
error-swf-cors =
    Ruffle konnte die Flash-SWF-Datei nicht laden.
    Der Zugriff auf die Datei wurde wahrscheinlich durch die CORS-Richtlinie blockiert.
    Wenn Sie der Serveradministrator sind, finden Sie Hilfe im Ruffle-Wiki.
error-wasm-cors =
    Ruffle konnte die Flash-SWF-Datei nicht laden.
    Der Zugriff auf den Abruf wurde wahrscheinlich durch die CORS-Richtlinie blockiert.
    Wenn Sie der Serveradministrator sind, finden Sie Hilfe im Ruffle-Wiki.
error-wasm-invalid =
    Bei der Initialisierung von Ruffle ist ein schwerwiegendes Problem aufgetreten.
    Es scheint, als fehlten auf dieser Seite Dateien, die f\xFCr die Ausf\xFChrung von Ruffle erforderlich sind, oder als seien diese ung\xFCltig.
    Wenn Sie der Serveradministrator sind, finden Sie Hilfe im Ruffle-Wiki.
error-wasm-download =
    Bei der Initialisierung von Ruffle ist ein schwerwiegendes Problem aufgetreten.
    Oftmals behebt sich dieses Problem von selbst, sodass Sie versuchen k\xF6nnen, die Seite neu zu laden.
    Andernfalls wenden Sie sich an den Website-Administrator.
error-wasm-disabled-on-edge =
    Ruffle konnte die erforderliche ".wasm"-Datei nicht laden.
    Um das Problem zu beheben, \xF6ffnen Sie die Einstellungen Ihres Browsers, klicken Sie auf "Datenschutz, Suche und Dienste", scrollen Sie nach unten und deaktivieren Sie die Option "Sicherheit im Internet verbessern".
    Dadurch kann Ihr Browser die erforderlichen ".wasm"-Dateien laden.
    Sollte das Problem weiterhin bestehen, m\xFCssen Sie m\xF6glicherweise einen anderen Browser verwenden.
error-wasm-unsupported-browser =
    Der von Ihnen verwendete Browser unterst\xFCtzt die WebAssembly-Erweiterungen nicht, die Ruffle zum Ausf\xFChren ben\xF6tigt.
    Bitte wechseln Sie zu einem unterst\xFCtzten Browser.
    Eine Liste der unterst\xFCtzten Browser finden Sie im Wiki.
error-javascript-conflict =
    Bei der Initialisierung von Ruffle ist ein schwerwiegendes Problem aufgetreten.
    Es scheint, als w\xFCrde diese Seite JavaScript-Code verwenden, der mit Ruffle in Konflikt steht.
    Falls Sie der Serveradministrator sind, bitten wir Sie, die Datei auf einer leeren Seite zu laden.
error-javascript-conflict-outdated = Sie k\xF6nnen auch versuchen, eine neuere Version von Ruffle hochzuladen, die das Problem m\xF6glicherweise behebt (der aktuelle Build ist veraltet: { $buildDate }).
error-csp-conflict =
    Bei der Initialisierung von Ruffle ist ein schwerwiegendes Problem aufgetreten.
    Die Content Security Policy dieses Webservers l\xE4sst die Ausf\xFChrung der erforderlichen ".wasm"-Komponente nicht zu.
    Wenn Sie der Serveradministrator sind, finden Sie Hilfe im Ruffle-Wiki.
error-url-invalid =
    Ruffle konnte die SWF-Datei nicht laden.
    Der wahrscheinlichste Grund ist eine fehlerhafte URL.
error-unknown =
    Bei der Anzeige dieses Flash-Inhalts ist bei Ruffle ein schwerwiegendes Problem aufgetreten.
    { $outdated ->
        [true] Wenn Sie der Serveradministrator sind, versuchen Sie bitte, eine aktuellere Version von Ruffle hochzuladen (der aktuelle Build ist veraltet: { $buildDate }).
       *[false] Das sollte eigentlich nicht passieren, daher w\xE4ren wir Ihnen sehr dankbar, wenn Sie den Fehler melden k\xF6nnten!
    }
`,"save-manager.ftl":`save-delete-prompt = Sind Sie sicher, dass Sie diese Speicherdatei l\xF6schen m\xF6chten?
save-reload-prompt =
    Diese Speicherdatei kann nur ohne Konflikte { $action ->
        [delete] gel\xF6scht
       *[replace] ersetzt
    } werden, wenn der Inhalt neu geladen wird. Trotzdem fortfahren?
save-download = Herunterladen
save-replace = Ersetzen
save-delete = L\xF6schen
save-backup-all = Alle Speicherdateien herunterladen
`,"volume-controls.ftl":`volume-controls-mute = Stummschalten
volume-controls-unmute = Stummschaltung aufheben
`},"en-US":{"context_menu.ftl":`context-menu-download-swf = Download SWF
context-menu-copy-debug-info = Copy Debug Info
context-menu-open-save-manager = Open Save Manager
context-menu-about-ruffle =
    { $flavor ->
        [extension] About Ruffle Extension ({$version})
        *[other] About Ruffle ({$version})
    }
context-menu-hide = Hide This Menu
context-menu-exit-fullscreen = Exit Full Screen
context-menu-enter-fullscreen = Enter Full Screen
context-menu-volume-controls = Volume Controls
`,"messages.ftl":`message-cant-embed =
    Ruffle wasn't able to run the Flash embedded in this page.
    You can try to open the file in a separate tab, to sidestep this issue.
message-restored-from-bfcache =
    Your browser restored this Flash content from a previous session.
    To start fresh, reload the page.
panic-title = Something went wrong :(
more-info = More info
run-anyway = Run anyway
continue = Continue
report-bug = Report Bug
update-ruffle = Update Ruffle
ruffle-demo = Web Demo
ruffle-desktop = Desktop Application
ruffle-wiki = View Ruffle Wiki
enable-hardware-acceleration = It looks like hardware acceleration is disabled. While Ruffle may work, it could be very slow. You can find out how to enable hardware acceleration by following the link below:
enable-hardware-acceleration-link = FAQ - Chrome Hardware Acceleration
view-error-details = View Error Details
open-in-new-tab = Open in a new tab
click-to-unmute = Click to unmute
clipboard-message-title = Copying and pasting in Ruffle
clipboard-message-description =
    { $variant ->
        *[unsupported] Your browser does not support full clipboard access,
        [access-denied] Access to the clipboard has been denied,
    } but you can always use these shortcuts instead:
clipboard-message-copy = { " " } for copy
clipboard-message-cut = { " " } for cut
clipboard-message-paste = { " " } for paste
error-canvas-reload = Cannot reload with the canvas renderer when the canvas renderer is already in use.
error-file-protocol =
    It appears you are running Ruffle on the "file:" protocol.
    This doesn't work as browsers block many features from working for security reasons.
    Instead, we invite you to setup a local server or either use the web demo or the desktop application.
error-javascript-config =
    Ruffle has encountered a major issue due to an incorrect JavaScript configuration.
    If you are the server administrator, we invite you to check the error details to find out which parameter is at fault.
    You can also consult the Ruffle wiki for help.
error-wasm-not-found =
    Ruffle failed to load the required ".wasm" file component.
    If you are the server administrator, please ensure the file has correctly been uploaded.
    If the issue persists, you may need to use the "publicPath" setting: please consult the Ruffle wiki for help.
error-wasm-mime-type =
    Ruffle has encountered a major issue whilst trying to initialize.
    This web server is not serving ".wasm" files with the correct MIME type.
    If you are the server administrator, please consult the Ruffle wiki for help.
error-invalid-swf =
    Ruffle cannot parse the requested file.
    The most likely reason is that the requested file is not a valid SWF.
error-swf-fetch =
    Ruffle failed to load the Flash SWF file.
    The most likely reason is that the file no longer exists, so there is nothing for Ruffle to load.
    Try contacting the website administrator for help.
error-swf-cors =
    Ruffle failed to load the Flash SWF file.
    Access to fetch has likely been blocked by CORS policy.
    If you are the server administrator, please consult the Ruffle wiki for help.
error-wasm-cors =
    Ruffle failed to load the required ".wasm" file component.
    Access to fetch has likely been blocked by CORS policy.
    If you are the server administrator, please consult the Ruffle wiki for help.
error-wasm-invalid =
    Ruffle has encountered a major issue whilst trying to initialize.
    It seems like this page has missing or invalid files for running Ruffle.
    If you are the server administrator, please consult the Ruffle wiki for help.
error-wasm-download =
    Ruffle has encountered a major issue whilst trying to initialize.
    This can often resolve itself, so you can try reloading the page.
    Otherwise, please contact the website administrator.
error-wasm-disabled-on-edge =
    Ruffle failed to load the required ".wasm" file component.
    To fix this, try opening your browser's settings, clicking "Privacy, search, and services", scrolling down, and turning off "Enhance your security on the web".
    This will allow your browser to load the required ".wasm" files.
    If the issue persists, you might have to use a different browser.
error-wasm-unsupported-browser =
    The browser you are using does not support the WebAssembly extensions Ruffle requires to run.
    Please switch to a supported browser.
    You can find a list of supported browsers on the Wiki.
error-javascript-conflict =
    Ruffle has encountered a major issue whilst trying to initialize.
    It seems like this page uses JavaScript code that conflicts with Ruffle.
    If you are the server administrator, we invite you to try loading the file on a blank page.
error-javascript-conflict-outdated = You can also try to upload a more recent version of Ruffle that may circumvent the issue (current build is outdated: {$buildDate}).
error-csp-conflict =
    Ruffle has encountered a major issue whilst trying to initialize.
    This web server's Content Security Policy does not allow the required ".wasm" component to run.
    If you are the server administrator, please consult the Ruffle wiki for help.
error-url-invalid =
    Ruffle failed to load the Flash SWF file.
    The most likely reason is that an invalid URL for the SWF file was passed to Ruffle.
error-unknown =
    Ruffle has encountered a major issue whilst trying to display this Flash content.
    {$outdated ->
        [true] If you are the server administrator, please try to upload a more recent version of Ruffle (current build is outdated: {$buildDate}).
        *[false] This isn't supposed to happen, so we'd really appreciate if you could file a bug!
    }
`,"save-manager.ftl":`save-delete-prompt = Are you sure you want to delete this save file?
save-reload-prompt =
    The only way to {$action ->
    [delete] delete
    *[replace] replace
    } this save file without potential conflict is to reload this content. Do you wish to continue anyway?
save-download = Download
save-replace = Replace
save-delete = Delete
save-backup-all = Download all save files
`,"volume-controls.ftl":`volume-controls-mute = Mute
volume-controls-unmute = Unmute
`},"eo-UY":{"context_menu.ftl":"","messages.ftl":"","save-manager.ftl":"","volume-controls.ftl":""},"es-ES":{"context_menu.ftl":`context-menu-download-swf = Descargar SWF
context-menu-copy-debug-info = Copiar informaci\xF3n de depuraci\xF3n
context-menu-open-save-manager = Abrir gestor de guardado
context-menu-about-ruffle =
    { $flavor ->
        [extension] Sobre la extensi\xF3n de Ruffle ({ $version })
       *[other] Sobre Ruffle ({ $version })
    }
context-menu-hide = Ocultar este men\xFA
context-menu-exit-fullscreen = Salir de pantalla completa
context-menu-enter-fullscreen = Entrar a pantalla completa
context-menu-volume-controls = Controles de volumen
`,"messages.ftl":`message-cant-embed =
    Ruffle no pudo ejecutar el Flash incrustado en esta p\xE1gina.
    Puedes intentar abrir el archivo en una pesta\xF1a aparte, para evitar este problema.
message-restored-from-bfcache =
    Su navegador ha recuperado este contenido Flash de una sesi\xF3n anterior.
    Para empezar de cero, refresque la p\xE1gina.
panic-title = Algo sali\xF3 mal :(
more-info = M\xE1s info
run-anyway = Ejecutar de todos modos
continue = Continuar
report-bug = Reportar un error
update-ruffle = Actualizar Ruffle
ruffle-demo = Demostraci\xF3n de web
ruffle-desktop = Aplicaci\xF3n de escritorio
ruffle-wiki = Ver la p\xE1gina wiki
enable-hardware-acceleration = Al parecer, la aceleraci\xF3n de hardware est\xE1 deshabilitada. Puede que Ruffle funcione, pero este podr\xEDa funcionar muy lentamente. Puedes averiguar como habilitar aceleraci\xF3n de hardware presionando el enlace:
enable-hardware-acceleration-link = Preguntas frecuentes sobre la aceleraci\xF3n de hardware en Chrome
view-error-details = Ver los detalles del error
open-in-new-tab = Abrir en una pesta\xF1a nueva
click-to-unmute = Haz clic para dejar de silenciar
clipboard-message-title = Para copiar y pegar en Ruffle
clipboard-message-description =
    { $variant ->
       *[unsupported] Este navegador no apoya acceso completo al portapapeles,
        [access-denied] Se ha denegado el acceso al portapapeles,
    } pero siempre se puede utilizar estos atajos:
clipboard-message-copy = Para copiar
clipboard-message-cut = Para cortar
clipboard-message-paste = Para pegar
error-canvas-reload = No se puede recargar con el renderizado de lienzo cuando este ya est\xE1 en uso.
error-file-protocol =
    Parece que est\xE1 ejecutando Ruffle en el protocolo "archivo:".
    Esto no funciona porque los navegadores bloquean que muchas caracter\xEDsticas funcionen por razones de seguridad.
    En su lugar, le invitamos a configurar un servidor local o bien usar la demostraci\xF3n web o la aplicaci\xF3n de desktop.
error-javascript-config =
    Ruffle ha encontrado un problema cr\xEDtico debido a una configuraci\xF3n JavaScript incorrecta.
    Si usted es el administrador del servidor, le invitamos a comprobar los detalles del error para averiguar qu\xE9 par\xE1metro est\xE1 en falta.
    Tambi\xE9n puedes consultar la wiki de Ruffle para obtener ayuda.
error-wasm-not-found =
    Ruffle no pudo cargar el componente de archivo ".wasm" requerido.
    Si usted es el administrador del servidor, aseg\xFArese de que el archivo ha sido subido correctamente.
    Si el problema persiste, puede que necesite usar la configuraci\xF3n "publicPath": por favor consulte la wiki de Ruffle para obtener ayuda.
error-wasm-mime-type =
    Ruffle ha encontrado un problema cr\xEDtico al intentar inicializar.
    Este servidor web no est\xE1 sirviendo archivos wasm" con el tipo MIME correcto.
    Si usted es el administrador del servidor, consulte la wiki de Ruffle para obtener ayuda.
error-invalid-swf = Ruffle no puede analizar el archivo solicitado. La raz\xF3n m\xE1s probable es que no es un archivo v\xE1lido SWF.
error-swf-fetch =
    Ruffle no pudo cargar el archivo Flash SWF.
    La raz\xF3n m\xE1s probable es que el archivo ya no existe, as\xED que no hay nada para cargar Ruffle.
    Intente ponerse en contacto con el administrador del sitio web para obtener ayuda.
error-swf-cors =
    Ruffle no pudo cargar el archivo Flash SWF.
    Es probable que el acceso a la b\xFAsqueda haya sido bloqueado por la pol\xEDtica CORS.
    Si usted es el administrador del servidor, consulte la wiki de Ruffle para obtener ayuda.
error-wasm-cors =
    Ruffle no pudo cargar el archivo ".wasm."
    Es probable que el acceso a la b\xFAsqueda o la llamada a la funci\xF3n fetch haya sido bloqueado por la pol\xEDtica CORS.
    Si usted es el administrador del servidor, consulte la wiki de Ruffle para obtener ayuda.
error-wasm-invalid =
    Ruffle ha encontrado un problema cr\xEDtico al intentar inicializar.
    Este servidor web no est\xE1 sirviendo archivos wasm" con el tipo Mime correcto.
    Si usted es el administrador del servidor, consulte la wiki de Ruffle para obtener ayuda.
error-wasm-download =
    Ruffle ha encontrado un problema cr\xEDtico mientras intentaba inicializarse.
    Esto a menudo puede resolverse por s\xED mismo, as\xED que puede intentar recargar la p\xE1gina.
    De lo contrario, p\xF3ngase en contacto con el administrador del sitio web.
error-wasm-disabled-on-edge =
    Ruffle no pudo cargar el componente de archivo ".wasm" requerido.
    Para solucionar esto, intenta abrir la configuraci\xF3n de tu navegador, haciendo clic en "Privacidad, b\xFAsqueda y servicios", desplaz\xE1ndote y apagando "Mejore su seguridad en la web".
    Esto permitir\xE1 a su navegador cargar los archivos ".wasm" necesarios.
    Si el problema persiste, puede que tenga que utilizar un navegador diferente.
error-wasm-unsupported-browser =
    Este navegador no apoya las extensiones de WebAssembly que Ruffle requiere para ejecutar.
    Por favor, cambia a un navegador apoyado.
    Se puede ver una lista de navegadores apoyados en el Wiki.
error-javascript-conflict =
    Ruffle ha encontrado un problema cr\xEDtico mientras intentaba inicializarse.
    Parece que esta p\xE1gina utiliza c\xF3digo JavaScript que entra en conflicto con Ruffle.
    Si usted es el administrador del servidor, le invitamos a intentar cargar el archivo en una p\xE1gina en blanco.
error-javascript-conflict-outdated = Tambi\xE9n puedes intentar subir una versi\xF3n m\xE1s reciente de Ruffle que puede eludir el problema (la versi\xF3n actual est\xE1 desactualizada: { $buildDate }).
error-csp-conflict =
    Ruffle encontr\xF3 un problema al intentar inicializarse.
    La Pol\xEDtica de Seguridad de Contenido de este servidor web no permite el componente requerido ".wasm".
    Si usted es el administrador del servidor, por favor consulta la wiki de Ruffle para obtener ayuda.
error-unknown =
    Ruffle ha encontrado un problema al tratar de mostrar el contenido Flash.
    { $outdated ->
        [true] Si usted es el administrador del servidor, intenta cargar una version m\xE1s reciente de Ruffle (la version actual esta desactualizada: { $buildDate }).
       *[false] Esto no deberia suceder! apreciariamos que reportes el error!
    }
`,"save-manager.ftl":`save-delete-prompt = \xBFEst\xE1 seguro de querer eliminar este archivo de guardado?
save-reload-prompt =
    La \xFAnica forma de { $action ->
        [delete] eliminar
       *[replace] sobreescribir
    } este archivo de guardado sin conflictos potenciales es reiniciando el contenido. \xBFDesea continuar de todos modos?
save-download = Descargar
save-replace = Sobreescribir
save-delete = Borrar
save-backup-all = Borrar todos los archivos de guardado
`,"volume-controls.ftl":`volume-controls-mute = Silenciar
volume-controls-unmute = Desmutear
`},"fi-FI":{"context_menu.ftl":`context-menu-download-swf = Lataa SWF
context-menu-copy-debug-info = Kopioi vianj\xE4ljitystiedot
context-menu-about-ruffle =
    { $flavor ->
        [extension] Tietoja \u2013 Ruffle-laajennus ({ $version })
       *[other] Tietoja \u2013 Ruffle ({ $version })
    }
context-menu-hide = Piilota t\xE4m\xE4 valikko
context-menu-exit-fullscreen = Poistu koko n\xE4yt\xF6n tilasta
context-menu-enter-fullscreen = Siirry koko n\xE4yt\xF6n tilaan
context-menu-volume-controls = \xC4\xE4nenvoimakkuuden s\xE4\xE4t\xF6
`,"messages.ftl":`message-restored-from-bfcache =
    Selaimesi palautti t\xE4m\xE4n Flash-sis\xE4ll\xF6n aiemmasta istunnosta.
    Aloita alusta lataamalla sivu uudelleen.
panic-title = Jokin meni pieleen :(
more-info = Lis\xE4tietoja
run-anyway = Suorita silti
continue = Jatka
report-bug = Ilmoita ongelmasta
update-ruffle = P\xE4ivit\xE4 Ruffle
ruffle-desktop = Ty\xF6p\xF6yt\xE4sovellus
ruffle-wiki = N\xE4yt\xE4 Rufflen wiki
enable-hardware-acceleration = Vaikuttaa silt\xE4, ett\xE4 laitteistokiihdytys on pois k\xE4yt\xF6st\xE4. Ruffle saattaa silti toimia, mutta hitaasti. Lis\xE4tietoja laitteistokiihdytyksen ottamisesta k\xE4ytt\xF6\xF6n on saatavilla alla olevan linkin kautta:
enable-hardware-acceleration-link = UKK - Chromen laitteistokiihdytys
view-error-details = N\xE4yt\xE4 virheen tiedot
open-in-new-tab = Avaa uudessa v\xE4lilehdess\xE4
click-to-unmute = Napsauta palauttaaksesi \xE4\xE4net
clipboard-message-title = Kopiointi ja liitt\xE4minen Rufflessa
clipboard-message-copy = { " " } kopioi
clipboard-message-cut = { " " } leikkaa
clipboard-message-paste = { " " } liitt\xE4\xE4
error-wasm-unsupported-browser =
    K\xE4ytt\xE4m\xE4si selain ei tue Rufflen vaatimia WebAssembly-laajennuksia.
    Vaihda tuettuun selaimeen.
    Lista tuetuista selaimista on koottu wikiin.
`,"save-manager.ftl":`save-delete-prompt = Haluatko varmasti poistaa t\xE4m\xE4n tallennuksen?
save-reload-prompt =
    Ainoa tapa { $action ->
        [delete] poistaa
       *[replace] korvata
    } t\xE4m\xE4 tiedosto ilman mahdollista ristiriitaa on ladata sis\xE4lt\xF6 uudelleen. Haluatko jatkaa silti?
save-download = Lataa
save-replace = Korvaa
save-delete = Poista
`,"volume-controls.ftl":`volume-controls-mute = Mykist\xE4
volume-controls-unmute = Poista mykistys
`},"fr-FR":{"context_menu.ftl":`context-menu-download-swf = T\xE9l\xE9charger en tant que SWF
context-menu-copy-debug-info = Copier les infos de d\xE9bogage
context-menu-open-save-manager = Ouvrir le gestionnaire de stockage
context-menu-about-ruffle =
    { $flavor ->
        [extension] \xC0 propos de l'Extension Ruffle ({ $version })
       *[other] \xC0 propos de Ruffle ({ $version })
    }
context-menu-hide = Masquer ce menu
context-menu-exit-fullscreen = Sortir du mode plein \xE9cran
context-menu-enter-fullscreen = Afficher en plein \xE9cran
context-menu-volume-controls = Contr\xF4les du volume
`,"messages.ftl":`message-cant-embed =
    Ruffle n'a pas \xE9t\xE9 en mesure de lire le fichier Flash int\xE9gr\xE9 dans cette page.
    Vous pouvez essayer d'ouvrir le fichier dans un onglet isol\xE9, pour contourner le probl\xE8me.
message-restored-from-bfcache =
    Votre navigateur a restaur\xE9 ce contenu Flash d'une session ant\xE9rieure.
    Rechargez la page pour repartir de z\xE9ro.
panic-title = Une erreur est survenue :(
more-info = Plus d'infos
run-anyway = Ex\xE9cuter quand m\xEAme
continue = Continuer
report-bug = Signaler le bug
update-ruffle = Mettre \xE0 jour Ruffle
ruffle-demo = D\xE9mo en ligne
ruffle-desktop = Application de bureau
ruffle-wiki = Wiki de Ruffle
enable-hardware-acceleration = Il semblerait que l'acc\xE9l\xE9ration mat\xE9rielle soit d\xE9sactiv\xE9e. Cela n'emp\xEAche g\xE9n\xE9ralement pas Ruffle de fonctionner, mais il peut \xEAtre beaucoup plus lent. Vous pouvez trouver comment activer l'acc\xE9l\xE9ration mat\xE9rielle en suivant le lien ci-dessous :
enable-hardware-acceleration-link = FAQ - Acc\xE9l\xE9ration mat\xE9rielle dans Chrome
view-error-details = D\xE9tails de l'erreur
open-in-new-tab = Ouvrir dans un nouvel onglet
click-to-unmute = Cliquez pour activer le son
clipboard-message-title = Copier et coller dans Ruffle
clipboard-message-description =
    { $variant ->
       *[unsupported] Votre navigateur ne prend pas en charge l'acc\xE8s au presse-papiers,
        [access-denied] L'acc\xE8s au presse-papiers a \xE9t\xE9 refus\xE9,
    } mais vous pouvez toujours utiliser ces raccourcis clavier \xE0 la place :
clipboard-message-copy = { " " } pour copier
clipboard-message-cut = { " " } pour couper
clipboard-message-paste = { " " } pour coller
error-canvas-reload = Impossible de recharger avec le moteur de rendu canvas lorsque celui-ci est d\xE9j\xE0 en cours d'utilisation.
error-file-protocol =
    Il semblerait que vous ex\xE9cutiez Ruffle sur le protocole "file:".
    Cela ne fonctionne pas car les navigateurs bloquent de nombreuses fonctionnalit\xE9s pour des raisons de s\xE9curit\xE9.
    Nous vous invitons soit \xE0 configurer un serveur local, soit \xE0 utiliser la d\xE9mo en ligne ou l'application de bureau.
error-javascript-config =
    Ruffle a rencontr\xE9 un probl\xE8me majeur en raison d'une configuration JavaScript incorrecte.
    Si vous \xEAtes l'administrateur du serveur, nous vous invitons \xE0 v\xE9rifier les d\xE9tails de l'erreur pour savoir quel est le param\xE8tre en cause.
    Vous pouvez \xE9galement consulter le wiki de Ruffle pour obtenir de l'aide.
error-wasm-not-found =
    Ruffle n'a pas r\xE9ussi \xE0 charger son fichier ".wasm".
    Si vous \xEAtes l'administrateur du serveur, veuillez vous assurer que ce fichier a bien \xE9t\xE9 mis en ligne.
    Si le probl\xE8me persiste, il vous faudra peut-\xEAtre utiliser le param\xE8tre "publicPath" : veuillez consulter le wiki de Ruffle pour obtenir de l'aide.
error-wasm-mime-type =
    Ruffle a rencontr\xE9 un probl\xE8me majeur durant sa phase d'initialisation.
    Ce serveur web ne renvoie pas le bon type MIME pour les fichiers ".wasm".
    Si vous \xEAtes l'administrateur du serveur, veuillez consulter le wiki de Ruffle pour obtenir de l'aide.
error-invalid-swf =
    Ruffle n'a pas \xE9t\xE9 en mesure de lire le fichier demand\xE9.
    La raison la plus probable est que ce fichier n'est pas un SWF valide.
error-swf-fetch =
    Ruffle n'a pas r\xE9ussi \xE0 charger le fichier Flash.
    La raison la plus probable est que le fichier n'existe pas ou plus.
    Vous pouvez essayer de prendre contact avec l'administrateur du site pour obtenir plus d'informations.
error-swf-cors =
    Ruffle n'a pas r\xE9ussi \xE0 charger le fichier Flash.
    La requ\xEAte a probablement \xE9t\xE9 rejet\xE9e en raison de la configuration du CORS.
    Si vous \xEAtes l'administrateur du serveur, veuillez consulter le wiki de Ruffle pour obtenir de l'aide.
error-wasm-cors =
    Ruffle n'a pas r\xE9ussi \xE0 charger son fichier ".wasm".
    La requ\xEAte a probablement \xE9t\xE9 rejet\xE9e en raison de la configuration du CORS.
    Si vous \xEAtes l'administrateur du serveur, veuillez consulter le wiki de Ruffle pour obtenir de l'aide.
error-wasm-invalid =
    Ruffle a rencontr\xE9 un probl\xE8me majeur durant sa phase d'initialisation.
    Il semblerait que cette page comporte des fichiers manquants ou invalides pour ex\xE9cuter Ruffle.
    Si vous \xEAtes l'administrateur du serveur, veuillez consulter le wiki de Ruffle pour obtenir de l'aide.
error-wasm-download =
    Ruffle a rencontr\xE9 un probl\xE8me majeur durant sa phase d'initialisation.
    Le probl\xE8me d\xE9tect\xE9 peut souvent se r\xE9soudre de lui-m\xEAme, donc vous pouvez essayer de recharger la page.
    Si le probl\xE8me persiste, veuillez prendre contact avec l'administrateur du site.
error-wasm-disabled-on-edge =
    Ruffle n'a pas r\xE9ussi \xE0 charger son fichier ".wasm".
    Pour r\xE9soudre ce probl\xE8me, essayez d'ouvrir les param\xE8tres de votre navigateur et de cliquer sur "Confidentialit\xE9, recherche et services". Puis, vers le bas de la page, d\xE9sactivez l'option "Am\xE9liorez votre s\xE9curit\xE9 sur le web".
    Cela permettra \xE0 votre navigateur de charger les fichiers ".wasm".
    Si le probl\xE8me persiste, vous devrez peut-\xEAtre utiliser un autre navigateur.
error-wasm-unsupported-browser =
    Votre navigateur ne prend pas en charge les extensions WebAssembly n\xE9cessaires au fonctionnement de Ruffle.
    Veuillez utiliser un navigateur les prenant en charge.
    Vous pouvez trouver une liste de navigateurs fonctionnant avec Ruffle sur le wiki.
error-javascript-conflict =
    Ruffle a rencontr\xE9 un probl\xE8me majeur durant sa phase d'initialisation.
    Il semblerait que cette page contienne du code JavaScript qui entre en conflit avec Ruffle.
    Si vous \xEAtes l'administrateur du serveur, nous vous invitons \xE0 essayer de charger le fichier dans une page vide.
error-javascript-conflict-outdated = Vous pouvez \xE9galement essayer de mettre en ligne une version plus r\xE9cente de Ruffle qui pourrait avoir corrig\xE9 le probl\xE8me (la version que vous utilisez est obsol\xE8te : { $buildDate }).
error-csp-conflict =
    Ruffle a rencontr\xE9 un probl\xE8me majeur durant sa phase d'initialisation.
    La strat\xE9gie de s\xE9curit\xE9 du contenu (CSP) de ce serveur web n'autorise pas l'ex\xE9cution de fichiers ".wasm".
    Si vous \xEAtes l'administrateur du serveur, veuillez consulter le wiki de Ruffle pour obtenir de l'aide.
error-unknown =
    Ruffle a rencontr\xE9 un probl\xE8me majeur durant l'ex\xE9cution de ce contenu Flash.
    { $outdated ->
        [true] Si vous \xEAtes l'administrateur du serveur, veuillez essayer de mettre en ligne une version plus r\xE9cente de Ruffle (la version que vous utilisez est obsol\xE8te : { $buildDate }).
       *[false] Cela n'est pas cens\xE9 se produire, donc nous vous serions reconnaissants si vous pouviez nous signaler ce bug !
    }
`,"save-manager.ftl":`save-delete-prompt = Voulez-vous vraiment supprimer ce fichier de sauvegarde ?
save-reload-prompt =
    La seule fa\xE7on de { $action ->
        [delete] supprimer
       *[replace] remplacer
    } ce fichier de sauvegarde sans conflit potentiel est de recharger ce contenu. Souhaitez-vous quand m\xEAme continuer ?
save-download = T\xE9l\xE9charger
save-replace = Remplacer
save-delete = Supprimer
save-backup-all = T\xE9l\xE9charger tous les fichiers de sauvegarde
`,"volume-controls.ftl":`volume-controls-mute = Rendre muet
volume-controls-unmute = Rendre audible
`},"gl-ES":{"context_menu.ftl":"","messages.ftl":"","save-manager.ftl":"","volume-controls.ftl":""},"he-IL":{"context_menu.ftl":`context-menu-download-swf = \u05D4\u05D5\u05E8\u05D3\u05EA \u05E7\u05D5\u05D1\u05E5 \u05D4SWF
context-menu-copy-debug-info = \u05D4\u05E2\u05EA\u05E7\u05EA \u05E0\u05EA\u05D5\u05E0\u05D9 \u05E0\u05D9\u05E4\u05D5\u05D9 \u05E9\u05D2\u05D9\u05D0\u05D5\u05EA
context-menu-open-save-manager = \u05E4\u05EA\u05D7 \u05D0\u05EA \u05DE\u05E0\u05D4\u05DC \u05D4\u05E9\u05DE\u05D9\u05E8\u05D5\u05EA
context-menu-about-ruffle =
    { $flavor ->
        [extension] \u05D0\u05D5\u05D3\u05D5\u05EA \u05D4\u05EA\u05D5\u05E1\u05E3 Ruffle ({ $version })
       *[other] \u05D0\u05D5\u05D3\u05D5\u05EA Ruffle ({ $version })
    }
context-menu-hide = \u05D4\u05E1\u05EA\u05E8 \u05EA\u05E4\u05E8\u05D9\u05D8 \u05D6\u05D4
context-menu-exit-fullscreen = \u05D9\u05E6\u05D9\u05D0\u05D4 \u05DE\u05DE\u05E1\u05DA \u05DE\u05DC\u05D0
context-menu-enter-fullscreen = \u05DE\u05E1\u05DA \u05DE\u05DC\u05D0
context-menu-volume-controls = \u05D1\u05E7\u05E8\u05EA \u05E2\u05D5\u05E6\u05DE\u05EA \u05E7\u05D5\u05DC
`,"messages.ftl":`message-cant-embed =
    Ruffle \u05DC\u05D0 \u05D4\u05E6\u05DC\u05D9\u05D7 \u05DC\u05D4\u05E8\u05D9\u05E5 \u05D0\u05EA \u05EA\u05D5\u05DB\u05DF \u05D4\u05E4\u05DC\u05D0\u05E9 \u05D4\u05DE\u05D5\u05D8\u05DE\u05E2 \u05D1\u05D3\u05E3 \u05D6\u05D4.
    \u05D0\u05EA\u05D4 \u05D9\u05DB\u05D5\u05DC \u05DC\u05E4\u05EA\u05D5\u05D7 \u05D0\u05EA \u05D4\u05E7\u05D5\u05D1\u05E5 \u05D1\u05DC\u05E9\u05D5\u05E0\u05D9\u05EA \u05E0\u05E4\u05E8\u05D3\u05EA, \u05E2\u05DC \u05DE\u05E0\u05EA \u05DC\u05E2\u05E7\u05D5\u05E3 \u05D1\u05E2\u05D9\u05D4 \u05D6\u05D5.
panic-title = \u05DE\u05E9\u05D4\u05D5 \u05D4\u05E9\u05EA\u05D1\u05E9 :(
more-info = \u05DE\u05D9\u05D3\u05E2 \u05E0\u05D5\u05E1\u05E3
run-anyway = \u05D4\u05E4\u05E2\u05DC \u05D1\u05DB\u05DC \u05D6\u05D0\u05EA
continue = \u05D4\u05DE\u05E9\u05DA
report-bug = \u05D3\u05D5\u05D5\u05D7 \u05E2\u05DC \u05EA\u05E7\u05DC\u05D4
update-ruffle = \u05E2\u05D3\u05DB\u05DF \u05D0\u05EA Ruffle
ruffle-demo = \u05D4\u05D3\u05D2\u05DE\u05D4
ruffle-desktop = \u05D0\u05E4\u05DC\u05D9\u05E7\u05E6\u05D9\u05D9\u05EA \u05E9\u05D5\u05DC\u05D7\u05DF \u05E2\u05D1\u05D5\u05D3\u05D4
ruffle-wiki = \u05E8\u05D0\u05D4 \u05D0\u05EA \u05D5\u05D9\u05E7\u05D9 \u05E9\u05DC Ruffle
enable-hardware-acceleration = \u05E0\u05E8\u05D0\u05D4 \u05E9\u05D4\u05D0\u05E6\u05EA \u05D4\u05D7\u05D5\u05DE\u05E8\u05D4 \u05E9\u05DC\u05DA \u05DC\u05D0 \u05DE\u05D5\u05E4\u05E2\u05DC\u05EA. \u05D1\u05E2\u05D5\u05D3 \u05E9Ruffle \u05E2\u05E9\u05D5\u05D9 \u05DC\u05E2\u05D1\u05D5\u05D3, \u05D4\u05D5\u05D0 \u05D9\u05DB\u05D5\u05DC \u05DC\u05D4\u05D9\u05D5\u05EA \u05D0\u05D9\u05D8\u05D9. \u05EA\u05D5\u05DB\u05DC \u05DC\u05E8\u05D0\u05D5\u05EA \u05DB\u05D9\u05E6\u05D3 \u05DC\u05D4\u05E4\u05E2\u05D9\u05DC \u05EA\u05DB\u05D5\u05E0\u05D4 \u05D6\u05D5 \u05D1\u05DC\u05D7\u05D9\u05E6\u05D4 \u05E2\u05DC \u05D4\u05DC\u05D9\u05E0\u05E7 \u05D4\u05D6\u05D4:
enable-hardware-acceleration-link = \u05E9\u05D0\u05DC\u05D5\u05EA \u05E0\u05E4\u05D5\u05E6\u05D5\u05EA - \u05D4\u05D0\u05E6\u05EA \u05D4\u05D7\u05D5\u05DE\u05E8\u05D4 \u05E9\u05DC Chrome
view-error-details = \u05E8\u05D0\u05D4 \u05E4\u05E8\u05D8\u05D9 \u05E9\u05D2\u05D9\u05D0\u05D4
open-in-new-tab = \u05E4\u05EA\u05D7 \u05D1\u05DB\u05E8\u05D8\u05D9\u05E1\u05D9\u05D9\u05D4 \u05D7\u05D3\u05E9\u05D4
click-to-unmute = \u05DC\u05D7\u05E5 \u05E2\u05DC \u05DE\u05E0\u05EA \u05DC\u05D1\u05D8\u05DC \u05D4\u05E9\u05EA\u05E7\u05D4
clipboard-message-title = \u05D4\u05E2\u05EA\u05E7\u05D4 \u05D5\u05D4\u05D3\u05D1\u05E7\u05D4 \u05D1Ruffle
clipboard-message-copy = { " " } \u05E2\u05D1\u05D5\u05E8 \u05D4\u05E2\u05EA\u05E7\u05D4
clipboard-message-cut = { " " } \u05E2\u05D1\u05D5\u05E8 \u05D2\u05D6\u05D9\u05E8\u05D4
clipboard-message-paste = { " " } \u05E2\u05D1\u05D5\u05E8 \u05D4\u05D3\u05D1\u05E7\u05D4
error-canvas-reload = \u05DC\u05D0 \u05E0\u05D9\u05EA\u05DF \u05DC\u05D8\u05E2\u05D5\u05DF \u05DE\u05D7\u05D3\u05E9 \u05E2\u05DD \u05DE\u05E2\u05D1\u05D3 \u05D4\u05E7\u05E0\u05D1\u05E1 \u05DB\u05D0\u05E9\u05E8 \u05DE\u05E2\u05D1\u05D3 \u05D4\u05E7\u05E0\u05D1\u05E1 \u05DB\u05D1\u05E8 \u05D1\u05E9\u05D9\u05DE\u05D5\u05E9.
error-file-protocol =
    \u05E0\u05D3\u05DE\u05D4 \u05E9\u05D0\u05EA\u05D4 \u05DE\u05E8\u05D9\u05E5 \u05D0\u05EA Ruffle \u05EA\u05D7\u05EA \u05E4\u05E8\u05D5\u05D8\u05D5\u05E7\u05D5\u05DC "file:".
    \u05D6\u05D4 \u05DC\u05D0 \u05D9\u05E2\u05D1\u05D5\u05D3 \u05DE\u05DB\u05D9\u05D5\u05D5\u05DF \u05E9\u05D3\u05E4\u05D3\u05E4\u05E0\u05D9\u05DD \u05D7\u05D5\u05E1\u05DE\u05D9\u05DD \u05D0\u05E4\u05E9\u05E8\u05D5\u05D9\u05D5\u05EA \u05E8\u05D1\u05D5\u05EA \u05DE\u05DC\u05E2\u05D1\u05D5\u05D3 \u05E2\u05E7\u05D1 \u05E1\u05D9\u05D1\u05D5\u05EA \u05D0\u05D1\u05D8\u05D7\u05D4.
    \u05D1\u05DE\u05E7\u05D5\u05DD \u05D6\u05D4, \u05D0\u05E0\u05D5 \u05DE\u05D6\u05DE\u05D9\u05E0\u05D9\u05DD \u05D0\u05D5\u05EA\u05DA \u05DC\u05D0\u05D7\u05E1\u05DF \u05D0\u05EA\u05E8 \u05D6\u05D4 \u05EA\u05D7\u05EA \u05E9\u05E8\u05EA \u05DE\u05E7\u05D5\u05DE\u05D9 \u05D0\u05D5 \u05D4\u05D3\u05D2\u05DE\u05D4 \u05D1\u05E8\u05E9\u05EA \u05D0\u05D5 \u05D3\u05E8\u05DA \u05D0\u05E4\u05DC\u05D9\u05E7\u05E6\u05D9\u05D9\u05EA \u05E9\u05D5\u05DC\u05D7\u05DF \u05D4\u05E2\u05D1\u05D5\u05D3\u05D4.
error-javascript-config =
    Ruffle \u05E0\u05EA\u05E7\u05DC \u05D1\u05EA\u05E7\u05DC\u05D4 \u05D7\u05DE\u05D5\u05E8\u05D4 \u05E2\u05E7\u05D1 \u05D4\u05D2\u05D3\u05E8\u05EA JavaScript \u05E9\u05D2\u05D5\u05D9\u05D4.
    \u05D0\u05DD \u05D0\u05EA\u05D4 \u05DE\u05E0\u05D4\u05DC \u05D4\u05D0\u05EA\u05E8, \u05D0\u05E0\u05D5 \u05DE\u05D6\u05DE\u05D9\u05E0\u05D9\u05DD \u05D0\u05D5\u05EA\u05DA \u05DC\u05D1\u05D3\u05D5\u05E7 \u05D0\u05EA \u05E4\u05E8\u05D8\u05D9 \u05D4\u05E9\u05D2\u05D9\u05D0\u05D4 \u05E2\u05DC \u05DE\u05E0\u05EA \u05DC\u05DE\u05E6\u05D5\u05D0 \u05D0\u05D9\u05D6\u05D4 \u05E4\u05E8\u05DE\u05D8\u05E8 \u05D4\u05D5\u05D0 \u05E9\u05D2\u05D5\u05D9.
    \u05D0\u05EA\u05D4 \u05D9\u05DB\u05D5\u05DC \u05DC\u05E2\u05D9\u05D9\u05DF \u05D5\u05DC\u05D4\u05D5\u05E2\u05E5 \u05D1wiki \u05E9\u05DC Ruffle \u05E2\u05DC \u05DE\u05E0\u05EA \u05DC\u05E7\u05D1\u05DC \u05E2\u05D6\u05E8\u05D4.
error-wasm-not-found =
    Ruffle \u05E0\u05DB\u05E9\u05DC \u05DC\u05D8\u05E2\u05D5\u05DF \u05D0\u05EA \u05E7\u05D5\u05D1\u05E5 \u05D4"wasm." \u05D4\u05D3\u05E8\u05D5\u05E9.
    \u05D0\u05DD \u05D0\u05EA\u05D4 \u05DE\u05E0\u05D4\u05DC \u05D4\u05D0\u05EA\u05E8, \u05D0\u05E0\u05D0 \u05D5\u05D5\u05D3\u05D0 \u05DB\u05D9 \u05D4\u05E7\u05D5\u05D1\u05E5 \u05D4\u05D5\u05E2\u05DC\u05D4 \u05DB\u05E9\u05D5\u05E8\u05D4.
    \u05D0\u05DD \u05D4\u05D1\u05E2\u05D9\u05D4 \u05DE\u05DE\u05E9\u05D9\u05DB\u05D4, \u05D9\u05D9\u05EA\u05DB\u05DF \u05D5\u05EA\u05E6\u05D8\u05E8\u05DA \u05DC\u05D4\u05E9\u05EA\u05DE\u05E9 \u05D1\u05D4\u05D2\u05D3\u05E8\u05EA "publicPath": \u05D0\u05E0\u05D0 \u05E2\u05D9\u05D9\u05DF \u05D5\u05D4\u05D5\u05E2\u05E5 \u05D1wiki \u05E9\u05DC Ruffle \u05E2\u05DC \u05DE\u05E0\u05EA \u05DC\u05E7\u05D1\u05DC \u05E2\u05D6\u05E8\u05D4.
error-wasm-mime-type =
    Ruffle \u05E0\u05EA\u05E7\u05DC \u05D1\u05D1\u05E2\u05D9\u05D4 \u05D7\u05DE\u05D5\u05E8\u05D4 \u05EA\u05D5\u05DA \u05DB\u05D3\u05D9 \u05E0\u05D9\u05E1\u05D9\u05D5\u05DF \u05DC\u05D0\u05EA\u05D7\u05DC.
    \u05E9\u05E8\u05EA\u05D5 \u05E9\u05DC \u05D0\u05EA\u05E8 \u05D6\u05D4 \u05DC\u05D0 \u05DE\u05E9\u05D9\u05D9\u05DA \u05E7\u05D1\u05E6\u05D9 ".wasm" \u05E2\u05DD \u05E1\u05D5\u05D2 \u05D4MIME \u05D4\u05E0\u05DB\u05D5\u05DF.
    \u05D0\u05DD \u05D0\u05EA\u05D4 \u05DE\u05E0\u05D4\u05DC \u05D4\u05D0\u05EA\u05E8, \u05D0\u05E0\u05D0 \u05E2\u05D9\u05D9\u05DF \u05D5\u05D4\u05D5\u05E2\u05E5 \u05D1wiki \u05E9\u05DC Ruffle \u05E2\u05DC \u05DE\u05E0\u05EA \u05DC\u05E7\u05D1\u05DC \u05E2\u05D6\u05E8\u05D4.
error-invalid-swf =
    Ruffle \u05DC\u05D0 \u05D9\u05DB\u05D5\u05DC \u05DC\u05E0\u05EA\u05D7 \u05D0\u05EA \u05D4\u05E7\u05D5\u05D1\u05E5 \u05D4\u05DE\u05D1\u05D5\u05E7\u05E9.
    \u05D4\u05E1\u05D9\u05D1\u05D4 \u05D4\u05E1\u05D1\u05D9\u05E8\u05D4 \u05D1\u05D9\u05D5\u05EA\u05E8 \u05DC\u05D1\u05E2\u05D9\u05D4 \u05D6\u05D5 \u05D4\u05D9\u05D0 \u05D1\u05D2\u05DC\u05DC \u05E9\u05D4\u05E7\u05D5\u05D1\u05E5 \u05D4\u05DE\u05D1\u05D5\u05E7\u05E9 \u05D0\u05D9\u05E0\u05D5 SWF \u05D7\u05D5\u05E7\u05D9.
error-swf-fetch =
    Ruffle \u05E0\u05DB\u05E9\u05DC \u05DC\u05D8\u05E2\u05D5\u05DF \u05D0\u05EA \u05E7\u05D5\u05D1\u05E5 \u05D4\u05E4\u05DC\u05D0\u05E9/swf. .
    \u05D6\u05D4 \u05E0\u05D5\u05D1\u05E2 \u05DB\u05DB\u05DC \u05D4\u05E0\u05E8\u05D0\u05D4 \u05DE\u05DB\u05D9\u05D5\u05D5\u05DF \u05D5\u05D4\u05E7\u05D5\u05D1\u05E5 \u05DC\u05D0 \u05E7\u05D9\u05D9\u05DD \u05D9\u05D5\u05EA\u05E8, \u05D0\u05D6 \u05D0\u05D9\u05DF \u05DCRuffle \u05DE\u05D4 \u05DC\u05D8\u05E2\u05D5\u05DF.
    \u05E0\u05E1\u05D4 \u05DC\u05D9\u05E6\u05D5\u05E8 \u05E7\u05E9\u05E8 \u05E2\u05DD \u05DE\u05E0\u05D4\u05DC \u05D4\u05D0\u05EA\u05E8 \u05E2\u05DC \u05DE\u05E0\u05EA \u05DC\u05E7\u05D1\u05DC \u05E2\u05D6\u05E8\u05D4.
error-swf-cors =
    Ruffle \u05E0\u05DB\u05E9\u05DC \u05DC\u05D8\u05E2\u05D5\u05DF \u05D0\u05EA \u05E7\u05D5\u05D1\u05E5 \u05D4\u05E4\u05DC\u05D0\u05E9/swf. .
    \u05D2\u05D9\u05E9\u05D4 \u05DCfetch \u05DB\u05DB\u05DC \u05D4\u05E0\u05E8\u05D0\u05D4 \u05E0\u05D7\u05E1\u05DE\u05D4 \u05E2\u05DC \u05D9\u05D3\u05D9 \u05DE\u05D3\u05D9\u05E0\u05D9\u05D5\u05EA CORS.
    \u05D0\u05DD \u05D0\u05EA\u05D4 \u05DE\u05E0\u05D4\u05DC \u05D4\u05D0\u05EA\u05E8, \u05D0\u05E0\u05D0 \u05E2\u05D9\u05D9\u05DF \u05D5\u05D4\u05D5\u05E2\u05E5 \u05D1wiki \u05E9\u05DC Ruffle \u05E2\u05DC \u05DE\u05E0\u05EA \u05DC\u05E7\u05D1\u05DC \u05E2\u05D6\u05E8\u05D4.
error-wasm-cors =
    Ruffle \u05E0\u05DB\u05E9\u05DC \u05DC\u05D8\u05E2\u05D5\u05DF \u05D0\u05EA \u05E7\u05D5\u05D1\u05E5 \u05D4".wasm" \u05D4\u05D3\u05E8\u05D5\u05E9.
    \u05D2\u05D9\u05E9\u05D4 \u05DCfetch \u05DB\u05DB\u05DC \u05D4\u05E0\u05E8\u05D0\u05D4 \u05E0\u05D7\u05E1\u05DE\u05D4 \u05E2\u05DC \u05D9\u05D3\u05D9 \u05DE\u05D3\u05D9\u05E0\u05D9\u05D5\u05EA CORS.
    \u05D0\u05DD \u05D0\u05EA\u05D4 \u05DE\u05E0\u05D4\u05DC \u05D4\u05D0\u05EA\u05E8, \u05D0\u05E0\u05D0 \u05E2\u05D9\u05D9\u05DF \u05D5\u05D4\u05D5\u05E2\u05E5 \u05D1wiki \u05E9\u05DC Ruffle \u05E2\u05DC \u05DE\u05E0\u05EA \u05DC\u05E7\u05D1\u05DC \u05E2\u05D6\u05E8\u05D4.
error-wasm-invalid =
    Ruffle \u05E0\u05EA\u05E7\u05DC \u05D1\u05D1\u05E2\u05D9\u05D4 \u05D7\u05DE\u05D5\u05E8\u05D4 \u05EA\u05D5\u05DA \u05DB\u05D3\u05D9 \u05E0\u05D9\u05E1\u05D9\u05D5\u05DF \u05DC\u05D0\u05EA\u05D7\u05DC.
    \u05E0\u05D3\u05DE\u05D4 \u05DB\u05D9 \u05D1\u05D3\u05E3 \u05D6\u05D4 \u05D7\u05E1\u05E8\u05D9\u05DD \u05D0\u05D5 \u05DC\u05D0 \u05E2\u05D5\u05D1\u05D3\u05D9\u05DD \u05DB\u05E8\u05D0\u05D5\u05D9 \u05E7\u05D1\u05E6\u05D9\u05DD \u05D0\u05E9\u05E8 \u05DE\u05E9\u05DE\u05E9\u05D9\u05DD \u05D0\u05EA Ruffle \u05DB\u05D3\u05D9 \u05DC\u05E4\u05E2\u05D5\u05DC
    \u05D0\u05DD \u05D0\u05EA\u05D4 \u05DE\u05E0\u05D4\u05DC \u05D4\u05D0\u05EA\u05E8, \u05D0\u05E0\u05D0 \u05E2\u05D9\u05D9\u05DF \u05D5\u05D4\u05D5\u05E2\u05E5 \u05D1wiki \u05E9\u05DC Ruffle \u05E2\u05DC \u05DE\u05E0\u05EA \u05DC\u05E7\u05D1\u05DC \u05E2\u05D6\u05E8\u05D4.
error-wasm-download =
    Ruffle \u05E0\u05EA\u05E7\u05DC \u05D1\u05D1\u05E2\u05D9\u05D4 \u05D7\u05DE\u05D5\u05E8\u05D4 \u05EA\u05D5\u05DA \u05DB\u05D3\u05D9 \u05E0\u05D9\u05E1\u05D9\u05D5\u05DF \u05DC\u05D0\u05EA\u05D7\u05DC.
    \u05DC\u05E2\u05D9\u05EA\u05D9\u05DD \u05D1\u05E2\u05D9\u05D4 \u05D6\u05D5 \u05D9\u05DB\u05D5\u05DC\u05D4 \u05DC\u05E4\u05EA\u05D5\u05E8 \u05D0\u05EA \u05E2\u05E6\u05DE\u05D4, \u05D0\u05D6 \u05D0\u05EA\u05D4 \u05D9\u05DB\u05D5\u05DC \u05DC\u05E0\u05E1\u05D5\u05EA \u05DC\u05D8\u05E2\u05D5\u05DF \u05DE\u05D7\u05D3\u05E9 \u05D0\u05EA \u05D4\u05D3\u05E3 \u05D6\u05D4.
    \u05D0\u05DD \u05DC\u05D0, \u05D0\u05E0\u05D0 \u05E4\u05E0\u05D4 \u05DC\u05DE\u05E0\u05D4\u05DC \u05D4\u05D0\u05EA\u05E8.
error-wasm-disabled-on-edge =
    Ruffle \u05E0\u05DB\u05E9\u05DC \u05DC\u05D8\u05E2\u05D5\u05DF \u05D0\u05EA \u05E7\u05D5\u05D1\u05E5 \u05D4".wasm" \u05D4\u05D3\u05E8\u05D5\u05E9.
    \u05E2\u05DC \u05DE\u05E0\u05EA \u05DC\u05EA\u05E7\u05DF \u05D1\u05E2\u05D9\u05D4 \u05D6\u05D5, \u05E0\u05E1\u05D4 \u05DC\u05E4\u05EA\u05D5\u05D7 \u05D0\u05EA \u05D4\u05D2\u05D3\u05E8\u05D5\u05EA \u05D4\u05D3\u05E4\u05D3\u05E4\u05DF \u05E9\u05DC\u05DA, \u05DC\u05D7\u05E5 \u05E2\u05DC "\u05D0\u05D1\u05D8\u05D7\u05D4, \u05D7\u05D9\u05E4\u05D5\u05E9 \u05D5\u05E9\u05D9\u05E8\u05D5\u05EA",
    \u05D2\u05DC\u05D5\u05DC \u05DE\u05D8\u05D4, \u05D5\u05DB\u05D1\u05D4 \u05D0\u05EA "\u05D4\u05D2\u05D1\u05E8 \u05D0\u05EA \u05D4\u05D0\u05D1\u05D8\u05D7\u05D4 \u05E9\u05DC\u05DA \u05D1\u05E8\u05E9\u05EA".
    \u05D6\u05D4 \u05D9\u05D0\u05E4\u05E9\u05E8 \u05DC\u05D3\u05E4\u05D3\u05E4\u05DF \u05E9\u05DC\u05DA \u05DC\u05D8\u05E2\u05D5\u05DF \u05D0\u05EA \u05E7\u05D5\u05D1\u05E5 \u05D4".wasm" \u05D4\u05D3\u05E8\u05D5\u05E9.
    \u05D0\u05DD \u05D4\u05D1\u05E2\u05D9\u05D4 \u05DE\u05DE\u05E9\u05D9\u05DB\u05D4, \u05D9\u05D9\u05EA\u05DB\u05DF \u05D5\u05E2\u05DC\u05D9\u05DA \u05DC\u05D4\u05E9\u05EA\u05DE\u05E9 \u05D1\u05D3\u05E4\u05D3\u05E4\u05DF \u05D0\u05D7\u05E8.
error-wasm-unsupported-browser =
    \u05D4\u05D3\u05E4\u05D3\u05E4\u05DF \u05E9\u05D1\u05D5 \u05D0\u05EA\u05D4 \u05DE\u05E9\u05EA\u05DE\u05E9 \u05D0\u05D9\u05E0\u05D5 \u05EA\u05D5\u05DE\u05DA \u05D1\u05EA\u05D5\u05E1\u05E4\u05D9 WebAssembly \u05E9-Ruffle \u05D3\u05D5\u05E8\u05E9 \u05DB\u05D3\u05D9 \u05DC\u05E4\u05E2\u05D5\u05DC.
    \u05D0\u05E0\u05D0 \u05E2\u05D1\u05D5\u05E8 \u05DC\u05D3\u05E4\u05D3\u05E4\u05DF \u05E0\u05EA\u05DE\u05DA.
    \u05D0\u05EA\u05D4 \u05D9\u05DB\u05D5\u05DC \u05DC\u05DE\u05E6\u05D5\u05D0 \u05E8\u05E9\u05D9\u05DE\u05D4 \u05E9\u05DC \u05D3\u05E4\u05D3\u05E4\u05E0\u05D9\u05DD \u05E0\u05EA\u05DE\u05DB\u05D9\u05DD \u05D1-Wiki \u05E9\u05DC\u05E0\u05D5.
error-javascript-conflict =
    Ruffle \u05E0\u05EA\u05E7\u05DC \u05D1\u05D1\u05E2\u05D9\u05D4 \u05D7\u05DE\u05D5\u05E8\u05D4 \u05EA\u05D5\u05DA \u05DB\u05D3\u05D9 \u05E0\u05D9\u05E1\u05D9\u05D5\u05DF \u05DC\u05D0\u05EA\u05D7\u05DC.
    \u05E0\u05D3\u05DE\u05D4 \u05DB\u05D9 \u05D3\u05E3 \u05D6\u05D4 \u05DE\u05E9\u05EA\u05DE\u05E9 \u05D1\u05E7\u05D5\u05D3 JavaScript \u05D0\u05E9\u05E8 \u05DE\u05EA\u05E0\u05D2\u05E9 \u05E2\u05DD Ruffle.
    \u05D0\u05DD \u05D0\u05EA\u05D4 \u05DE\u05E0\u05D4\u05DC \u05D4\u05D0\u05EA\u05E8, \u05D0\u05E0\u05D5 \u05DE\u05D6\u05DE\u05D9\u05E0\u05D9\u05DD \u05D0\u05D5\u05EA\u05DA \u05DC\u05E0\u05E1\u05D5\u05EA \u05DC\u05D8\u05E2\u05D5\u05DF \u05D0\u05EA \u05D4\u05D3\u05E3 \u05EA\u05D7\u05EA \u05E2\u05DE\u05D5\u05D3 \u05E8\u05D9\u05E7.
error-javascript-conflict-outdated = \u05D1\u05E0\u05D5\u05E1\u05E3, \u05D0\u05EA\u05D4 \u05D9\u05DB\u05D5\u05DC \u05DC\u05E0\u05E1\u05D5\u05EA \u05D5\u05DC\u05D4\u05E2\u05DC\u05D5\u05EA \u05D2\u05E8\u05E1\u05D0\u05D5\u05EA \u05E2\u05D3\u05DB\u05E0\u05D9\u05D5\u05EA \u05E9\u05DC Ruffle \u05D0\u05E9\u05E8 \u05E2\u05DC\u05D5\u05DC\u05D9\u05DD \u05DC\u05E2\u05E7\u05D5\u05E3 \u05D1\u05E2\u05D9\u05D4 \u05D6\u05D5 (\u05D2\u05E8\u05E1\u05D4 \u05D6\u05D5 \u05D4\u05D9\u05E0\u05D4 \u05DE\u05D9\u05D5\u05E9\u05E0\u05EA : { $buildDate }).
error-csp-conflict =
    Ruffle \u05E0\u05EA\u05E7\u05DC \u05D1\u05D1\u05E2\u05D9\u05D4 \u05D7\u05DE\u05D5\u05E8\u05D4 \u05EA\u05D5\u05DA \u05DB\u05D3\u05D9 \u05E0\u05D9\u05E1\u05D9\u05D5\u05DF \u05DC\u05D0\u05EA\u05D7\u05DC.
    \u05DE\u05D3\u05D9\u05E0\u05D9\u05D5\u05EA \u05D0\u05D1\u05D8\u05D7\u05EA \u05D4\u05EA\u05D5\u05DB\u05DF \u05E9\u05DC \u05E9\u05E8\u05EA\u05D5 \u05E9\u05DC \u05D0\u05EA\u05E8 \u05D6\u05D4 \u05D0\u05D9\u05E0\u05D4 \u05DE\u05D0\u05E4\u05E9\u05E8\u05EA \u05DC\u05E7\u05D5\u05D1\u05E5 \u05D4"wasm." \u05D4\u05D3\u05E8\u05D5\u05E9 \u05DC\u05E4\u05E2\u05D5\u05DC.
    \u05D0\u05DD \u05D0\u05EA\u05D4 \u05DE\u05E0\u05D4\u05DC \u05D4\u05D0\u05EA\u05E8, \u05D0\u05E0\u05D0 \u05E2\u05D9\u05D9\u05DF \u05D5\u05D4\u05D5\u05E2\u05E5 \u05D1wiki \u05E9\u05DC Ruffle \u05E2\u05DC \u05DE\u05E0\u05EA \u05DC\u05E7\u05D1\u05DC \u05E2\u05D6\u05E8\u05D4.
error-unknown =
    Ruffle \u05E0\u05EA\u05E7\u05DC \u05D1\u05D1\u05E2\u05D9\u05D4 \u05D7\u05DE\u05D5\u05E8\u05D4 \u05D1\u05E0\u05D9\u05E1\u05D9\u05D5\u05DF \u05DC\u05D4\u05E6\u05D9\u05D2 \u05D0\u05EA \u05EA\u05D5\u05DB\u05DF \u05E4\u05DC\u05D0\u05E9 \u05D6\u05D4.
    { $outdated ->
        [true] \u05D0\u05DD \u05D0\u05EA\u05D4 \u05DE\u05E0\u05D4\u05DC \u05D4\u05D0\u05EA\u05E8, \u05D0\u05E0\u05D0 \u05E0\u05E1\u05D4 \u05DC\u05D4\u05E2\u05DC\u05D5\u05EA \u05D2\u05E8\u05E1\u05D4 \u05E2\u05D3\u05DB\u05E0\u05D9\u05EA \u05D9\u05D5\u05EA\u05E8 \u05E9\u05DC Ruffle (\u05D2\u05E8\u05E1\u05D4 \u05D6\u05D5 \u05D4\u05D9\u05E0\u05D4 \u05DE\u05D9\u05D5\u05E9\u05E0\u05EA:  { $buildDate }).
       *[false] \u05D6\u05D4 \u05DC\u05D0 \u05D0\u05DE\u05D5\u05E8 \u05DC\u05E7\u05E8\u05D5\u05EA, \u05E0\u05E9\u05DE\u05D7 \u05D0\u05DD \u05EA\u05D5\u05DB\u05DC \u05DC\u05E9\u05EA\u05E3 \u05EA\u05E7\u05DC\u05D4 \u05D6\u05D5!
    }
`,"save-manager.ftl":`save-delete-prompt = \u05D4\u05D0\u05DD \u05D0\u05EA\u05D4 \u05D1\u05D8\u05D5\u05D7 \u05E9\u05D1\u05E8\u05E6\u05D5\u05E0\u05DA \u05DC\u05DE\u05D7\u05D5\u05E7 \u05D0\u05EA \u05E7\u05D5\u05D1\u05E5 \u05E9\u05DE\u05D9\u05E8\u05D4 \u05D6\u05D4?
save-reload-prompt =
    \u05D4\u05D3\u05E8\u05DA \u05D4\u05D9\u05D7\u05D9\u05D3\u05D4 { $action ->
        [delete] \u05DC\u05DE\u05D7\u05D5\u05E7
       *[replace] \u05DC\u05D4\u05D7\u05DC\u05D9\u05E3
    } \u05D0\u05EA \u05E7\u05D5\u05D1\u05E5 \u05D4\u05E9\u05DE\u05D9\u05E8\u05D4 \u05D4\u05D6\u05D4 \u05DE\u05D1\u05DC\u05D9 \u05DC\u05D2\u05E8\u05D5\u05DD \u05DC\u05D5 \u05DC\u05D4\u05EA\u05E0\u05D2\u05E9 \u05D4\u05D9\u05D0 \u05DC\u05D8\u05E2\u05D5\u05DF \u05DE\u05D7\u05D3\u05E9 \u05D0\u05EA \u05EA\u05D5\u05DB\u05DF \u05D6\u05D4. \u05D4\u05D0\u05DD \u05D0\u05EA\u05D4 \u05E8\u05D5\u05E6\u05D4 \u05DC\u05D4\u05DE\u05E9\u05D9\u05DA \u05D1\u05DB\u05DC \u05D6\u05D0\u05EA?
save-download = \u05D4\u05D5\u05E8\u05D3\u05D4
save-replace = \u05D4\u05D7\u05DC\u05E4\u05D4
save-delete = \u05DE\u05D7\u05D9\u05E7\u05D4
save-backup-all = \u05D4\u05D5\u05E8\u05D3\u05EA \u05DB\u05DC \u05E7\u05D1\u05E6\u05D9 \u05D4\u05E9\u05DE\u05D9\u05E8\u05D4
`,"volume-controls.ftl":`volume-controls-mute = \u05D4\u05E9\u05EA\u05E7
volume-controls-unmute = \u05D1\u05D9\u05D8\u05D5\u05DC \u05D4\u05E9\u05EA\u05E7\u05D4
`},"hr-HR":{"context_menu.ftl":`context-menu-download-swf = Preuzmi SWF datoteku
context-menu-copy-debug-info = Kopiraj informacije o otklanjanju pogre\u0161aka
context-menu-open-save-manager = Otvori Upravitelj spremanja
context-menu-about-ruffle =
    { $flavor ->
    [extension] O pro\u0161irenju Ruffle ({ $version })
    *[other] O Ruffle ({ $version })
    }
context-menu-hide = Sakrij ovaj izbornik
context-menu-exit-fullscreen = Iza\u0111i iz cijelog zaslona
context-menu-enter-fullscreen = U\u0111i u cijeli zaslon
context-menu-volume-controls = Kontrole glasno\u0107e
`,"messages.ftl":`message-cant-embed =
    Ruffle nije uspio pokrenuti Flash ugra\u0111en na ovoj stranici.
    Mo\u017Eete poku\u0161ati otvoriti datoteku u zasebnoj kartici kako biste izbjegli ovaj problem.
message-restored-from-bfcache =
    Va\u0161 je preglednik vratio ovaj Flash sadr\u017Eaj iz prethodne sesije.
    Za novi po\u010Detak ponovno u\u010Ditajte stranicu.
panic-title = Ne\u0161to je po\u0161lo po zlu :(
more-info = Dodatne informacije
run-anyway = Svejedno pokreni
continue = Nastavi
report-bug = Prijavi gre\u0161ku
update-ruffle = A\u017Eurirajte Ruffle
ruffle-demo = Web demo
ruffle-desktop = Aplikacija za stolna ra\u010Dunala
ruffle-wiki = Pogledajte Ruffle Wiki
enable-hardware-acceleration = Izgleda da je hardversko ubrzanje onemogu\u0107eno. Iako Ruffle mo\u017Eda radi, mogao bi biti vrlo spor. Kako omogu\u0107iti hardversko ubrzanje mo\u017Eete saznati slijede\u0107i donju poveznicu:
enable-hardware-acceleration-link = \u010Cesto postavljana pitanja - Ubrzanje hardvera u Chromeu
view-error-details = Prika\u017Ei detalje o pogre\u0161ci
open-in-new-tab = Otvori u novoj kartici
click-to-unmute = Kliknite za uklju\u010Divanje zvuka
clipboard-message-title = Kopiranje i lijepljenje u Ruffleu
clipboard-message-description =
    { $variant ->
       *[unsupported] Va\u0161 preglednik ne podr\u017Eava puni pristup me\u0111uspremniku,
        [access-denied] Pristup me\u0111uspremniku je uskra\u0107en,
    } ali uvijek mo\u017Eete umjesto toga koristiti ove pre\u010Dace:
clipboard-message-copy = { " " } za kopiranje
clipboard-message-cut = { " " } za izrezivanje
clipboard-message-paste = { " " } za lijepljenje
error-canvas-reload = Nije mogu\u0107e ponovno u\u010Ditavanje s rendererom platna kada je renderer platna ve\u0107 u upotrebi.
error-file-protocol =
    \u010Cini se da koristite Ruffle na protokolu "file:".
    Ovo ne radi jer preglednici blokiraju mnoge zna\u010Dajke iz sigurnosnih razloga.
    Umjesto toga, pozivamo vas da postavite lokalni poslu\u017Eitelj ili koristite web demo ili desktop aplikaciju.
error-javascript-config =
    Ruffle je nai\u0161ao na veliki problem zbog neto\u010Dne konfiguracije JavaScripta.
    Ako ste administrator poslu\u017Eitelja, pozivamo vas da provjerite detalje pogre\u0161ke kako biste saznali koji je parametar uzrok problema. Tako\u0111er mo\u017Eete konzultirati Ruffle wiki za pomo\u0107.
error-wasm-not-found =
    Ruffle nije uspio u\u010Ditati potrebnu komponentu datoteke ".wasm".
    Ako ste administrator poslu\u017Eitelja, provjerite je li datoteka ispravno prenesena.
    Ako se problem nastavi, mo\u017Eda \u0107ete morati upotrijebiti postavku "publicPath": za pomo\u0107 se obratite Ruffle wikiju.
error-wasm-mime-type =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja inicijalizacije.
    Ovaj web poslu\u017Eitelj ne poslu\u017Euje ".wasm" datoteke s ispravnom MIME vrstom.
    Ako ste administrator poslu\u017Eitelja, obratite se Ruffle wiki stranici za pomo\u0107.
error-invalid-swf =
    Ruffle ne mo\u017Ee analizirati tra\u017Eenu datoteku.
    Najvjerojatniji razlog je taj \u0161to tra\u017Eena datoteka nije valjani SWF.
error-swf-fetch =
    Ruffle nije uspio u\u010Ditati Flash SWF datoteku.
    Najvjerojatniji razlog je taj \u0161to datoteka vi\u0161e ne postoji, pa Ruffle nema \u0161to u\u010Ditati.
    Poku\u0161ajte se obratiti administratoru web-mjesta za pomo\u0107.
error-swf-cors =
    Ruffle nije uspio u\u010Ditati Flash SWF datoteku.
    Pristup dohva\u0107anju vjerojatno je blokiran pravilom CORS.
    Ako ste administrator poslu\u017Eitelja, za pomo\u0107 se obratite Ruffle wikiju.
error-wasm-cors =
    Ruffle nije uspio u\u010Ditati potrebnu komponentu datoteke ".wasm".
    Pristup dohva\u0107anju vjerojatno je blokiran CORS pravilom.
    Ako ste administrator poslu\u017Eitelja, za pomo\u0107 se obratite Ruffle wikiju.
error-wasm-invalid =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja inicijalizacije.
    \u010Cini se da ovoj stranici nedostaju ili su datoteke neva\u017Ee\u0107e za pokretanje Rufflea.
    Ako ste administrator poslu\u017Eitelja, za pomo\u0107 se obratite Ruffle wikiju.
error-wasm-download =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja inicijalizacije.
    To se \u010Desto mo\u017Ee samo rije\u0161iti, pa mo\u017Eete poku\u0161ati ponovno u\u010Ditati stranicu.
    U suprotnom, obratite se administratoru web-mjesta.
error-wasm-disabled-on-edge =
    Ruffle nije uspio u\u010Ditati potrebnu komponentu datoteke ".wasm".
    Da biste to rije\u0161ili, poku\u0161ajte otvoriti postavke preglednika, kliknuti "Privatnost, pretra\u017Eivanje i usluge", pomaknuti se prema dolje i isklju\u010Diti "Pobolj\u0161ajte sigurnost na webu".
    To \u0107e omogu\u0107iti va\u0161em pregledniku da u\u010Dita potrebne datoteke ".wasm".
    Ako se problem nastavi, mo\u017Eda \u0107ete morati koristiti drugi preglednik.
error-wasm-unsupported-browser =
    Preglednik koji koristite ne podr\u017Eava WebAssembly ekstenzije koje su potrebne za rad Rufflea.
    Molimo prebacite se na podr\u017Eani preglednik.
    Popis podr\u017Eanih preglednika mo\u017Eete prona\u0107i na Wiki stranici.
error-javascript-conflict =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja inicijalizacije.
    \u010Cini se da ova stranica koristi JavaScript kod koji je u sukobu s Ruffleom.
    Ako ste administrator poslu\u017Eitelja, pozivamo vas da poku\u0161ate u\u010Ditati datoteku na praznoj stranici.
error-javascript-conflict-outdated = Tako\u0111er mo\u017Eete poku\u0161ati prenijeti noviju verziju Rufflea koja bi mogla zaobi\u0107i problem (trenutna verzija je zastarjela: { $buildDate }).
error-csp-conflict =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja inicijalizacije.
    Pravila sigurnosti sadr\u017Eaja ovog web poslu\u017Eitelja ne dopu\u0161taju pokretanje potrebne komponente ".wasm".
    Ako ste administrator poslu\u017Eitelja, za pomo\u0107 se obratite Ruffle wikiju.
error-url-invalid =
    Ruffle nije uspio u\u010Ditati Flash SWF datoteku.
    Najvjerojatniji razlog je taj \u0161to je Ruffleu proslije\u0111en neva\u017Ee\u0107i URL za SWF datoteku.
error-unknown =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja prikaza ovog Flash sadr\u017Eaja.
    { $outdated ->
    [true] Ako ste administrator poslu\u017Eitelja, poku\u0161ajte prenijeti noviju verziju Rufflea (trenutna verzija je zastarjela: { $buildDate }).
    *[false] Ovo se ne bi trebalo doga\u0111ati, pa bismo vam bili jako zahvalni ako biste prijavili gre\u0161ku!
    }
`,"save-manager.ftl":`save-delete-prompt = Jeste li sigurni da \u017Eelite izbrisati ovu spremljenu datoteku?
save-reload-prompt =
    Jedini na\u010Din da { $action ->
    [delete] izbri\u0161ete
    *[replace] zamijenite
    } ovu datoteku za spremanje bez potencijalnog sukoba jest ponovno u\u010Ditavanje ovog sadr\u017Eaja. \u017Delite li ipak nastaviti?
save-download = Preuzmite
save-replace = Zamijeni
save-delete = Izbri\u0161i
save-backup-all = Preuzmi sve spremljene datoteke
`,"volume-controls.ftl":`volume-controls-mute = Isklju\u010Di zvuk
volume-controls-unmute = Uklju\u010Di zvuk
`},"hu-HU":{"context_menu.ftl":`context-menu-download-swf = SWF f\xE1jl let\xF6lt\xE9se
context-menu-copy-debug-info = Hibakeres\xE9si inform\xE1ci\xF3k m\xE1sol\xE1sa
context-menu-open-save-manager = Ment\xE9skezel\u0151 megnyit\xE1sa
context-menu-about-ruffle =
    { $flavor ->
        [extension] A Ruffle kieg\xE9sz\xEDt\u0151 ({ $version }) n\xE9vjegye
       *[other] A Ruffle ({ $version }) n\xE9vjegye
    }
context-menu-hide = Ezen men\xFC elrejt\xE9se
context-menu-exit-fullscreen = Kil\xE9p\xE9s a teljes k\xE9perny\u0151b\u0151l
context-menu-enter-fullscreen = V\xE1lt\xE1s teljes k\xE9perny\u0151re
context-menu-volume-controls = Hanger\u0151szab\xE1lyz\xF3
`,"messages.ftl":`message-cant-embed =
    A Ruffle nem tudta futtatni az oldalba \xE1gyazott Flash tartalmat.
    A probl\xE9ma kiker\xFCl\xE9s\xE9hez megpr\xF3b\xE1lhatod megnyitni a f\xE1jlt egy k\xFCl\xF6n lapon.
message-restored-from-bfcache =
    A b\xF6ng\xE9sz\u0151 ezt a Flash tartalmat egy kor\xE1bbi munkamenetb\u0151l \xE1ll\xEDtotta vissza.
    A tiszta indul\xE1shoz friss\xEDtse az oldalt.
panic-title = Valami baj t\xF6rt\xE9nt :(
more-info = Tov\xE1bbi inform\xE1ci\xF3
run-anyway = Futtat\xE1s m\xE9gis
continue = Folytat\xE1s
report-bug = Hiba jelent\xE9se
update-ruffle = Ruffle friss\xEDt\xE9se
ruffle-demo = Webes dem\xF3
ruffle-desktop = Asztali alkalmaz\xE1s
ruffle-wiki = Ruffle Wiki megnyit\xE1sa
enable-hardware-acceleration = \xDAgy t\u0171nik, a hardveres gyors\xEDt\xE1s ki van kapcsolva. B\xE1r a Ruffle m\u0171k\xF6dhet, nagyon lass\xFA lehet. Az al\xE1bbi hivatkoz\xE1st k\xF6vetve megtudhatod, hogyan enged\xE9lyezd a hardveres gyors\xEDt\xE1st:
enable-hardware-acceleration-link = GYIK - Chrome hardveres gyors\xEDt\xE1s
view-error-details = Hiba r\xE9szletei
open-in-new-tab = Megnyit\xE1s \xFAj lapon
click-to-unmute = Kattints a n\xE9m\xEDt\xE1s felold\xE1s\xE1hoz
clipboard-message-title = M\xE1sol\xE1s \xE9s be\xEDlleszt\xE9s a Ruffle-ben
clipboard-message-description =
    { $variant ->
       *[unsupported] A b\xF6ng\xE9sz\u0151d nem t\xE1mogatja a v\xE1g\xF3laphoz val\xF3 teljes hozz\xE1f\xE9r\xE9st,
        [access-denied] A v\xE1g\xF3laphoz val\xF3 hozz\xE1f\xE9r\xE9s el lett utas\xEDtva,
    } de mindig haszn\xE1lhatod ezeket a gyorsbillenty\u0171ket helyette:
clipboard-message-copy = { " " } m\xE1sol\xE1shoz
clipboard-message-cut = { " " } kiv\xE1g\xE1shoz
clipboard-message-paste = { " " } beilleszt\xE9shez
error-canvas-reload = \xDAjrat\xF6lt\xE9s a canvas megjelen\xEDt\u0151vel nem lehets\xE9ges, ha m\xE1r az van haszn\xE1latban.
error-file-protocol =
    \xDAgy t\u0171nik, a Ruffle-t a "file:" protokollon futtatod.
    Ez nem m\u0171k\xF6dik, mivel \xEDgy a b\xF6ng\xE9sz\u0151k biztons\xE1gi okokb\xF3l sz\xE1mos funkci\xF3 m\u0171k\xF6d\xE9s\xE9t letiltj\xE1k.
    Ehelyett azt aj\xE1nljuk hogy ind\xEDts egy helyi kiszolg\xE1l\xF3t, vagy haszn\xE1ld a webes dem\xF3t vagy az asztali alkalmaz\xE1st.
error-javascript-config =
    A Ruffle komoly probl\xE9m\xE1ba \xFCtk\xF6z\xF6tt egy helytelen JavaScript-konfigur\xE1ci\xF3 miatt.
    Ha a szerver rendszergazd\xE1ja vagy, k\xE9rj\xFCk, ellen\u0151rizd a hiba r\xE9szleteit, hogy megtudd, melyik param\xE9ter a hib\xE1s.
    A Ruffle wikiben is tal\xE1lhatsz ehhez seg\xEDts\xE9get.
error-wasm-not-found =
    A Ruffle nem tudta bet\xF6lteni a sz\xFCks\xE9ges ".wasm" \xF6sszetev\u0151t.
    Ha a szerver rendszergazd\xE1ja vagy, k\xE9rj\xFCk ellen\u0151rizd, hogy a f\xE1jl megfelel\u0151en lett-e felt\xF6ltve.
    Ha a probl\xE9ma tov\xE1bbra is fenn\xE1ll, el\u0151fordulhat, hogy a "publicPath" be\xE1ll\xEDt\xE1st kell haszn\xE1lnod: seg\xEDts\xE9g\xE9rt keresd fel a Ruffle wikit.
error-wasm-mime-type =
    A Ruffle komoly probl\xE9m\xE1ba \xFCtk\xF6z\xF6tt az inicializ\xE1l\xE1s sor\xE1n.
    Ez a webszerver a ".wasm" f\xE1jlokat nem a megfelel\u0151 MIME-t\xEDpussal szolg\xE1lja ki.
    Ha a szerver rendszergazd\xE1ja vagy, k\xE9rj\xFCk, keresd fel a Ruffle wikit seg\xEDts\xE9g\xE9rt.
error-invalid-swf =
    A Ruffle nem tudta \xE9rtelmezni a k\xE9rt f\xE1jlt.
    Ennek a legval\xF3sz\xEDn\u0171bb oka az, hogy a k\xE9rt f\xE1jl nem \xE9rv\xE9nyes SWF.
error-swf-fetch =
    A Ruffle nem tudta bet\xF6lteni a Flash SWF f\xE1jlt.
    A legval\xF3sz\xEDn\u0171bb ok az, hogy a f\xE1jl m\xE1r nem l\xE9tezik, \xEDgy a Ruffle sz\xE1m\xE1ra nincs mit bet\xF6lteni.
    Pr\xF3b\xE1ld meg felvenni a kapcsolatot a webhely rendszergazd\xE1j\xE1val seg\xEDts\xE9g\xE9rt.
error-swf-cors =
    A Ruffle nem tudta bet\xF6lteni a Flash SWF f\xE1jlt.
    A lek\xE9r\xE9shez val\xF3 hozz\xE1f\xE9r\xE9st val\xF3sz\xEDn\u0171leg letiltotta a CORS-h\xE1zirend.
    Ha a szerver rendszergazd\xE1ja vagy, k\xE9rj\xFCk, keresd fel a Ruffle wikit seg\xEDts\xE9g\xE9rt.
error-wasm-cors =
    A Ruffle nem tudta bet\xF6lteni a sz\xFCks\xE9ges ".wasm" \xF6sszetev\u0151t.
    A lek\xE9r\xE9shez val\xF3 hozz\xE1f\xE9r\xE9st val\xF3sz\xEDn\u0171leg letiltotta a CORS-h\xE1zirend.
    Ha a szerver rendszergazd\xE1ja vagy, k\xE9rj\xFCk keresd fel a Ruffle wikit seg\xEDts\xE9g\xE9rt.
error-wasm-invalid =
    A Ruffle komoly probl\xE9m\xE1ba \xFCtk\xF6z\xF6tt az inicializ\xE1l\xE1s sor\xE1n.
    \xDAgy t\u0171nik, hogy ezen az oldalon hi\xE1nyoznak vagy hib\xE1sak a Ruffle futtat\xE1s\xE1hoz sz\xFCks\xE9ges f\xE1jlok.
    Ha a szerver rendszergazd\xE1ja vagy, k\xE9rj\xFCk keresd fel a Ruffle wikit seg\xEDts\xE9g\xE9rt.
error-wasm-download =
    A Ruffle komoly probl\xE9m\xE1ba \xFCtk\xF6z\xF6tt az inicializ\xE1l\xE1s sor\xE1n.
    Ez gyakran mag\xE1t\xF3l megold\xF3dik, ez\xE9rt megpr\xF3b\xE1lhatod \xFAjrat\xF6lteni az oldalt.
    Ellenkez\u0151 esetben fordulj a webhely rendszergazd\xE1j\xE1hoz.
error-wasm-disabled-on-edge =
    A Ruffle nem tudta bet\xF6lteni a sz\xFCks\xE9ges ".wasm" \xF6sszetev\u0151t.
    A probl\xE9ma megold\xE1s\xE1hoz nyisd meg a b\xF6ng\xE9sz\u0151 be\xE1ll\xEDt\xE1sait, kattints az \u201EAdatv\xE9delem, keres\xE9s \xE9s szolg\xE1ltat\xE1sok\u201D elemre, g\xF6rgess le, \xE9s kapcsold ki a \u201EFokozott biztons\xE1g a weben\u201D opci\xF3t.
    Ez lehet\u0151v\xE9 teszi a b\xF6ng\xE9sz\u0151 sz\xE1m\xE1ra, hogy bet\xF6ltse a sz\xFCks\xE9ges ".wasm" f\xE1jlokat.
    Ha a probl\xE9ma tov\xE1bbra is fenn\xE1ll, lehet, hogy m\xE1sik b\xF6ng\xE9sz\u0151t kell haszn\xE1lnod.
error-wasm-unsupported-browser =
    Az \xE1ltalad haszn\xE1lt b\xF6ng\xE9sz\u0151 nem t\xE1mogatja a Ruffle futtat\xE1s\xE1hoz sz\xFCks\xE9ges WebAssembly kieg\xE9sz\xEDt\xE9seket.
    K\xE9rlek, v\xE1lts egy t\xE1mogatott b\xF6ng\xE9sz\u0151re.
    A t\xE1mogatott b\xF6ng\xE9sz\u0151k list\xE1j\xE1t a Wikin tal\xE1lod.
error-javascript-conflict =
    A Ruffle komoly probl\xE9m\xE1ba \xFCtk\xF6z\xF6tt az inicializ\xE1l\xE1s sor\xE1n.
    \xDAgy t\u0171nik, ez az oldal olyan JavaScript-k\xF3dot haszn\xE1l, amely \xFCtk\xF6zik a Ruffle-lel.
    Ha a kiszolg\xE1l\xF3 rendszergazd\xE1ja vagy, k\xE9rj\xFCk, pr\xF3b\xE1ld meg a f\xE1jlt egy \xFCres oldalon bet\xF6lteni.
error-javascript-conflict-outdated = Megpr\xF3b\xE1lhatod tov\xE1bb\xE1 felt\xF6lteni a Ruffle egy \xFAjabb verzi\xF3j\xE1t is, amely megker\xFClheti a probl\xE9m\xE1t (a jelenlegi elavult: { $buildDate }).
error-csp-conflict =
    A Ruffle komoly probl\xE9m\xE1ba \xFCtk\xF6z\xF6tt az inicializ\xE1l\xE1s sor\xE1n.
    A kiszolg\xE1l\xF3 tartalombiztons\xE1gi h\xE1zirendje nem teszi lehet\u0151v\xE9 a sz\xFCks\xE9ges \u201E.wasm\u201D \xF6sszetev\u0151k futtat\xE1s\xE1t.
    Ha a szerver rendszergazd\xE1ja vagy, k\xE9rj\xFCk, keresd fel a Ruffle wikit seg\xEDts\xE9g\xE9rt.
error-unknown =
    A Ruffle komoly probl\xE9m\xE1ba \xFCtk\xF6z\xF6tt, mik\xF6zben megpr\xF3b\xE1lta megjelen\xEDteni ezt a Flash-tartalmat.
    { $outdated ->
        [true] Ha a szerver rendszergazd\xE1ja vagy, k\xE9rj\xFCk, pr\xF3b\xE1ld meg felt\xF6lteni a Ruffle egy \xFAjabb verzi\xF3j\xE1t (a jelenlegi elavult: { $buildDate }).
       *[false] Ennek nem lett volna szabad megt\xF6rt\xE9nnie, ez\xE9rt nagyon h\xE1l\xE1sak lenn\xE9nk, ha jelezn\xE9d a hib\xE1t!
    }
`,"save-manager.ftl":`save-delete-prompt = Biztosan t\xF6r\xF6lni akarod ezt a ment\xE9st?
save-reload-prompt =
    Ennek a ment\xE9snek az esetleges konfliktus n\xE9lk\xFCli { $action ->
        [delete] t\xF6rl\xE9s\xE9hez
       *[replace] cser\xE9j\xE9hez
    } \xFAjra kell t\xF6lteni a tartalmat. M\xE9gis szeretn\xE9d folytatni?
save-download = Let\xF6lt\xE9s
save-replace = Csere
save-delete = T\xF6rl\xE9s
save-backup-all = Az \xF6sszes f\xE1jl let\xF6lt\xE9se
`,"volume-controls.ftl":`volume-controls-mute = N\xE9m\xEDt\xE1s
volume-controls-unmute = N\xE9m\xEDt\xE1s felold\xE1sa
`},"id-ID":{"context_menu.ftl":`context-menu-download-swf = Unduh SWF
context-menu-copy-debug-info = Salin info debug
context-menu-open-save-manager = Buka Manager Save
context-menu-about-ruffle =
    { $flavor ->
        [extension] Tentang Ekstensi Ruffle ({ $version })
       *[other] Tentang Ruffle ({ $version })
    }
context-menu-hide = Sembunyikan Menu ini
context-menu-exit-fullscreen = Keluar dari layar penuh
context-menu-enter-fullscreen = Masuk mode layar penuh
context-menu-volume-controls = Pengaturan Volume
`,"messages.ftl":`message-cant-embed =
    Ruffle tidak dapat menjalankan Flash yang disematkan di halaman ini.
    Anda dapat mencoba membuka berkas di tab terpisah, untuk menghindari masalah ini.
panic-title = Terjadi kesalahan :(
more-info = Info lebih lanjut
run-anyway = Jalankan
continue = Lanjutkan
report-bug = Laporkan Bug
update-ruffle = Perbarui Ruffle
ruffle-demo = Demo Web
ruffle-desktop = Aplikasi Desktop
ruffle-wiki = Kunjungi Wiki Ruffle
view-error-details = Tunjukan Detail Error
open-in-new-tab = Buka di Tab Baru
click-to-unmute = Tekan untuk menyalakan suara
clipboard-message-title = Menyalin dan Menempel di Ruffle
clipboard-message-copy = { " " } untuk menyalin
clipboard-message-cut = { " " } untuk memotong
clipboard-message-paste = { " " } untuk menempel
error-file-protocol =
    Sepertinya anda menjalankan Ruffle di protokol "file:".
    Ini tidak berfungsi karena browser memblokir fitur ini dengan alasan keamanan.
    Sebagai gantinya, kami mengajak anda untuk membuat server lokal, menggunakan demo web atau aplikasi desktop.
error-javascript-config =
    Ruffle mengalami masalah besar karena konfigurasi JavaScript yang salah.
    Jika Anda adalah administrator server ini, kami mengajak Anda untuk memeriksa detail kesalahan untuk mengetahui parameter mana yang salah.
    Anda juga dapat membaca wiki Ruffle untuk mendapatkan bantuan.
error-wasm-not-found =
    Ruffle gagal memuat komponen berkas ".wasm" yang diperlukan.
    Jika Anda adalah administrator server ini, pastikan berkas telah diunggah dengan benar.
    Jika masalah terus berlanjut, Anda mungkin perlu menggunakan pengaturan "publicPath": silakan baca wiki Ruffle untuk mendapatkan bantuan.
error-wasm-mime-type =
    Ruffle mengalami masalah ketika mencoba melakukan inisialisasi.
    Server web ini tidak melayani berkas ".wasm" dengan tipe MIME yang benar.
    Jika Anda adalah administrator server ini, silakan baca wiki Ruffle untuk mendapatkan bantuan.
error-invalid-swf =
    Ruffle tidak dapat membaca berkas yang diminta.
    Kemungkinan terbesar berkas yang diminta bukan berkas SWF valid.
error-swf-fetch =
    Ruffle gagal memuat berkas SWF Flash.
    Kemungkinan berkas tersebut sudah tidak ada, sehingga tidak dapat dimuat oleh Ruffle.
    Coba hubungi administrator situs web ini untuk mendapatkan bantuan.
error-swf-cors =
    Ruffle gagal memuat berkas SWF Flash.
    Akses untuk memuat kemungkinan telah diblokir oleh kebijakan CORS.
    Jika Anda adalah administrator server ini, silakan baca wiki Ruffle untuk mendapatkan bantuan.
error-wasm-cors =
    Ruffle gagal memuat komponen berkas ".wasm" yang diperlukan.
    Akses untuk mengambil kemungkinan telah diblokir oleh kebijakan CORS.
    Jika Anda adalah administrator server ini, silakan baca wiki Ruffle untuk mendapatkan bantuan.
error-wasm-invalid =
    Ruffle mengalami masalah besar ketika mencoba melakukan inisialisasi.
    Sepertinya halaman ini memiliki berkas yang hilang atau tidak valid untuk menjalankan Ruffle.
    Jika Anda adalah administrator server ini, silakan baca wiki Ruffle untuk mendapatkan bantuan.
error-wasm-download =
    Ruffle mengalami masalah besar ketika mencoba melakukan inisialisasi.
    Hal ini sering kali dapat teratasi dengan sendirinya, sehingga Anda dapat mencoba memuat ulang halaman.
    Jika tidak, silakan hubungi administrator situs web ini.
error-wasm-disabled-on-edge =
    Ruffle gagal memuat komponen berkas ".wasm" yang diperlukan.
    Untuk mengatasinya, coba buka pengaturan peramban Anda, klik "Privasi, pencarian, dan layanan", turun ke bawah, dan matikan "Tingkatkan keamanan Anda di web".
    Ini akan memungkinkan browser Anda memuat berkas ".wasm" yang diperlukan.
    Jika masalah berlanjut, Anda mungkin harus menggunakan browser yang berbeda.
error-javascript-conflict =
    Ruffle mengalami masalah besar ketika mencoba melakukan inisialisasi.
    Sepertinya situs web ini menggunakan kode JavaScript yang bertentangan dengan Ruffle.
    Jika Anda adalah administrator server ini, kami mengajak Anda untuk mencoba memuat berkas pada halaman kosong.
error-javascript-conflict-outdated = Anda juga dapat mencoba mengunggah versi Ruffle yang lebih baru yang mungkin dapat mengatasi masalah ini (versi saat ini sudah kedaluwarsa: { $buildDate }).
error-csp-conflict =
    Ruffle mengalami masalah besar ketika mencoba melakukan inisialisasi.
    Kebijakan Keamanan Konten server web ini tidak mengizinkan komponen ".wasm" yang diperlukan untuk dijalankan.
    Jika Anda adalah administrator server ini, silakan baca wiki Ruffle untuk mendapatkan bantuan.
error-unknown =
    Ruffle mengalami masalah besar saat menampilkan konten Flash ini.
    { $outdated ->
        [true] Jika Anda administrator server ini, cobalah untuk mengganti versi Ruffle yang lebih baru (versi saat ini sudah kedaluwarsa: { $buildDate }).
       *[false] Hal ini seharusnya tidak terjadi, jadi kami sangat menghargai jika Anda dapat melaporkan bug ini!
    }
`,"save-manager.ftl":`save-delete-prompt = Anda yakin ingin menghapus berkas ini?
save-reload-prompt =
    Satu-satunya cara untuk { $action ->
        [delete] menghapus
       *[replace] mengganti
    } berkas penyimpanan ini tanpa potensi konflik adalah dengan memuat ulang konten ini. Apakah Anda ingin melanjutkannya?
save-download = Unduh
save-replace = Ganti
save-delete = Hapus
save-backup-all = Unduh semua berkas penyimpanan
`,"volume-controls.ftl":`volume-controls-mute = Bisukan
volume-controls-unmute = Bunyikan
`},"it-IT":{"context_menu.ftl":`context-menu-download-swf = Scarica SWF
context-menu-copy-debug-info = Copia informazioni di debug
context-menu-open-save-manager = Apri gestione salvataggi
context-menu-about-ruffle =
    { $flavor ->
        [extension] Informazioni su Ruffle Extension ({ $version })
       *[other] Informazioni su Ruffle ({ $version })
    }
context-menu-hide = Nascondi questo menu
context-menu-exit-fullscreen = Esci dallo schermo intero
context-menu-enter-fullscreen = Entra a schermo intero
context-menu-volume-controls = Controlli volume
`,"messages.ftl":`message-cant-embed =
    Ruffle non \xE8 stato in grado di eseguire il Flash incorporato in questa pagina.
    Puoi provare ad aprire il file in una scheda separata, per evitare questo problema.
message-restored-from-bfcache =
    Il tuo browser ha ripristinato il contenuto del Flash da una sessione precedente.
    Per iniziare da capo, ricarica la pagina.
panic-title = Qualcosa \xE8 andato storto :(
more-info = Maggiori informazioni
run-anyway = Esegui comunque
continue = Continua
report-bug = Segnala un bug
update-ruffle = Aggiorna Ruffle
ruffle-demo = Demo web
ruffle-desktop = Applicazione desktop
ruffle-wiki = Visualizza la wiki di Ruffle
enable-hardware-acceleration = Sembra che l'accelerazione hardware sia disabilitata. Sebbene Ruffle possa funzionare, potrebbe essere molto lento. Puoi scoprire come abilitare l'accelerazione hardware seguendo il link seguente:
enable-hardware-acceleration-link = FAQ - Accelerazione hardware di Chrome
view-error-details = Visualizza dettagli errore
open-in-new-tab = Apri in una nuova scheda
click-to-unmute = Clicca per riattivare l'audio
clipboard-message-title = Copiando e incollando su Ruffle
clipboard-message-description =
    { $variant ->
      *[unsupported] Il tuo browser non ha supporto per accesso completo degli appunti,
       [access-denied] Accesso agli appunti e stato negato,
    } ma puoi sempre usare le scorciatoie al loro posto:
clipboard-message-copy = { " " } per copiare
clipboard-message-cut = { " " } per tagliare
clipboard-message-paste = { " " } per incollare
error-canvas-reload = Impossibile ricaricare con il canvas renderer quando \xE8 in uso.
error-file-protocol =
    Sembra che tu stia eseguendo Ruffle sul protocollo "file:".
    Questo non funziona come browser blocca molte funzionalit\xE0 di lavoro per motivi di sicurezza.
    Invece, ti invitiamo a configurare un server locale o a utilizzare la demo web o l'applicazione desktop.
error-javascript-config =
    Ruffle ha incontrato un problema importante a causa di una configurazione JavaScript non corretta.
    Se sei l'amministratore del server, ti invitiamo a controllare i dettagli dell'errore per scoprire quale parametro \xE8 in errore.
    Puoi anche consultare la wiki di Ruffle per aiuto.
error-wasm-not-found =
    Ruffle non \xE8 riuscito a caricare il componente di file ".wasm".
    Se sei l'amministratore del server, assicurati che il file sia stato caricato correttamente.
    Se il problema persiste, potrebbe essere necessario utilizzare l'impostazione "publicPath": si prega di consultare la wiki di Ruffle per aiuto.
error-wasm-mime-type =
    Ruffle ha incontrato un problema importante durante il tentativo di inizializzazione.
    Questo server web non serve ".wasm" file con il tipo MIME corretto.
    Se sei l'amministratore del server, consulta la wiki di Ruffle per aiuto.
error-invalid-swf =
    Ruffle non pu\xF2 leggere il file richiesto.
    La ragione pi\xF9 probabile \xE8 che il file non \xE8 un SWF valido.
error-swf-fetch =
    Ruffle non \xE8 riuscito a caricare il file Flash SWF.
    La ragione pi\xF9 probabile \xE8 che il file non esiste pi\xF9, quindi non c'\xE8 nulla che Ruffle possa caricare.
    Prova a contattare l'amministratore del sito web per aiuto.
error-swf-cors =
    Ruffle non \xE8 riuscito a caricare il file SWF Flash.
    L'accesso al recupero probabilmente \xE8 stato bloccato dalla politica CORS.
    Se sei l'amministratore del server, consulta la wiki di Ruffle per ricevere aiuto.
error-wasm-cors =
    Ruffle non \xE8 riuscito a caricare il componente di file ".wasm".
    L'accesso al recupero probabilmente \xE8 stato bloccato dalla politica CORS.
    Se sei l'amministratore del server, consulta la wiki di Ruffle per ricevere aiuto.
error-wasm-invalid =
    Ruffle ha incontrato un problema importante durante il tentativo di inizializzazione.
    Sembra che questa pagina abbia file mancanti o non validi per l'esecuzione di Ruffle.
    Se sei l'amministratore del server, consulta la wiki di Ruffle per ricevere aiuto.
error-wasm-download =
    Ruffle ha incontrato un problema importante durante il tentativo di inizializzazione.
    Questo pu\xF2 spesso risolversi da solo, quindi puoi provare a ricaricare la pagina.
    Altrimenti, contatta l'amministratore del sito.
error-wasm-disabled-on-edge =
    Ruffle non ha caricato il componente di file ".wasm" richiesto.
    Per risolvere il problema, prova ad aprire le impostazioni del tuo browser, facendo clic su "Privacy, ricerca e servizi", scorrendo verso il basso e disattivando "Migliora la tua sicurezza sul web".
    Questo permetter\xE0 al tuo browser di caricare i file ".wasm" richiesti.
    Se il problema persiste, potresti dover usare un browser diverso.
error-wasm-unsupported-browser =
    Il browser che stai usando non ha supporto per l'estensione WebAssembly che Ruffle richiede per funzionare.
    Per favore cambi con un browser supportato.
    Puoi trovare una lista di browser supportati nella Wiki.
error-javascript-conflict =
    Ruffle ha riscontrato un problema importante durante il tentativo di inizializzazione.
    Sembra che questa pagina utilizzi il codice JavaScript che \xE8 in conflitto con Ruffle.
    Se sei l'amministratore del server, ti invitiamo a provare a caricare il file su una pagina vuota.
error-javascript-conflict-outdated = Puoi anche provare a caricare una versione pi\xF9 recente di Ruffle che potrebbe aggirare il problema (l'attuale build \xE8 obsoleta: { $buildDate }).
error-csp-conflict =
    Ruffle ha incontrato un problema importante durante il tentativo di inizializzare.
    La Politica di Sicurezza dei Contenuti di questo server web non consente l'impostazione richiesta". asm" componente da eseguire.
    Se sei l'amministratore del server, consulta la Ruffle di wiki per aiuto.
error-url-invalid =
    Ruffle non \xE8 riuscito a caricare il file Flash SWF.
    La ragione pi\xF9 probabile \xE8 che un URL non valido per il file SWF \xE8 stato passato a Ruffle.
error-unknown =
    Ruffle ha incontrato un problema importante durante il tentativo di visualizzare questo contenuto Flash.
    { $outdated ->
        [true] Se sei l'amministratore del server, prova a caricare una versione pi\xF9 recente di Ruffle (la versione attuale \xE8 obsoleta: { $buildDate }).
       *[false] Questo non dovrebbe accadere, quindi ci piacerebbe molto se si potesse inviare un bug!
    }
`,"save-manager.ftl":`save-delete-prompt = Sei sicuro di voler eliminare questo file di salvataggio?
save-reload-prompt =
    L'unico modo per { $action ->
        [delete] delete
       *[replace] replace
    } questo salvataggio file senza potenziali conflitti \xE8 quello di ricaricare questo contenuto. Volete continuare comunque?
save-download = Scarica
save-replace = Sostituisci
save-delete = Elimina
save-backup-all = Scarica tutti i file di salvataggio
`,"volume-controls.ftl":`volume-controls-mute = Silenzia
volume-controls-unmute = Riattiva l'audio
`},"ja-JP":{"context_menu.ftl":`context-menu-download-swf = .swf\u3092\u30C0\u30A6\u30F3\u30ED\u30FC\u30C9
context-menu-copy-debug-info = \u30C7\u30D0\u30C3\u30B0\u60C5\u5831\u3092\u30B3\u30D4\u30FC
context-menu-open-save-manager = \u30BB\u30FC\u30D6\u30DE\u30CD\u30FC\u30B8\u30E3\u30FC\u3092\u958B\u304F
context-menu-about-ruffle =
    { $flavor ->
        [extension] Ruffle\u62E1\u5F35\u6A5F\u80FD\u306B\u3064\u3044\u3066 ({ $version })
       *[other] Ruffle\u306B\u3064\u3044\u3066 ({ $version })
    }
context-menu-hide = \u30E1\u30CB\u30E5\u30FC\u3092\u96A0\u3059
context-menu-exit-fullscreen = \u30D5\u30EB\u30B9\u30AF\u30EA\u30FC\u30F3\u3092\u7D42\u4E86
context-menu-enter-fullscreen = \u30D5\u30EB\u30B9\u30AF\u30EA\u30FC\u30F3\u306B\u3059\u308B
context-menu-volume-controls = \u97F3\u91CF\u8ABF\u7BC0
`,"messages.ftl":`message-cant-embed =
    Ruffle\u306F\u3053\u306E\u30DA\u30FC\u30B8\u306B\u57CB\u3081\u8FBC\u307E\u308C\u305F Flash \u3092\u5B9F\u884C\u3067\u304D\u307E\u305B\u3093\u3067\u3057\u305F\u3002
    \u5225\u306E\u30BF\u30D6\u3067\u30D5\u30A1\u30A4\u30EB\u3092\u958B\u304F\u3053\u3068\u3067\u3001\u3053\u306E\u554F\u984C\u3092\u89E3\u6C7A\u3067\u304D\u308B\u304B\u3082\u3057\u308C\u307E\u305B\u3093\u3002
message-restored-from-bfcache =
    \u30D6\u30E9\u30A6\u30B6\u306F\u3001\u524D\u56DE\u306E\u30BB\u30C3\u30B7\u30E7\u30F3\u304B\u3089Flash\u30B3\u30F3\u30C6\u30F3\u30C4\u3092\u5FA9\u5143\u3057\u307E\u3057\u305F\u3002
    \u6700\u521D\u304B\u3089\u958B\u59CB\u3059\u308B\u306B\u306F\u3001\u30DA\u30FC\u30B8\u3092\u518D\u8AAD\u307F\u8FBC\u307F\u3057\u3066\u304F\u3060\u3055\u3044\u3002
panic-title = \u30A8\u30E9\u30FC\u304C\u767A\u751F\u3057\u307E\u3057\u305F :(
more-info = \u8A73\u7D30\u60C5\u5831
run-anyway = \u3068\u306B\u304B\u304F\u5B9F\u884C\u3059\u308B
continue = \u7D9A\u884C
report-bug = \u30D0\u30B0\u3092\u5831\u544A
update-ruffle = Ruffle\u3092\u66F4\u65B0
ruffle-demo = Web\u30C7\u30E2
ruffle-desktop = \u30C7\u30B9\u30AF\u30C8\u30C3\u30D7\u30A2\u30D7\u30EA
ruffle-wiki = Ruffle Wiki\u3092\u95B2\u89A7
enable-hardware-acceleration = \u30CF\u30FC\u30C9\u30A6\u30A7\u30A2 \u30A2\u30AF\u30BB\u30E9\u30EC\u30FC\u30B7\u30E7\u30F3\u304C\u7121\u52B9\u306B\u306A\u3063\u3066\u3044\u308B\u3088\u3046\u3067\u3059\u3002Ruffle \u306F\u52D5\u4F5C\u3059\u308B\u304B\u3082\u3057\u308C\u307E\u305B\u3093\u304C\u3001\u975E\u5E38\u306B\u9045\u304F\u306A\u308B\u53EF\u80FD\u6027\u304C\u3042\u308A\u307E\u3059\u3002\u30CF\u30FC\u30C9\u30A6\u30A7\u30A2 \u30A2\u30AF\u30BB\u30E9\u30EC\u30FC\u30B7\u30E7\u30F3\u3092\u6709\u52B9\u306B\u3059\u308B\u65B9\u6CD5\u306B\u3064\u3044\u3066\u306F\u3001\u4EE5\u4E0B\u306E\u30EA\u30F3\u30AF\u3092\u53C2\u7167\u3057\u3066\u304F\u3060\u3055\u3044\u3002
enable-hardware-acceleration-link = \u3088\u304F\u3042\u308B\u8CEA\u554F - Chrome\u306E\u30CF\u30FC\u30C9\u30A6\u30A7\u30A2 \u30A2\u30AF\u30BB\u30E9\u30EC\u30FC\u30B7\u30E7\u30F3
view-error-details = \u30A8\u30E9\u30FC\u306E\u8A73\u7D30\u3092\u8868\u793A
open-in-new-tab = \u65B0\u3057\u3044\u30BF\u30D6\u3067\u958B\u304F
click-to-unmute = \u30AF\u30EA\u30C3\u30AF\u3067\u30DF\u30E5\u30FC\u30C8\u3092\u89E3\u9664
clipboard-message-title = Ruffle\u3067\u306E\u30B3\u30D4\u30FC\u3068\u8CBC\u308A\u4ED8\u3051
clipboard-message-description =
    { $variant ->
       *[unsupported] \u304A\u4F7F\u3044\u306E\u30D6\u30E9\u30A6\u30B6\u306F\u30AF\u30EA\u30C3\u30D7\u30DC\u30FC\u30C9\u3078\u306E\u30D5\u30EB\u30A2\u30AF\u30BB\u30B9\u3092\u30B5\u30DD\u30FC\u30C8\u3057\u3066\u3044\u307E\u305B\u3093\u3002
        [access-denied] \u30AF\u30EA\u30C3\u30D7\u30DC\u30FC\u30C9\u3078\u306E\u30A2\u30AF\u30BB\u30B9\u304C\u62D2\u5426\u3055\u308C\u307E\u3057\u305F\u3002
    } \u4EE3\u308F\u308A\u306B\u3001\u4EE5\u4E0B\u306E\u30B7\u30E7\u30FC\u30C8\u30AB\u30C3\u30C8\u3092\u5229\u7528\u3067\u304D\u307E\u3059:
clipboard-message-copy = { " " } : \u30B3\u30D4\u30FC
clipboard-message-cut = { " " } : \u5207\u308A\u53D6\u308A
clipboard-message-paste = { " " } : \u8CBC\u308A\u4ED8\u3051
error-canvas-reload = canvas\u30EC\u30F3\u30C0\u30E9\u4F7F\u7528\u4E2D\u306E\u305F\u3081\u3001canvas\u30EC\u30F3\u30C0\u30E9\u306B\u3088\u308B\u518D\u8AAD\u307F\u8FBC\u307F\u306F\u3067\u304D\u307E\u305B\u3093\u3002
error-file-protocol =
    Ruffle\u3092"file:"\u30D7\u30ED\u30C8\u30B3\u30EB\u3067\u4F7F\u7528\u3057\u3066\u3044\u308B\u3088\u3046\u3067\u3059\u3002
    \u30D6\u30E9\u30A6\u30B6\u306F\u30BB\u30AD\u30E5\u30EA\u30C6\u30A3\u4E0A\u306E\u7406\u7531\u304B\u3089\u591A\u304F\u306E\u6A5F\u80FD\u3092\u5236\u9650\u3057\u3066\u3044\u308B\u305F\u3081\u3001\u6B63\u3057\u304F\u52D5\u4F5C\u3057\u307E\u305B\u3093\u3002
    \u30ED\u30FC\u30AB\u30EB\u30B5\u30FC\u30D0\u30FC\u3092\u30BB\u30C3\u30C8\u30A2\u30C3\u30D7\u3059\u308B\u304B\u3001\u30A6\u30A7\u30D6\u30C7\u30E2\u307E\u305F\u306F\u30C7\u30B9\u30AF\u30C8\u30C3\u30D7\u30A2\u30D7\u30EA\u3092\u3054\u5229\u7528\u304F\u3060\u3055\u3044\u3002
error-javascript-config =
    JavaScript\u306E\u8A2D\u5B9A\u304C\u6B63\u3057\u304F\u306A\u3044\u305F\u3081\u3001Ruffle\u3067\u554F\u984C\u304C\u767A\u751F\u3057\u307E\u3057\u305F\u3002
    \u30B5\u30FC\u30D0\u30FC\u7BA1\u7406\u8005\u306E\u65B9\u306F\u3001\u30A8\u30E9\u30FC\u306E\u8A73\u7D30\u304B\u3089\u3001\u3069\u306E\u30D1\u30E9\u30E1\u30FC\u30BF\u30FC\u306B\u554F\u984C\u304C\u3042\u308B\u306E\u304B\u3092\u78BA\u8A8D\u3057\u3066\u304F\u3060\u3055\u3044\u3002
    Ruffle\u306Ewiki\u3092\u53C2\u7167\u3059\u308B\u3053\u3068\u3067\u3001\u89E3\u6C7A\u65B9\u6CD5\u304C\u898B\u3064\u304B\u308B\u304B\u3082\u3057\u308C\u307E\u305B\u3093\u3002
error-wasm-not-found =
    Ruffle\u306F\u3001\u5FC5\u8981\u306A\u300C.wasm\u300D\u30D5\u30A1\u30A4\u30EB\u30B3\u30F3\u30DD\u30FC\u30CD\u30F3\u30C8\u306E\u8AAD\u307F\u8FBC\u307F\u306B\u5931\u6557\u3057\u307E\u3057\u305F\u3002
    \u30B5\u30FC\u30D0\u30FC\u306E\u7BA1\u7406\u8005\u306F\u3001\u30D5\u30A1\u30A4\u30EB\u304C\u6B63\u3057\u304F\u30A2\u30C3\u30D7\u30ED\u30FC\u30C9\u3055\u308C\u3066\u3044\u308B\u304B\u78BA\u8A8D\u3092\u3057\u3066\u304F\u3060\u3055\u3044\u3002\u554F\u984C\u304C\u89E3\u6C7A\u3057\u306A\u3044\u5834\u5408\u306F\u3001\u300CpublicPath\u300D\u306E\u8A2D\u5B9A\u304C\u5FC5\u8981\u304B\u3082\u3057\u308C\u307E\u305B\u3093\u3002Ruffle\u306Ewiki\u3092\u53C2\u7167\u3057\u3066\u304F\u3060\u3055\u3044\u3002
error-wasm-mime-type =
    Ruffle\u306E\u521D\u671F\u5316\u4E2D\u306B\u5927\u304D\u306A\u554F\u984C\u304C\u767A\u751F\u3057\u307E\u3057\u305F\u3002
    \u3053\u306EWeb\u30B5\u30FC\u30D0\u30FC\u306F\u300C.wasm\u300D\u30D5\u30A1\u30A4\u30EB\u3092\u6B63\u3057\u3044MIME\u30BF\u30A4\u30D7\u3067\u63D0\u4F9B\u3057\u3066\u3044\u307E\u305B\u3093\u3002
    \u30B5\u30FC\u30D0\u30FC\u306E\u7BA1\u7406\u8005\u306F\u3001Ruffle\u306Ewiki\u3092\u53C2\u7167\u3057\u3066\u304F\u3060\u3055\u3044\u3002
error-invalid-swf =
    Ruffle \u306F\u30EA\u30AF\u30A8\u30B9\u30C8\u3055\u308C\u305F\u30D5\u30A1\u30A4\u30EB\u306E\u30D1\u30FC\u30B9\u306B\u5931\u6557\u3057\u307E\u3057\u305F\u3002
    \u6700\u3082\u8003\u3048\u3089\u308C\u308B\u539F\u56E0\u306F\u3001\u30D5\u30A1\u30A4\u30EB\u304C\u6709\u52B9\u306A SWF \u3067\u306A\u3044\u3053\u3068\u3067\u3059\u3002
error-swf-fetch =
    Ruffle\u304CFlash SWF\u30D5\u30A1\u30A4\u30EB\u306E\u8AAD\u307F\u8FBC\u307F\u306B\u5931\u6557\u3057\u307E\u3057\u305F\u3002
    \u8AAD\u307F\u8FBC\u3080\u3079\u304D\u30D5\u30A1\u30A4\u30EB\u304C\u65E2\u306B\u5B58\u5728\u3057\u3066\u3044\u306A\u3044\u3053\u3068\u304C\u539F\u56E0\u3067\u3042\u308B\u53EF\u80FD\u6027\u304C\u9AD8\u3044\u3067\u3059\u3002
    Web\u30B5\u30A4\u30C8\u306E\u7BA1\u7406\u8005\u306B\u304A\u554F\u3044\u5408\u308F\u305B\u304F\u3060\u3055\u3044\u3002
error-swf-cors =
    Ruffle\u306FSWF\u30D5\u30A1\u30A4\u30EB\u306E\u8AAD\u307F\u8FBC\u307F\u306B\u5931\u6557\u3057\u307E\u3057\u305F\u3002
    CORS\u30DD\u30EA\u30B7\u30FC\u306E\u8A2D\u5B9A\u306B\u3088\u308A\u3001fetch\u3078\u306E\u30A2\u30AF\u30BB\u30B9\u304C\u30D6\u30ED\u30C3\u30AF\u3055\u308C\u3066\u3044\u308B\u53EF\u80FD\u6027\u304C\u3042\u308A\u307E\u3059\u3002
    \u30B5\u30FC\u30D0\u30FC\u7BA1\u7406\u8005\u306E\u65B9\u306F\u3001Ruffle\u306Ewiki\u3092\u53C2\u7167\u3057\u3066\u304F\u3060\u3055\u3044\u3002
error-wasm-cors =
    Ruffle\u306B\u5FC5\u8981\u3068\u306A\u308B\u300C.wasm\u300D\u30D5\u30A1\u30A4\u30EB\u30B3\u30F3\u30DD\u30FC\u30CD\u30F3\u30C8\u306E\u8AAD\u307F\u8FBC\u307F\u306B\u5931\u6557\u3057\u307E\u3057\u305F\u3002
    CORS\u30DD\u30EA\u30B7\u30FC\u306B\u3088\u3063\u3066fetch\u3078\u306E\u30A2\u30AF\u30BB\u30B9\u304C\u30D6\u30ED\u30C3\u30AF\u3055\u308C\u3066\u3044\u308B\u53EF\u80FD\u6027\u304C\u3042\u308A\u307E\u3059\u3002
    \u30B5\u30FC\u30D0\u30FC\u306E\u7BA1\u7406\u8005\u306F\u3001Ruffle wiki\u3092\u53C2\u7167\u3057\u3066\u304F\u3060\u3055\u3044\u3002
error-wasm-invalid =
    Ruffle\u306E\u521D\u671F\u5316\u6642\u306B\u91CD\u5927\u306A\u554F\u984C\u304C\u767A\u751F\u3057\u307E\u3057\u305F\u3002
    \u3053\u306E\u30DA\u30FC\u30B8\u306B\u306FRuffle\u3092\u5B9F\u884C\u3059\u308B\u305F\u3081\u306E\u30D5\u30A1\u30A4\u30EB\u304C\u5B58\u5728\u3057\u306A\u3044\u304B\u3001\u7121\u52B9\u306A\u30D5\u30A1\u30A4\u30EB\u304C\u3042\u308B\u304B\u3082\u3057\u308C\u307E\u305B\u3093\u3002
    \u30B5\u30FC\u30D0\u30FC\u306E\u7BA1\u7406\u8005\u306F\u3001Ruffle\u306Ewiki\u3092\u53C2\u7167\u3057\u3066\u304F\u3060\u3055\u3044\u3002
error-wasm-download =
    Ruffle\u306E\u521D\u671F\u5316\u6642\u306B\u91CD\u5927\u306A\u554F\u984C\u304C\u767A\u751F\u3057\u307E\u3057\u305F\u3002
    \u3053\u306E\u554F\u984C\u306F\u81EA\u7136\u306B\u89E3\u6C7A\u3059\u308B\u5834\u5408\u304C\u3042\u308B\u305F\u3081\u3001\u30DA\u30FC\u30B8\u306E\u518D\u8AAD\u307F\u8FBC\u307F\u3092\u8A66\u3057\u3066\u304F\u3060\u3055\u3044\u3002
    \u305D\u308C\u3067\u3082\u89E3\u6C7A\u3057\u306A\u3044\u5834\u5408\u306F\u3001Web\u30B5\u30A4\u30C8\u306E\u7BA1\u7406\u8005\u306B\u304A\u554F\u3044\u5408\u308F\u305B\u304F\u3060\u3055\u3044\u3002
error-wasm-disabled-on-edge =
    Ruffle\u306B\u5FC5\u8981\u3068\u306A\u308B\u300C.wasm\u300D\u30D5\u30A1\u30A4\u30EB\u30B3\u30F3\u30DD\u30FC\u30CD\u30F3\u30C8\u306E\u8AAD\u307F\u8FBC\u307F\u306B\u5931\u6557\u3057\u307E\u3057\u305F\u3002
    \u554F\u984C\u89E3\u6C7A\u306E\u305F\u3081\u3001\u30D6\u30E9\u30A6\u30B6\u30FC\u306E\u8A2D\u5B9A\u753B\u9762\u304B\u3089\u3001\u300C\u30D7\u30E9\u30A4\u30D0\u30B7\u30FC\u3001\u691C\u7D22\u3001\u30B5\u30FC\u30D3\u30B9\u300D\u3092\u30AF\u30EA\u30C3\u30AF\u3057\u3001\u4E0B\u306B\u30B9\u30AF\u30ED\u30FC\u30EB\u3057\u3066\u300CWeb\u4E0A\u306E\u30BB\u30AD\u30E5\u30EA\u30C6\u30A3\u3092\u5F37\u5316\u3059\u308B\u300D\u3092\u30AA\u30D5\u306B\u3057\u3066\u307F\u3066\u304F\u3060\u3055\u3044\u3002
    \u5FC5\u8981\u3068\u306A\u308B\u300C.wasm\u300D\u30D5\u30A1\u30A4\u30EB\u306E\u8AAD\u307F\u8FBC\u307F\u304C\u8A31\u53EF\u3055\u308C\u307E\u3059\u3002
    \u305D\u308C\u3067\u3082\u554F\u984C\u304C\u89E3\u6C7A\u3057\u306A\u3044\u5834\u5408\u3001\u5225\u306E\u30D6\u30E9\u30A6\u30B6\u30FC\u3092\u4F7F\u7528\u3059\u308B\u5FC5\u8981\u304C\u3042\u308B\u304B\u3082\u3057\u308C\u307E\u305B\u3093\u3002
error-wasm-unsupported-browser =
    \u73FE\u5728\u4F7F\u7528\u4E2D\u306E\u30D6\u30E9\u30A6\u30B6\u306F\u3001Ruffle\u306E\u52D5\u4F5C\u306B\u5FC5\u8981\u306AWebAssembly\u62E1\u5F35\u3092\u30B5\u30DD\u30FC\u30C8\u3057\u3066\u3044\u307E\u305B\u3093\u3002
    \u30B5\u30DD\u30FC\u30C8\u3055\u308C\u3066\u3044\u308B\u30D6\u30E9\u30A6\u30B6\u3092\u3054\u5229\u7528\u304F\u3060\u3055\u3044\u3002
    \u30B5\u30DD\u30FC\u30C8\u3055\u308C\u3066\u3044\u308B\u30D6\u30E9\u30A6\u30B6\u4E00\u89A7\u306F\u3001Wiki\u306B\u8A18\u8F09\u3055\u308C\u3066\u3044\u307E\u3059\u3002
error-javascript-conflict =
    Ruffle\u306E\u521D\u671F\u5316\u6642\u306B\u91CD\u5927\u306A\u554F\u984C\u304C\u767A\u751F\u3057\u307E\u3057\u305F\u3002
    \u3053\u306E\u30DA\u30FC\u30B8\u3067\u306FRuffle\u3068\u7AF6\u5408\u3059\u308BJavaScript\u30B3\u30FC\u30C9\u304C\u4F7F\u7528\u3055\u308C\u3066\u3044\u308B\u304B\u3082\u3057\u308C\u307E\u305B\u3093\u3002
    \u30B5\u30FC\u30D0\u30FC\u306E\u7BA1\u7406\u8005\u306F\u3001\u7A7A\u767D\u306E\u30DA\u30FC\u30B8\u3067\u30D5\u30A1\u30A4\u30EB\u3092\u8AAD\u307F\u8FBC\u307F\u3057\u76F4\u3057\u3066\u307F\u3066\u304F\u3060\u3055\u3044\u3002
error-javascript-conflict-outdated = \u65B0\u3057\u3044\u30D0\u30FC\u30B8\u30E7\u30F3\u306ERuffle\u3092\u30A2\u30C3\u30D7\u30ED\u30FC\u30C9\u3059\u308B\u3053\u3068\u3067\u3001\u3053\u306E\u554F\u984C\u3092\u56DE\u907F\u3067\u304D\u308B\u53EF\u80FD\u6027\u304C\u3042\u308A\u307E\u3059\u3002(\u73FE\u5728\u306E\u30D3\u30EB\u30C9\u306F\u53E4\u3044\u7269\u3067\u3059:{ $buildDate })
error-csp-conflict =
    Ruffle\u306E\u521D\u671F\u5316\u6642\u306B\u91CD\u5927\u306A\u554F\u984C\u304C\u767A\u751F\u3057\u307E\u3057\u305F\u3002
    \u3053\u306EWeb\u30B5\u30FC\u30D0\u30FC\u306E\u30B3\u30F3\u30C6\u30F3\u30C4\u30BB\u30AD\u30E5\u30EA\u30C6\u30A3\u30DD\u30EA\u30B7\u30FC\u304C\u5B9F\u884C\u306B\u5FC5\u8981\u3068\u306A\u308B\u300C.wasm\u300D\u30B3\u30F3\u30DD\u30FC\u30CD\u30F3\u30C8\u306E\u5B9F\u884C\u3092\u8A31\u53EF\u3057\u3066\u3044\u307E\u305B\u3093\u3002
    \u30B5\u30FC\u30D0\u30FC\u306E\u7BA1\u7406\u8005\u306F\u3001Ruffle\u306Ewiki\u3092\u53C2\u7167\u3057\u3066\u304F\u3060\u3055\u3044\u3002
error-url-invalid =
    Ruffle\u306FSWF\u30D5\u30A1\u30A4\u30EB\u306E\u8AAD\u307F\u8FBC\u307F\u306B\u5931\u6557\u3057\u307E\u3057\u305F\u3002
    Ruffle\u306B\u6E21\u3055\u308C\u305FURL\u304C\u7121\u52B9\u3067\u3042\u308B\u3053\u3068\u304C\u539F\u56E0\u304B\u3082\u3057\u308C\u307E\u305B\u3093\u3002
error-unknown =
    Flash\u30B3\u30F3\u30C6\u30F3\u30C4\u3092\u8868\u793A\u3059\u308B\u969B\u306BRuffle\u3067\u554F\u984C\u304C\u767A\u751F\u3057\u307E\u3057\u305F\u3002
    { $outdated ->
        [true] \u73FE\u5728\u4F7F\u7528\u3057\u3066\u3044\u308B\u30D3\u30EB\u30C9\u306F\u6700\u65B0\u3067\u306F\u306A\u3044\u305F\u3081\u3001\u30B5\u30FC\u30D0\u30FC\u7BA1\u7406\u8005\u306E\u65B9\u306F\u3001\u6700\u65B0\u7248\u306ERuffle\u306B\u66F4\u65B0\u3057\u3066\u307F\u3066\u304F\u3060\u3055\u3044(\u73FE\u5728\u5229\u7528\u4E2D\u306E\u30D3\u30EB\u30C9: { $buildDate })\u3002
       *[false] \u60F3\u5B9A\u5916\u306E\u554F\u984C\u306A\u306E\u3067\u3001\u30D0\u30B0\u3068\u3057\u3066\u5831\u544A\u3057\u3066\u3044\u305F\u3060\u3051\u308B\u3068\u5B09\u3057\u3044\u3067\u3059!
    }
`,"save-manager.ftl":`save-delete-prompt = \u3053\u306E\u30BB\u30FC\u30D6\u30D5\u30A1\u30A4\u30EB\u3092\u524A\u9664\u3057\u3066\u3082\u3088\u308D\u3057\u3044\u3067\u3059\u304B?
save-reload-prompt =
    \u30BB\u30FC\u30D6\u30D5\u30A1\u30A4\u30EB\u3092\u7AF6\u5408\u306E\u53EF\u80FD\u6027\u306A\u304F { $action ->
        [delete] \u524A\u9664\u3059\u308B
       *[replace] \u7F6E\u304D\u63DB\u3048\u308B
    } \u305F\u3081\u306B\u3001\u3053\u306E\u30B3\u30F3\u30C6\u30F3\u30C4\u3092\u518D\u8AAD\u307F\u8FBC\u307F\u3059\u308B\u3053\u3068\u3092\u63A8\u5968\u3057\u307E\u3059\u3002\u7D9A\u884C\u3057\u307E\u3059\u304B\uFF1F
save-download = \u30C0\u30A6\u30F3\u30ED\u30FC\u30C9
save-replace = \u7F6E\u304D\u63DB\u3048
save-delete = \u524A\u9664
save-backup-all = \u3059\u3079\u3066\u306E\u30BB\u30FC\u30D6\u30D5\u30A1\u30A4\u30EB\u3092\u30C0\u30A6\u30F3\u30ED\u30FC\u30C9
`,"volume-controls.ftl":`volume-controls-mute = \u30DF\u30E5\u30FC\u30C8
volume-controls-unmute = \u30DF\u30E5\u30FC\u30C8\u89E3\u9664
`},"ko-KR":{"context_menu.ftl":`context-menu-download-swf = SWF \uB2E4\uC6B4\uB85C\uB4DC
context-menu-copy-debug-info = \uB514\uBC84\uADF8 \uC815\uBCF4 \uBCF5\uC0AC
context-menu-open-save-manager = \uC800\uC7A5 \uAD00\uB9AC\uC790 \uC5F4\uAE30
context-menu-about-ruffle =
    { $flavor ->
        [extension] Ruffle \uD655\uC7A5 \uD504\uB85C\uADF8\uB7A8 \uC815\uBCF4 ({ $version })
       *[other] Ruffle \uC815\uBCF4 ({ $version })
    }
context-menu-hide = \uC774 \uBA54\uB274 \uC228\uAE30\uAE30
context-menu-exit-fullscreen = \uC804\uCCB4\uD654\uBA74 \uB098\uAC00\uAE30
context-menu-enter-fullscreen = \uC804\uCCB4\uD654\uBA74\uC73C\uB85C \uC5F4\uAE30
context-menu-volume-controls = \uC74C\uB7C9 \uC870\uC808
`,"messages.ftl":`message-cant-embed = Ruffle\uC774 \uC774 \uD398\uC774\uC9C0\uC5D0 \uD3EC\uD568\uB41C \uD50C\uB798\uC2DC\uB97C \uC2E4\uD589\uD560 \uC218 \uC5C6\uC5C8\uC2B5\uB2C8\uB2E4. \uBCC4\uB3C4\uC758 \uD0ED\uC5D0\uC11C \uD30C\uC77C\uC744 \uC5F4\uC5B4\uBD04\uC73C\uB85C\uC11C \uC774 \uBB38\uC81C\uB97C \uD574\uACB0\uD560 \uC218 \uC788\uC2B5\uB2C8\uB2E4.
message-restored-from-bfcache =
    \uBE0C\uB77C\uC6B0\uC800\uAC00 \uC774\uC804 \uC138\uC158\uC5D0\uC11C \uD50C\uB798\uC2DC \uCF58\uD150\uCE20\uB97C \uBCF5\uC6D0\uD588\uC2B5\uB2C8\uB2E4.
    \uC0C8\uB85C \uC2DC\uC791\uD558\uB824\uBA74 \uD398\uC774\uC9C0\uB97C \uC0C8\uB85C \uACE0\uCE68\uD558\uC138\uC694.
panic-title = \uBB38\uC81C\uAC00 \uBC1C\uC0DD\uD588\uC2B5\uB2C8\uB2E4 :(
more-info = \uCD94\uAC00 \uC815\uBCF4
run-anyway = \uADF8\uB798\uB3C4 \uC2E4\uD589\uD558\uAE30
continue = \uACC4\uC18D\uD558\uAE30
report-bug = \uBC84\uADF8 \uC81C\uBCF4
update-ruffle = Ruffle \uC5C5\uB370\uC774\uD2B8
ruffle-demo = \uC6F9 \uB370\uBAA8
ruffle-desktop = \uB370\uC2A4\uD06C\uD1B1 \uC560\uD50C\uB9AC\uCF00\uC774\uC158
ruffle-wiki = Ruffle \uC704\uD0A4 \uBCF4\uAE30
enable-hardware-acceleration = \uD558\uB4DC\uC6E8\uC5B4 \uAC00\uC18D\uC774 \uBE44\uD65C\uC131\uD654\uB418\uC5B4 \uC788\uB294 \uAC83 \uAC19\uC2B5\uB2C8\uB2E4. Ruffle\uC740 \uACC4\uC18D \uC791\uB3D9\uD558\uC9C0\uB9CC \uB9E4\uC6B0 \uB290\uB9B4 \uC218 \uC788\uC2B5\uB2C8\uB2E4. \uC544\uB798 \uB9C1\uD06C\uB97C \uCC38\uACE0\uD558\uC5EC \uD558\uB4DC\uC6E8\uC5B4 \uAC00\uC18D\uC744 \uD65C\uC131\uD654\uD558\uB294 \uBC29\uBC95\uC744 \uCC3E\uC544\uBCF4\uC138\uC694:
enable-hardware-acceleration-link = FAQ - \uD06C\uB86C \uD558\uB4DC\uC6E8\uC5B4 \uAC00\uC18D
view-error-details = \uC624\uB958 \uC138\uBD80 \uC815\uBCF4 \uBCF4\uAE30
open-in-new-tab = \uC0C8 \uD0ED\uC5D0\uC11C \uC5F4\uAE30
click-to-unmute = \uD074\uB9AD\uD558\uC5EC \uC74C\uC18C\uAC70 \uD574\uC81C
clipboard-message-title = Ruffle\uC5D0\uC11C \uBCF5\uC0AC\uD558\uACE0 \uBD99\uC5EC\uB123\uAE30
clipboard-message-description =
    { $variant ->
       *[unsupported] \uC774 \uBE0C\uB77C\uC6B0\uC800\uB294 \uD074\uB9BD\uBCF4\uB4DC \uC561\uC138\uC2A4\uB97C \uC9C0\uC6D0\uD558\uC9C0 \uC54A\uC2B5\uB2C8\uB2E4,
        [access-denied] \uD074\uB9BD\uBCF4\uB4DC \uC561\uC138\uC2A4\uAC00 \uAC70\uC808\uB418\uC5C8\uC2B5\uB2C8\uB2E4,
    } \uD558\uC9C0\uB9CC \uB2E4\uC74C \uB2E8\uCD95\uD0A4\uB97C \uB300\uC2E0 \uC0AC\uC6A9\uD560 \uC218 \uC788\uC2B5\uB2C8\uB2E4:
clipboard-message-copy = { " " } \uBCF5\uC0AC
clipboard-message-cut = { " " } \uC798\uB77C\uB0B4\uAE30
clipboard-message-paste = { " " } \uBD99\uC5EC\uB123\uAE30
error-canvas-reload = \uCE94\uBC84\uC2A4 \uB80C\uB354\uB7EC\uAC00 \uC774\uBBF8 \uC0AC\uC6A9 \uC911\uC778 \uACBD\uC6B0 \uCE94\uBC84\uC2A4 \uB80C\uB354\uB7EC\uB85C \uB2E4\uC2DC \uB85C\uB4DC\uD560 \uC218 \uC5C6\uC2B5\uB2C8\uB2E4.
error-file-protocol =
    Ruffle\uC744 "file:" \uD504\uB85C\uD1A0\uCF5C\uC5D0\uC11C \uC2E4\uD589\uD558\uACE0 \uC788\uB294 \uAC83\uC73C\uB85C \uBCF4\uC785\uB2C8\uB2E4.
    \uBE0C\uB77C\uC6B0\uC800\uC5D0\uC11C\uB294 \uC774 \uD504\uB85C\uD1A0\uCF5C\uC744 \uBCF4\uC548\uC0C1\uC758 \uC774\uC720\uB85C \uB9CE\uC740 \uAE30\uB2A5\uC744 \uC791\uB3D9\uD558\uC9C0 \uC54A\uAC8C \uCC28\uB2E8\uD558\uBBC0\uB85C \uC774 \uBC29\uBC95\uC740 \uC791\uB3D9\uD558\uC9C0 \uC54A\uC2B5\uB2C8\uB2E4.
    \uB300\uC2E0, \uB85C\uCEEC \uC11C\uBC84\uB97C \uC9C1\uC811 \uC5F4\uC5B4\uC11C \uC124\uC815\uD558\uAC70\uB098 \uC6F9 \uB370\uBAA8 \uB610\uB294 \uB370\uC2A4\uD06C\uD1B1 \uC560\uD50C\uB9AC\uCF00\uC774\uC158\uC744 \uC0AC\uC6A9\uD558\uC2DC\uAE30 \uBC14\uB78D\uB2C8\uB2E4.
error-javascript-config =
    \uC798\uBABB\uB41C \uC790\uBC14\uC2A4\uD06C\uB9BD\uD2B8 \uC124\uC815\uC73C\uB85C \uC778\uD574 Ruffle\uC5D0\uC11C \uC911\uB300\uD55C \uBB38\uC81C\uAC00 \uBC1C\uC0DD\uD588\uC2B5\uB2C8\uB2E4.
    \uB9CC\uC57D \uB2F9\uC2E0\uC774 \uC11C\uBC84 \uAD00\uB9AC\uC790\uC778 \uACBD\uC6B0, \uC624\uB958 \uC138\uBD80\uC0AC\uD56D\uC744 \uD655\uC778\uD558\uC5EC \uC5B4\uB5A4 \uB9E4\uAC1C\uBCC0\uC218\uAC00 \uC798\uBABB\uB418\uC5C8\uB294\uC9C0 \uC54C\uC544\uBCF4\uC138\uC694.
    \uB610\uB294 Ruffle \uC704\uD0A4\uB97C \uD1B5\uD574 \uB3C4\uC6C0\uC744 \uBC1B\uC544 \uBCFC \uC218\uB3C4 \uC788\uC2B5\uB2C8\uB2E4.
error-wasm-not-found =
    Ruffle\uC774 ".wasm" \uD544\uC218 \uD30C\uC77C \uAD6C\uC131\uC694\uC18C\uB97C \uB85C\uB4DC\uD558\uC9C0 \uBABB\uD588\uC2B5\uB2C8\uB2E4.
    \uB9CC\uC57D \uB2F9\uC2E0\uC774 \uC11C\uBC84 \uAD00\uB9AC\uC790\uB77C\uBA74 \uD30C\uC77C\uC774 \uC62C\uBC14\uB974\uAC8C \uC5C5\uB85C\uB4DC\uB418\uC5C8\uB294\uC9C0 \uD655\uC778\uD558\uC138\uC694.
    \uBB38\uC81C\uAC00 \uC9C0\uC18D\uB41C\uB2E4\uBA74 "publicPath" \uC635\uC158\uC744 \uC0AC\uC6A9\uD574\uC57C \uD560 \uC218\uB3C4 \uC788\uC2B5\uB2C8\uB2E4: Ruffle \uC704\uD0A4\uB97C \uCC38\uC870\uD558\uC5EC \uB3C4\uC6C0\uC744 \uBC1B\uC73C\uC138\uC694.
error-wasm-mime-type =
    Ruffle\uC774 \uCD08\uAE30\uD654\uB97C \uC2DC\uB3C4\uD558\uB294 \uB3D9\uC548 \uC911\uB300\uD55C \uBB38\uC81C\uAC00 \uBC1C\uC0DD\uD588\uC2B5\uB2C8\uB2E4.
    \uC774 \uC6F9 \uC11C\uBC84\uB294 \uC62C\uBC14\uB978 MIME \uC720\uD615\uC758 ".wasm" \uD30C\uC77C\uC744 \uC81C\uACF5\uD558\uC9C0 \uC54A\uC2B5\uB2C8\uB2E4.
    \uB9CC\uC57D \uB2F9\uC2E0\uC774 \uC11C\uBC84 \uAD00\uB9AC\uC790\uB77C\uBA74 Ruffle \uC704\uD0A4\uB97C \uD1B5\uD574 \uB3C4\uC6C0\uC744 \uBC1B\uC73C\uC138\uC694.
error-invalid-swf =
    Ruffle\uC774 \uC694\uCCAD\uD55C \uD30C\uC77C\uC744 \uBD84\uC11D\uD558\uC9C0 \uBABB\uD588\uC2B5\uB2C8\uB2E4.
    \uC694\uCCAD\uD55C \uD30C\uC77C\uC774 \uC720\uD6A8\uD55C SWF \uD30C\uC77C\uC774 \uC544\uB2D0 \uAC00\uB2A5\uC131\uC774 \uB192\uC2B5\uB2C8\uB2E4.
error-swf-fetch =
    Ruffle\uC774 \uD50C\uB798\uC2DC SWF \uD30C\uC77C\uC744 \uB85C\uB4DC\uD558\uB294 \uB370 \uC2E4\uD328\uD558\uC600\uC2B5\uB2C8\uB2E4.
    \uC774\uB294 \uC8FC\uB85C \uD30C\uC77C\uC774 \uB354 \uC774\uC0C1 \uC874\uC7AC\uD558\uC9C0 \uC54A\uC544 Ruffle\uC774 \uB85C\uB4DC\uD560 \uC218 \uC788\uB294 \uAC83\uC774 \uC5C6\uC744 \uAC00\uB2A5\uC131\uC774 \uB192\uC2B5\uB2C8\uB2E4.
    \uC6F9\uC0AC\uC774\uD2B8 \uAD00\uB9AC\uC790\uC5D0\uAC8C \uBB38\uC758\uD558\uC5EC \uB3C4\uC6C0\uC744 \uBC1B\uC544\uBCF4\uC138\uC694.
error-swf-cors =
    Ruffle\uC774 \uD50C\uB798\uC2DC SWF \uD30C\uC77C\uC744 \uB85C\uB4DC\uD558\uB294 \uB370 \uC2E4\uD328\uD558\uC600\uC2B5\uB2C8\uB2E4.
    CORS \uC815\uCC45\uC5D0 \uC758\uD574 \uB370\uC774\uD130 \uAC00\uC838\uC624\uAE30\uC5D0 \uB300\uD55C \uC561\uC138\uC2A4\uAC00 \uCC28\uB2E8\uB418\uC5C8\uC744 \uC218 \uC788\uC2B5\uB2C8\uB2E4.
    \uB9CC\uC57D \uB2F9\uC2E0\uC774 \uC11C\uBC84 \uAD00\uB9AC\uC790\uB77C\uBA74 Ruffle \uC704\uD0A4\uB97C \uCC38\uC870\uD558\uC5EC \uB3C4\uC6C0\uC744 \uBC1B\uC544\uBCFC \uC218 \uC788\uC2B5\uB2C8\uB2E4.
error-wasm-cors =
    Ruffle\uC774 ".wasm" \uD544\uC218 \uD30C\uC77C \uAD6C\uC131\uC694\uC18C\uB97C \uB85C\uB4DC\uD558\uC9C0 \uBABB\uD588\uC2B5\uB2C8\uB2E4.
    CORS \uC815\uCC45\uC5D0 \uC758\uD574 \uB370\uC774\uD130 \uAC00\uC838\uC624\uAE30\uC5D0 \uB300\uD55C \uC561\uC138\uC2A4\uAC00 \uCC28\uB2E8\uB418\uC5C8\uC744 \uC218 \uC788\uC2B5\uB2C8\uB2E4.
    \uB9CC\uC57D \uB2F9\uC2E0\uC774 \uC11C\uBC84 \uAD00\uB9AC\uC790\uB77C\uBA74 Ruffle \uC704\uD0A4\uB97C \uCC38\uC870\uD558\uC5EC \uB3C4\uC6C0\uC744 \uBC1B\uC544\uBCFC \uC218 \uC788\uC2B5\uB2C8\uB2E4.
error-wasm-invalid =
    Ruffle\uC774 \uCD08\uAE30\uD654\uB97C \uC2DC\uB3C4\uD558\uB294 \uB3D9\uC548 \uC911\uB300\uD55C \uBB38\uC81C\uAC00 \uBC1C\uC0DD\uD588\uC2B5\uB2C8\uB2E4.
    \uC774 \uD398\uC774\uC9C0\uC5D0 Ruffle\uC744 \uC2E4\uD589\uD558\uAE30 \uC704\uD55C \uD30C\uC77C\uC774 \uB204\uB77D\uB418\uC5C8\uAC70\uB098 \uC798\uBABB\uB41C \uAC83 \uAC19\uC2B5\uB2C8\uB2E4.
    \uB9CC\uC57D \uB2F9\uC2E0\uC774 \uC11C\uBC84 \uAD00\uB9AC\uC790\uB77C\uBA74 Ruffle \uC704\uD0A4\uB97C \uCC38\uC870\uD558\uC5EC \uB3C4\uC6C0\uC744 \uBC1B\uC544\uBCFC \uC218 \uC788\uC2B5\uB2C8\uB2E4.
error-wasm-download =
    Ruffle\uC774 \uCD08\uAE30\uD654\uB97C \uC2DC\uB3C4\uD558\uB294 \uB3D9\uC548 \uC911\uB300\uD55C \uBB38\uC81C\uAC00 \uBC1C\uC0DD\uD588\uC2B5\uB2C8\uB2E4.
    \uC774 \uBB38\uC81C\uB294 \uB54C\uB54C\uB85C \uBC14\uB85C \uD574\uACB0\uB420 \uC218 \uC788\uC73C\uBBC0\uB85C \uD398\uC774\uC9C0\uB97C \uC0C8\uB85C\uACE0\uCE68\uD558\uC5EC \uB2E4\uC2DC \uC2DC\uB3C4\uD574\uBCF4\uC138\uC694.
    \uADF8\uB798\uB3C4 \uBB38\uC81C\uAC00 \uC9C0\uC18D\uB41C\uB2E4\uBA74, \uC6F9\uC0AC\uC774\uD2B8 \uAD00\uB9AC\uC790\uC5D0\uAC8C \uBB38\uC758\uD574\uC8FC\uC138\uC694.
error-wasm-disabled-on-edge =
    Ruffle\uC774 ".wasm" \uD544\uC218 \uD30C\uC77C \uAD6C\uC131\uC694\uC18C\uB97C \uB85C\uB4DC\uD558\uC9C0 \uBABB\uD588\uC2B5\uB2C8\uB2E4.
    \uC774\uB97C \uD574\uACB0\uD558\uB824\uBA74 \uBE0C\uB77C\uC6B0\uC800 \uC124\uC815\uC5D0\uC11C "\uAC1C\uC778 \uC815\uBCF4, \uAC80\uC0C9 \uBC0F \uC11C\uBE44\uC2A4"\uB97C \uD074\uB9AD\uD55C \uD6C4, \uD558\uB2E8\uC73C\uB85C \uC2A4\uD06C\uB864\uD558\uC5EC "\uC6F9\uC5D0\uC11C \uBCF4\uC548 \uAC15\uD654" \uAE30\uB2A5\uC744 \uAEBC\uC57C \uD569\uB2C8\uB2E4.
    \uC774\uB294 \uD544\uC694\uD55C ".wasm" \uD30C\uC77C\uC744 \uBE0C\uB77C\uC6B0\uC800\uC5D0\uC11C \uB85C\uB4DC\uD560 \uC218 \uC788\uB3C4\uB85D \uD5C8\uC6A9\uD569\uB2C8\uB2E4.
    \uC774 \uBB38\uC81C\uAC00 \uC9C0\uC18D\uB420 \uACBD\uC6B0 \uB2E4\uB978 \uBE0C\uB77C\uC6B0\uC800\uB97C \uC0AC\uC6A9\uD574\uC57C \uD560 \uC218 \uC788\uC2B5\uB2C8\uB2E4.
error-wasm-unsupported-browser =
    \uC0AC\uC6A9 \uC911\uC778 \uBE0C\uB77C\uC6B0\uC800\uC5D0\uC11C Ruffle\uC774 \uD544\uC694\uD55C \uC6F9 \uC5B4\uC148\uBE14\uB9AC \uD655\uC7A5\uC744 \uC9C0\uC6D0\uD558\uC9C0 \uC54A\uC2B5\uB2C8\uB2E4.
    \uC9C0\uC6D0\uB418\uB294 \uBE0C\uB77C\uC6B0\uC800\uB85C \uC804\uD658\uD558\uC138\uC694. \uC9C0\uC6D0\uB418\uB294 \uBE0C\uB77C\uC6B0\uC800 \uBAA9\uB85D\uC740 \uC704\uD0A4\uC5D0\uC11C \uD655\uC778\uD560 \uC218 \uC788\uC2B5\uB2C8\uB2E4.
error-javascript-conflict =
    Ruffle\uC774 \uCD08\uAE30\uD654\uB97C \uC2DC\uB3C4\uD558\uB294 \uB3D9\uC548 \uC911\uB300\uD55C \uBB38\uC81C\uAC00 \uBC1C\uC0DD\uD588\uC2B5\uB2C8\uB2E4.
    \uC774 \uD398\uC774\uC9C0\uC5D0\uC11C \uC0AC\uC6A9\uB418\uB294 \uC790\uBC14\uC2A4\uD06C\uB9BD\uD2B8 \uCF54\uB4DC\uAC00 Ruffle\uACFC \uCDA9\uB3CC\uD558\uB294 \uAC83\uC73C\uB85C \uBCF4\uC785\uB2C8\uB2E4.
    \uB9CC\uC57D \uB2F9\uC2E0\uC774 \uC11C\uBC84 \uAD00\uB9AC\uC790\uB77C\uBA74 \uBE48 \uD398\uC774\uC9C0\uC5D0\uC11C \uD30C\uC77C\uC744 \uB85C\uB4DC\uD574\uBCF4\uC138\uC694.
error-javascript-conflict-outdated = \uB610\uD55C Ruffle\uC758 \uCD5C\uC2E0 \uBC84\uC804\uC744 \uC5C5\uB85C\uB4DC\uD558\uB294 \uAC83\uC744 \uC2DC\uB3C4\uD558\uC5EC \uBB38\uC81C\uB97C \uC6B0\uD68C\uD574\uBCFC \uC218 \uC788\uC2B5\uB2C8\uB2E4. (\uD604\uC7AC \uBE4C\uB4DC\uAC00 \uC624\uB798\uB418\uC5C8\uC2B5\uB2C8\uB2E4: { $buildDate }).
error-csp-conflict =
    Ruffle\uC774 \uCD08\uAE30\uD654\uB97C \uC2DC\uB3C4\uD558\uB294 \uB3D9\uC548 \uC911\uB300\uD55C \uBB38\uC81C\uAC00 \uBC1C\uC0DD\uD588\uC2B5\uB2C8\uB2E4.
    \uC774 \uC6F9 \uC11C\uBC84\uC758 CSP(Content Security Policy) \uC815\uCC45\uC774 ".wasm" \uD544\uC218 \uAD6C\uC131\uC694\uC18C\uB97C \uC2E4\uD589\uD558\uB294 \uAC83\uC744 \uD5C8\uC6A9\uD558\uC9C0 \uC54A\uC2B5\uB2C8\uB2E4.
    \uB9CC\uC57D \uB2F9\uC2E0\uC774 \uC11C\uBC84 \uAD00\uB9AC\uC790\uB77C\uBA74 Ruffle \uC704\uD0A4\uB97C \uCC38\uC870\uD558\uC5EC \uB3C4\uC6C0\uC744 \uBC1B\uC544\uBCFC \uC218 \uC788\uC2B5\uB2C8\uB2E4.
error-unknown =
    Ruffle\uC774 \uD50C\uB798\uC2DC \uCF58\uD150\uCE20\uB97C \uD45C\uC2DC\uD558\uB824\uACE0 \uC2DC\uB3C4\uD558\uB294 \uB3D9\uC548 \uC911\uB300\uD55C \uBB38\uC81C\uAC00 \uBC1C\uC0DD\uD588\uC2B5\uB2C8\uB2E4.
    { $outdated ->
        [true] \uB9CC\uC57D \uB2F9\uC2E0\uC774 \uC11C\uBC84 \uAD00\uB9AC\uC790\uB77C\uBA74, Ruffle\uC758 \uCD5C\uC2E0 \uBC84\uC804\uC744 \uC5C5\uB85C\uB4DC\uD558\uC5EC \uB2E4\uC2DC \uC2DC\uB3C4\uD574\uBCF4\uC138\uC694. (\uD604\uC7AC \uBE4C\uB4DC\uAC00 \uC624\uB798\uB418\uC5C8\uC2B5\uB2C8\uB2E4: { $buildDate }).
       *[false] \uC774\uB7F0 \uD604\uC0C1\uC774 \uBC1C\uC0DD\uD574\uC11C\uB294 \uC548\uB418\uBBC0\uB85C, \uBC84\uADF8\uB97C \uC81C\uBCF4\uD574\uC8FC\uC2E0\uB2E4\uBA74 \uAC10\uC0AC\uD558\uACA0\uC2B5\uB2C8\uB2E4!
    }
`,"save-manager.ftl":`save-delete-prompt = \uC815\uB9D0\uB85C \uC774 \uC138\uC774\uBE0C \uD30C\uC77C\uC744 \uC0AD\uC81C\uD558\uC2DC\uACA0\uC2B5\uB2C8\uAE4C?
save-reload-prompt =
    \b\uC774 \uD30C\uC77C\uC744 \uC7A0\uC7AC\uC801\uC778 \uCDA9\uB3CC \uC5C6\uC774 { $action ->
        [delete] \uC0AD\uC81C
       *[replace] \uAD50\uCCB4
    }\uD558\uB824\uBA74 \uCF58\uD150\uCE20\uB97C \uB2E4\uC2DC \uB85C\uB4DC\uD574\uC57C \uD569\uB2C8\uB2E4. \uADF8\uB798\uB3C4 \uACC4\uC18D\uD558\uC2DC\uACA0\uC2B5\uB2C8\uAE4C?
save-download = \uB2E4\uC6B4\uB85C\uB4DC
save-replace = \uAD50\uCCB4
save-delete = \uC0AD\uC81C
save-backup-all = \uBAA8\uB4E0 \uC800\uC7A5 \uD30C\uC77C \uB2E4\uC6B4\uB85C\uB4DC
`,"volume-controls.ftl":`volume-controls-mute = \uC74C\uC18C\uAC70
volume-controls-unmute = \uC74C\uC18C\uAC70 \uD574\uC81C
`},"nb-NO":{"context_menu.ftl":`context-menu-download-swf = Last ned SWF
context-menu-copy-debug-info = Kopier feils\xF8kningsinfo
context-menu-open-save-manager = \xC5pne lagringsadministrasjon
context-menu-about-ruffle =
    { $flavor ->
        [extension] Om Ruffle-tillegget ({ $version })
       *[other] Om Ruffle ({ $version })
    }
context-menu-hide = Skjul denne menyen
context-menu-exit-fullscreen = Avslutt fullskjermmodus
context-menu-enter-fullscreen = Fullskjermmodus
context-menu-volume-controls = Justering av lydniv\xE5
`,"messages.ftl":"","save-manager.ftl":`save-delete-prompt = Er du sikker p\xE5 at du vil slette filen?
save-download = Last ned
save-replace = Erstatt
save-delete = Slett
`,"volume-controls.ftl":`volume-controls-mute = Demp
volume-controls-unmute = Skru p\xE5 lyd
`},"nl-NL":{"context_menu.ftl":`context-menu-download-swf = SWF downloaden
context-menu-copy-debug-info = Kopieer debuginformatie
context-menu-open-save-manager = Open opgeslagen-data-manager
context-menu-about-ruffle =
    { $flavor ->
        [extension] Over Ruffle Uitbreiding ({ $version })
       *[other] Over Ruffle ({ $version })
    }
context-menu-hide = Verberg dit menu
context-menu-exit-fullscreen = Verlaat volledig scherm
context-menu-enter-fullscreen = Naar volledig scherm
context-menu-volume-controls = Volumeregelaars
`,"messages.ftl":`message-cant-embed =
    Ruffle kon de Flash-inhoud op de pagina niet draaien.
    Je kan proberen het bestand in een apart tabblad te openen, om hier omheen te werken.
message-restored-from-bfcache =
    Je browser heeft deze Flash-inhoud uit een eerdere sessie hersteld.
    Herlaad de pagina voor een frisse start.
panic-title = Er ging iets mis :(
more-info = Meer informatie
run-anyway = Toch starten
continue = Doorgaan
report-bug = Bug rapporteren
update-ruffle = Ruffle updaten
ruffle-demo = Web Demo
ruffle-desktop = Desktopapplicatie
ruffle-wiki = Bekijk de Ruffle Wiki
enable-hardware-acceleration = Het lijkt erop dat hardwareversnelling is uitgeschakeld. Ruffle zou hierdoor erg traag kunnen zijn. In de link hieronder wordt uitgelegd hoe je hardwareversnelling kunt inschakelen:
enable-hardware-acceleration-link = FAQ - Chrome Hardwareversnelling
view-error-details = Foutdetails tonen
open-in-new-tab = Openen in een nieuw tabblad
click-to-unmute = Klik om te ontdempen
clipboard-message-title = Kopi\xEBren en plakken in Ruffle
clipboard-message-description =
    { $variant ->
       *[unsupported] Je browser heeft geen ondersteuning voor volledige toegang tot het klembord,
        [access-denied] Toegang tot het klembord werd geweigerd,
    } maar je kunt altijd nog de volgende sneltoetsen gebruiken:
clipboard-message-copy = { " " } om te kopi\xEBren
clipboard-message-cut = { " " } om te knippen
clipboard-message-paste = { " " } om te plakken
error-canvas-reload = De canvas renderer kan niet herladen worden wanneer deze al in gebruik is.
error-file-protocol =
    Het lijkt erop dat je Ruffle gebruikt met het "file" protocol.
    De meeste browsers blokkeren dit om veiligheidsredenen, waardoor het niet werkt.
    In plaats hiervan raden we aan om een lokale server te draaien, de web demo te gebruiken, of de desktopapplicatie.
error-javascript-config =
    Ruffle heeft een groot probleem ondervonden vanwege een onjuiste JavaScript configuratie.
    Als je de serverbeheerder bent, kijk dan naar de foutdetails om te zien wat er verkeerd is.
    Je kan ook in de Ruffle wiki kijken voor hulp.
error-wasm-not-found =
    Ruffle kon het vereiste ".wasm" bestandscomponent niet laden.
    Als je de serverbeheerder bent, controleer dan of het bestaand juist is ge\xFCpload.
    Mocht het probleem blijven voordoen, moet je misschien de "publicPath" instelling gebruiken: zie ook de Ruffle wiki voor hulp.
error-wasm-mime-type =
    Ruffle heeft een groot probleem ondervonden tijdens het initialiseren.
    Deze webserver serveert ".wasm" bestanden niet met het juiste MIME type.
    Als je de serverbeheerder bent, kijk dan in de Ruffle wiki voor hulp.
error-invalid-swf =
    Ruffle kon het gevraagde bestand niet verwerken.
    Waarschijnlijk is het geen geldig SWF bestand.
error-swf-fetch =
    Ruffle kon het Flash SWF bestand niet inladen.
    De meest waarschijnlijke reden is dat het bestand niet langer bestaat, en er dus niets is om in te laden.
    Probeer contact op te nemen met de websitebeheerder voor hulp.
error-swf-cors =
    Ruffle kon het Flash SWD bestand niet inladen.
    Toegang is waarschijnlijk geblokeerd door het CORS beleid.
    Als je de serverbeheerder bent, kijk dan in de Ruffle wiki voor hulp.
error-wasm-cors =
    Ruffle kon het vereiste ".wasm" bestandscomponent niet laden.
    Toegang is waarschijnlijk geblokeerd door het CORS beleid.
    Als je de serverbeheerder bent, kijk dan in de Ruffle wiki voor hulp.
error-wasm-invalid =
    Ruffle heeft een groot probleem ondervonden tijdens het initialiseren.
    Het lijkt erop dat de Ruffle bestanden ontbreken of ongeldig zijn.
    Als je de serverbeheerder bent, kijk dan in de Ruffle wiki voor hulp.
error-wasm-download =
    Ruffle heeft een groot probleem ondervonden tijdens het initialiseren.
    Dit lost zichzelf vaak op als je de bladzijde opnieuw inlaadt.
    Zo niet, neem dan contact op met de websitebeheerder.
error-wasm-disabled-on-edge =
    Ruffle kon het vereiste ".wasm" bestandscomponent niet laden.
    Om dit op te lossen, ga naar je browserinstellingen, klik op "Privacy, zoeken en diensten", scroll omlaag, en schakel "Verbeter je veiligheid op he web" uit.
    Dan kan je browser wel de vereiste ".wasm" bestanden inladen.
    Als het probleem zich blijft voordoen, moet je misschien een andere browser gebruiken.
error-wasm-unsupported-browser =
    De browser die je gebruikt ondersteunt de WebAssembly extensies die Ruffle nodig heeft niet.
    Gebruik alsjeblieft een ondersteunde browser.
    Je kunt een lijst aan ondersteunde browsers vinden op de Wiki.
error-javascript-conflict =
    Ruffle heeft een groot probleem ondervonden tijdens het initialiseren.
    Het lijkt erop dat deze pagina JavaScript code gebruikt die conflicteert met Ruffle.
    Als je de serverbeheerder bent, raden we aan om het bestand op een lege pagina te proberen in te laden.
error-javascript-conflict-outdated = Je kan ook proberen een nieuwe versie van Ruffle te installeren, om om het probleem heen te werken (huidige versie is oud: { $buildDate }).
error-csp-conflict =
    Ruffle heeft een groot probleem ondervonden tijdens het initialiseren.
    Het CSP-beleid staat niet toe dat het vereiste ".wasm" component kan draaien.
    Als je de serverbeheerder bent, kijk dan in de Ruffle wiki voor hulp.
error-unknown =
    Ruffle heeft een groot probleem onderbonden tijdens het weergeven van deze Flash-inhoud.
    { $outdated ->
        [true] Als je de serverbeheerder bent, upload dan een nieuwe versie van Ruffle (huidige versie is oud: { $buildDate }).
       *[false] Dit hoort niet te gebeuren, dus we stellen het op prijs als je de fout aan ons rapporteert!
    }
`,"save-manager.ftl":`save-delete-prompt = Weet je zeker dat je deze opgeslagen data wilt verwijderen?
save-reload-prompt =
    De enige manier om deze opgeslagen data te { $action ->
        [delete] verwijderen
       *[replace] vervangen
    } zonder potenti\xEBle problemen is door de inhoud opnieuw te laden. Toch doorgaan?
save-download = Downloaden
save-replace = Vervangen
save-delete = Verwijderen
save-backup-all = Download alle opgeslagen data
`,"volume-controls.ftl":`volume-controls-mute = Dempen
volume-controls-unmute = Dempen opheffen
`},"pl-PL":{"context_menu.ftl":`context-menu-download-swf = Pobierz SWF
context-menu-copy-debug-info = Kopiuj informacje debugowania
context-menu-open-save-manager = Otw\xF3rz menad\u017Cer zapis\xF3w
context-menu-about-ruffle =
    { $flavor ->
        [extension] O rozszerzeniu Ruffle ({ $version })
       *[other] O Ruffle ({ $version })
    }
context-menu-hide = Ukryj to menu
context-menu-exit-fullscreen = Opu\u015B\u0107 tryb pe\u0142noekranowy
context-menu-enter-fullscreen = W\u0142\u0105cz tryb pe\u0142noekranowy
context-menu-volume-controls = Sterowanie g\u0142o\u015Bno\u015Bci\u0105
`,"messages.ftl":`message-cant-embed =
    Ruffle nie by\u0142o w stanie uruchomi\u0107 zawarto\u015Bci Flash w tej stronie.
    Mo\u017Cesz spr\xF3bowa\u0107 otworzy\u0107 plik w nowej karcie, aby unikn\u0105\u0107 tego problemu.
message-restored-from-bfcache =
    Twoja przegl\u0105darka przywr\xF3ci\u0142a t\u0119 zawarto\u015B\u0107 Flash z poprzedniej sesji.
    Aby zacz\u0105\u0107 od nowa, od\u015Bwie\u017C stron\u0119.
panic-title = Co\u015B posz\u0142o nie tak :(
more-info = Wi\u0119cej informacji
run-anyway = Uruchom mimo tego
continue = Kontynuuj
report-bug = Zg\u0142o\u015B b\u0142\u0105d
update-ruffle = Zaktualizuj Ruffle
ruffle-demo = Webowe demo
ruffle-desktop = Aplikacja na komputer
ruffle-wiki = Zobacz Wiki Ruffle
enable-hardware-acceleration = Wygl\u0105da na to, \u017Ce akceleracja grafiki jest wy\u0142\u0105czona. Chocia\u017C Ruffle mo\u017Ce dzia\u0142a\u0107, mo\u017Ce by\u0107 bardzo powolny. Mo\u017Cesz dowiedzie\u0107 si\u0119, jak w\u0142\u0105czy\u0107 akceleracj\u0119 grafiki, klikaj\u0105c poni\u017Cszy link:
enable-hardware-acceleration-link = FAQ \u2014 Akceleracja Grafiki Chrome
view-error-details = Zobacz szczeg\xF3\u0142y b\u0142\u0119du
open-in-new-tab = Otw\xF3rz w nowej karcie
click-to-unmute = Kliknij aby wy\u0142\u0105czy\u0107 wyciszenie
clipboard-message-title = Kopiowanie i wklejanie w Ruffle
clipboard-message-description =
    { $variant ->
       *[unsupported] Twoja przegl\u0105darka nie obs\u0142uguje pe\u0142nego dost\u0119pu do schowka,
        [access-denied] Odm\xF3wiono dost\u0119pu do schowka,
    } ale zawsze mo\u017Cesz stosowa\u0107 te skr\xF3ty klawiszowe:
clipboard-message-copy = { " " } w celu skopiowania
clipboard-message-cut = { " " } w celu wyci\u0119cia
clipboard-message-paste = { " " } w celu wklejenia
error-canvas-reload = Nie mo\u017Cna ponownie za\u0142adowa\u0107 renderera canvas, gdy jest ju\u017C on u\u017Cywany.
error-file-protocol =
    Wygl\u0105da na to, \u017Ce u\u017Cywasz Ruffle z protoko\u0142em "file:".
    To nie dzia\u0142a, poniewa\u017C przegl\u0105darka blokuje wiele funkcji przed dzia\u0142aniem ze wzgl\u0119d\xF3w bezpiecze\u0144stwa.
    Zamiast tego zach\u0119camy do konfiguracji lokalnego serwera lub u\u017Cycia webowego demo lub aplikacji desktopowej.
error-javascript-config =
    Ruffle napotka\u0142 powa\u017Cny problem z powodu nieprawid\u0142owej konfiguracji JavaScript.
    Je\u015Bli jeste\u015B administratorem serwera, prosimy o sprawdzenie szczeg\xF3\u0142\xF3w b\u0142\u0119du, aby dowiedzie\u0107 si\u0119, kt\xF3ry parametr jest b\u0142\u0119dny.
    Mo\u017Cesz r\xF3wnie\u017C zapozna\u0107 si\u0119 z wiki Ruffle, aby uzyska\u0107 pomoc.
error-wasm-not-found =
    Nie uda\u0142o si\u0119 za\u0142adowa\u0107 wymaganego komponentu pliku ".wasm".
    Je\u015Bli jeste\u015B administratorem serwera, upewnij si\u0119, \u017Ce plik zosta\u0142 poprawnie przes\u0142any.
    Je\u015Bli problem b\u0119dzie si\u0119 powtarza\u0142, by\u0107 mo\u017Ce b\u0119dziesz musia\u0142 u\u017Cy\u0107 ustawienia "publicPath": zapoznaj si\u0119 z wiki Ruffle, aby uzyska\u0107 pomoc.
error-wasm-mime-type =
    Ruffle napotka\u0142 powa\u017Cny problem podczas pr\xF3by zainicjowania.
    Ten serwer nie serwuje plik\xF3w ".wasm" z poprawnym typem MIME.
    Je\u015Bli jeste\u015B administratorem serwera, zasi\u0119gnij pomocy na wiki Ruffle.
error-invalid-swf =
    Ruffle nie mo\u017Ce przetworzy\u0107 \u017C\u0105danego pliku.
    Prawdopodobnie to nie jest poprawny plik SWF.
error-swf-fetch =
    Nie uda\u0142o si\u0119 za\u0142adowa\u0107 pliku Flash SWF.
    Najbardziej prawdopodobnym powodem jest to, \u017Ce plik ju\u017C nie istnieje, wi\u0119c Ruffle nie ma co za\u0142adowa\u0107.
    Spr\xF3buj skontaktowa\u0107 si\u0119 z administratorem witryny, aby uzyska\u0107 pomoc.
error-swf-cors =
    Nie uda\u0142o si\u0119 za\u0142adowa\u0107 pliku Flash SWF.
    Pobieranie zosta\u0142o prawdopodobnie zablokowane przez polityk\u0119 CORS.
    Je\u015Bli jeste\u015B administratorem serwera, zasi\u0119gnij pomocy na wiki Ruffle.
error-wasm-cors =
    Nie uda\u0142o si\u0119 za\u0142adowa\u0107 wymaganego komponentu pliku ".wasm".
    Pobieranie zosta\u0142o prawdopodobnie zablokowane przez polityk\u0119 CORS.
    Je\u015Bli jeste\u015B administratorem serwera, zasi\u0119gnij pomocy na wiki Ruffle.
error-wasm-invalid =
    Ruffle napotka\u0142 powa\u017Cny problem podczas pr\xF3by zainicjowania.
    Wygl\u0105da na to, \u017Ce ta strona ma brakuj\u0105ce lub nieprawid\u0142owe pliki niezb\u0119dne do uruchomienia Ruffle.
    Je\u015Bli jeste\u015B administratorem serwera, zasi\u0119gnij pomocy na wiki Ruffle.
error-wasm-download =
    Ruffle napotka\u0142 powa\u017Cny problem podczas pr\xF3by zainicjowania.
    Ten problem cz\u0119sto sam si\u0119 rozwi\u0105zuje, wi\u0119c mo\u017Cesz spr\xF3bowa\u0107 od\u015Bwie\u017Cy\u0107 stron\u0119.
    W przeciwnym razie skontaktuj si\u0119 z administratorem witryny.
error-wasm-disabled-on-edge =
    Ruffle nie uda\u0142o si\u0119 za\u0142adowa\u0107 wymaganego komponentu pliku ".wasm".
    Aby to naprawi\u0107, spr\xF3buj otworzy\u0107 ustawienia przegl\u0105darki, klikaj\u0105c "Prywatno\u015B\u0107, wyszukiwanie i us\u0142ugi", przewijaj\u0105c w d\xF3\u0142 i wy\u0142\u0105czaj\u0105c "Zwi\u0119ksz bezpiecze\u0144stwo w sieci".
    Pozwoli to przegl\u0105darce za\u0142adowa\u0107 wymagane pliki ".wasm".
    Je\u015Bli problem b\u0119dzie si\u0119 powtarza\u0142, by\u0107 mo\u017Ce b\u0119dziesz musia\u0142 u\u017Cy\u0107 innej przegl\u0105darki.
error-wasm-unsupported-browser =
    Przegl\u0105darka, kt\xF3rej u\u017Cywasz, nie obs\u0142uguje rozszerze\u0144 WebAssembly wymaganych do dzia\u0142ania Ruffle.
    Prosz\u0119 u\u017Cy\u0107 obs\u0142ugiwanej przegl\u0105darki.
    List\u0119 obs\u0142ugiwanych przegl\u0105darek znajdziesz na Wiki.
error-javascript-conflict =
    Ruffle napotka\u0142 powa\u017Cny problem podczas pr\xF3by zainicjowania.
    Wygl\u0105da na to, \u017Ce ta strona u\u017Cywa kodu JavaScript, kt\xF3ry koliduje z Ruffle.
    Je\u015Bli jeste\u015B administratorem serwera, zapraszamy Ci\u0119 do \u0142adowania pliku na pustej stronie.
error-javascript-conflict-outdated = Mo\u017Cesz r\xF3wnie\u017C spr\xF3bowa\u0107 przes\u0142a\u0107 nowsz\u0105 wersj\u0119 Ruffle, kt\xF3ra mo\u017Ce omin\u0105\u0107 problem (obecna wersja jest przestarza\u0142a: { $buildDate }).
error-csp-conflict =
    Ruffle napotka\u0142 powa\u017Cny problem podczas pr\xF3by zainicjowania.
    Polityka bezpiecze\u0144stwa zawarto\u015Bci tego serwera (CSP) nie zezwala na komponent ".wasm" wymagany do uruchomienia.
    Je\u015Bli jeste\u015B administratorem serwera, zasi\u0119gnij pomocy na wiki Ruffle.
error-url-invalid =
    Ruffle nie za\u0142adowa\u0142 pliku SWF Flash.
    Najprawdopodobniejsz\u0105 przyczyn\u0105 jest przekazanie do Ruffle nieprawid\u0142owego adresu URL pliku SWF.
error-unknown =
    Ruffle napotka\u0142 powa\u017Cny problem podczas pr\xF3by wy\u015Bwietlenia tej zawarto\u015Bci Flash.
    { $outdated ->
        [true] Je\u015Bli jeste\u015B administratorem serwera, spr\xF3buj zaktualizowa\u0107 Ruffle (obecna wersja jest przestarza\u0142a: { $buildDate }).
       *[false] To nie powinno si\u0119 wydarzy\u0107, wi\u0119c byliby\u015Bmy wdzi\u0119czni, gdyby\u015B zg\u0142osi\u0142 b\u0142\u0105d!
    }
`,"save-manager.ftl":`save-delete-prompt = Czy na pewno chcesz skasowa\u0107 ten plik zapisu?
save-reload-prompt =
    Jedyn\u0105 opcj\u0105, aby { $action ->
        [delete] usun\u0105\u0107
       *[replace] zamieni\u0107
    } ten plik zapisu bez potencjalnych konflikt\xF3w jest prze\u0142adowanie zawarto\u015Bci. Czy chcesz kontynuowa\u0107?
save-download = Pobierz
save-replace = Zamie\u0144
save-delete = Usu\u0144
save-backup-all = Pobierz wszystkie pliki zapisu
`,"volume-controls.ftl":`volume-controls-mute = Wycisz
volume-controls-unmute = Wy\u0142\u0105cz wyciszenie
`},"pt-BR":{"context_menu.ftl":`context-menu-download-swf = Baixar SWF
context-menu-copy-debug-info = Copiar informa\xE7\xE3o de depura\xE7\xE3o
context-menu-open-save-manager = Abrir o gerenciador de salvamento
context-menu-about-ruffle =
    { $flavor ->
        [extension] Sobre a extens\xE3o do Ruffle ({ $version })
       *[other] Sobre o Ruffle ({ $version })
    }
context-menu-hide = Esconder este menu
context-menu-exit-fullscreen = Sair da tela cheia
context-menu-enter-fullscreen = Entrar em tela cheia
context-menu-volume-controls = Controles de volume
`,"messages.ftl":`message-cant-embed =
    Ruffle n\xE3o conseguiu executar o Flash incorporado nesta p\xE1gina.
    Voc\xEA pode tentar abrir o arquivo em uma guia separada para evitar esse problema.
message-restored-from-bfcache =
    Seu navegador restaurou este conte\xFAdo Flash de uma sess\xE3o anterior.
    Para come\xE7ar do zero, recarregue a p\xE1gina.
panic-title = Algo deu errado :(
more-info = Mais informa\xE7\xE3o
run-anyway = Executar mesmo assim
continue = Continuar
report-bug = Reportar erro
update-ruffle = Atualizar Ruffle
ruffle-demo = Demo Web
ruffle-desktop = Aplicativo de desktop
ruffle-wiki = Ver guia oficial do Ruffle
enable-hardware-acceleration = Parece que a acelera\xE7\xE3o de hardware est\xE1 desabilitada. Embora o Ruffle possa funcionar, ele pode ser muito lento. Voc\xEA pode descobrir como habilitar a acelera\xE7\xE3o de hardware seguindo o link abaixo:
enable-hardware-acceleration-link = FAQ \u2014 Acelera\xE7\xE3o de hardware no Chrome
view-error-details = Ver detalhes do erro
open-in-new-tab = Abrir em uma nova guia
click-to-unmute = Clique para ativar o som
clipboard-message-title = Copiando e colando no Ruffle
clipboard-message-description =
    { $variant ->
       *[unsupported] Seu navegador n\xE3o suporta acesso total \xE0 \xE1rea de transfer\xEAncia,
        [access-denied] O acesso \xE0 \xE1rea de transfer\xEAncia foi negado,
    } mas voc\xEA sempre pode usar estes atalhos:
clipboard-message-copy = { " " } para copiar
clipboard-message-cut = { " " } para recortar
clipboard-message-paste = { " " } para colar
error-canvas-reload = N\xE3o \xE9 poss\xEDvel recarregar com o renderizador canvas enquanto ele j\xE1 est\xE1 em uso.
error-file-protocol =
    Parece que voc\xEA est\xE1 executando o Ruffle no protocolo "file:".
    Isto n\xE3o funciona como navegadores bloqueiam muitos recursos de funcionar por raz\xF5es de seguran\xE7a.
    Ao inv\xE9s disso, convidamos voc\xEA a configurar um servidor local ou a usar a demonstra\xE7\xE3o da web, ou o aplicativo de desktop.
error-javascript-config =
    O Ruffle encontrou um grande problema devido a uma configura\xE7\xE3o incorreta do JavaScript.
    Se voc\xEA for o administrador do servidor, convidamos voc\xEA a verificar os detalhes do erro para descobrir qual par\xE2metro est\xE1 com falha.
    Voc\xEA tamb\xE9m pode consultar o guia oficial do Ruffle para obter ajuda.
error-wasm-not-found =
    Ruffle falhou ao carregar o componente de arquivo ".wasm" necess\xE1rio.
    Se voc\xEA \xE9 o administrador do servidor, por favor, certifique-se de que o arquivo foi carregado corretamente.
    Se o problema persistir, voc\xEA pode precisar usar a configura\xE7\xE3o "publicPath": por favor consulte o guia oficial do Ruffle para obter ajuda.
error-wasm-mime-type =
    Ruffle encontrou um grande problema ao tentar inicializar.
    Este servidor de web n\xE3o est\xE1 servindo ".wasm" arquivos com o tipo MIME correto.
    Se voc\xEA \xE9 o administrador do servidor, por favor consulte o guia oficial do Ruffle para obter ajuda.
error-invalid-swf =
    Ruffle n\xE3o pode analisar o arquivo solicitado.
    O motivo prov\xE1vel \xE9 que o arquivo solicitado n\xE3o seja um SWF v\xE1lido.
error-swf-fetch =
    Ruffle falhou ao carregar o arquivo Flash SWF.
    A raz\xE3o prov\xE1vel \xE9 que o arquivo n\xE3o existe mais, ent\xE3o n\xE3o h\xE1 nada para o Ruffle carregar.
    Tente contatar o administrador do site para obter ajuda.
error-swf-cors =
    O Ruffle n\xE3o conseguiu carregar o arquivo SWF do Flash.
    O acesso \xE0 requisi\xE7\xE3o provavelmente foi bloqueado pela pol\xEDtica de CORS.
    Se voc\xEA for o administrador do servidor, consulte o guia oficial do Ruffle para obter ajuda.
error-wasm-cors =
    O Ruffle n\xE3o conseguiu carregar o componente obrigat\xF3rio do arquivo \u201C.wasm\u201D.
    O acesso \xE0 busca provavelmente foi bloqueado pela pol\xEDtica de CORS.
    Se voc\xEA \xE9 o administrador do servidor, consulte o guia oficial do Ruffle para obter ajuda.
error-wasm-invalid =
    O Ruffle encontrou um erro grave ao tentar iniciar.
    Parece que esta p\xE1gina possui arquivos ausentes ou inv\xE1lidos para executar o Ruffle.
    Se voc\xEA \xE9 o administrador do servidor, consulte o guia oficial do Ruffle para obter assist\xEAncia.
error-wasm-download =
    O Ruffle encontrou um grande problema ao tentar inicializar.
    Muitas vezes isso pode se resolver sozinho, ent\xE3o voc\xEA pode tentar recarregar a p\xE1gina.
    Caso contr\xE1rio, contate o administrador do site.
error-wasm-disabled-on-edge =
    O Ruffle falhou ao carregar o componente de arquivo ".wasm" necess\xE1rio.
    Para corrigir isso, tente abrir configura\xE7\xF5es do seu navegador, clicando em "Privacidade, pesquisa e servi\xE7os", rolando para baixo e desativando "Melhore sua seguran\xE7a na web".
    Isso permitir\xE1 que seu navegador carregue os arquivos ".wasm" necess\xE1rios.
    Se o problema persistir, talvez seja necess\xE1rio usar um navegador diferente.
error-wasm-unsupported-browser =
    O navegador que voc\xEA est\xE1 usando n\xE3o oferece suporte \xE0s extens\xF5es WebAssembly necess\xE1rias para o Ruffle funcionar.
    Por favor, mude para um navegador compat\xEDvel.
    Voc\xEA pode encontrar uma lista de navegadores compat\xEDveis no guia oficial.
error-javascript-conflict =
    Ruffle encontrou um grande problema ao tentar inicializar.
    Parece que esta p\xE1gina usa c\xF3digo JavaScript que entra em conflito com o Ruffle.
    Se voc\xEA for o administrador do servidor, convidamos voc\xEA a tentar carregar o arquivo em uma p\xE1gina em branco.
error-javascript-conflict-outdated = Voc\xEA tamb\xE9m pode tentar fazer o upload de uma vers\xE3o mais recente do Ruffle que pode contornar o problema (a compila\xE7\xE3o atual est\xE1 desatualizada: { $buildDate }).
error-csp-conflict =
    O Ruffle encontrou um problema grave ao tentar iniciar.
    A Pol\xEDtica de Seguran\xE7a de Conte\xFAdo deste servidor n\xE3o permite a execu\xE7\xE3o do componente \u201C.wasm\u201D necess\xE1rio.
    Se voc\xEA for o administrador do servidor, consulte o guia oficial do Ruffle para obter ajuda.
error-url-invalid =
    O Ruffle n\xE3o conseguiu carregar o arquivo SWF do Flash.
    O motivo mais prov\xE1vel \xE9 que uma URL inv\xE1lida para o arquivo SWF foi fornecida ao Ruffle.
error-unknown =
    O Ruffle encontrou um grande problema enquanto tentava exibir este conte\xFAdo em Flash.
    { $outdated ->
        [true] Se voc\xEA \xE9 o administrador do servidor, por favor tente fazer o upload de uma vers\xE3o mais recente do Ruffle (a compila\xE7\xE3o atual est\xE1 desatualizada: { $buildDate }).
       *[false] Isso n\xE3o deveria acontecer, ent\xE3o apreciar\xEDamos muito se voc\xEA pudesse arquivar um bug!
    }
`,"save-manager.ftl":`save-delete-prompt = Tem certeza que deseja excluir este arquivo de salvamento?
save-reload-prompt =
    A \xFAnica maneira de { $action ->
        [delete] excluir
       *[replace] substituir
    } este arquivo sem potencial conflito \xE9 recarregar este conte\xFAdo. Deseja continuar mesmo assim?
save-download = Baixar
save-replace = Substituir
save-delete = Excluir
save-backup-all = Baixar todos os arquivos de salvamento
`,"volume-controls.ftl":`volume-controls-mute = Silenciar
volume-controls-unmute = Ativar som
`},"pt-PT":{"context_menu.ftl":`context-menu-download-swf = Descarga.swf
context-menu-copy-debug-info = Copiar informa\xE7\xF5es de depura\xE7\xE3o
context-menu-open-save-manager = Abrir gestor de grava\xE7\xF5es
context-menu-about-ruffle =
    { $flavor ->
        [extension] Sobre a extens\xE3o do Ruffle ({ $version })
       *[other] Sobre o Ruffle ({ $version })
    }
context-menu-hide = Esconder este menu
context-menu-exit-fullscreen = Fechar ecr\xE3 inteiro
context-menu-enter-fullscreen = Abrir ecr\xE3 inteiro
context-menu-volume-controls = Controlos de volume
`,"messages.ftl":`message-cant-embed =
    O Ruffle n\xE3o conseguiu abrir o Flash integrado nesta p\xE1gina.
    Para tentar resolver o problema, pode abrir o ficheiro num novo separador.
message-restored-from-bfcache =
    O seu navegador restaurou este conte\xFAdo Flash de uma sess\xE3o anterior.
    Para come\xE7ar do zero, recarregue a p\xE1gina.
panic-title = Algo correu mal :(
more-info = Mais informa\xE7\xF5es
run-anyway = Executar mesmo assim
continue = Continuar
report-bug = Reportar falha
update-ruffle = Atualizar o Ruffle
ruffle-demo = Demonstra\xE7\xE3o web
ruffle-desktop = Aplica\xE7\xE3o para computador
ruffle-wiki = Ver a wiki do Ruffle
enable-hardware-acceleration = Parece que a acelera\xE7\xE3o de hardware est\xE1 desativada. Mesmo que o Ruffle funcione, pode estar demasiado lento. Descubra como ativar a acelera\xE7\xE3o de hardware seguindo este link:
enable-hardware-acceleration-link = Perguntas Frequentes - Acelera\xE7\xE3o de Hardware no Chrome
view-error-details = Ver detalhes do erro
open-in-new-tab = Abrir num novo separador
click-to-unmute = Clique para ativar o som
clipboard-message-title = Copiar e colar no Ruffle
clipboard-message-description =
    { $variant ->
       *[unsupported] O seu navegador n\xE3o suporta acesso total \xE0 \xE1rea de transfer\xEAncia,
        [access-denied] O acesso \xE0 \xE1rea de transfer\xEAncia foi negado,
    } mas pode sempre usar estes atalhos em alternativa:
clipboard-message-copy = { " " } para copiar
clipboard-message-cut = { " " } para cortar
clipboard-message-paste = { " " } para colar
error-canvas-reload = N\xE3o \xE9 poss\xEDvel recarregar com o renderizador canvas quando este j\xE1 est\xE1 em uso.
error-file-protocol =
    Parece que executou o Ruffle no protocolo "file:".
    Isto n\xE3o funciona porque os navegadores bloqueiam muitas funcionalidades por seguran\xE7a.
    Em vez disto, experimente configurar um servidor local, ou ent\xE3o a usar a demonstra\xE7\xE3o web ou a aplica\xE7\xE3o para computador.
error-javascript-config =
    O Ruffle encontrou um problema grave devido a uma configura\xE7\xE3o de JavaScript incorreta.
    Se \xE9 o administrador do servidor, experimente verificar os detalhes do erro para identificar o par\xE2metro em falha.
    Pode ainda consultar a wiki do Ruffle para obter ajuda.
error-wasm-not-found =
    O Ruffle falhou ao carregar o componente de ficheiro ".wasm" necess\xE1rio.
    Se \xE9 o administrador do servidor, certifique-se de que o ficheiro foi devidamente carregado.
    Se o problema persistir, talvez queira usar a configura\xE7\xE3o "publicPath": consulte a wiki do Ruffle para obter ajuda.
error-wasm-mime-type =
    O Ruffle encontrou um problema grave ao tentar inicializar.
    Este servidor web n\xE3o est\xE1 a servir ficheiros \u201C.wasm\u201D com o tipo MIME correto.
    Se \xE9 o administrador do servidor, consulte a wiki do Ruffle para obter ajuda.
error-invalid-swf =
    O Ruffle n\xE3o consegue analisar o ficheiro solicitado.
    O mais prov\xE1vel \xE9 que o ficheiro solicitado n\xE3o seja um SWF v\xE1lido.
error-swf-fetch =
    O Ruffle falhou ao carregar o ficheiro Flash SWF.
    O mais prov\xE1vel \xE9 que o ficheiro j\xE1 n\xE3o exista, da\xED n\xE3o haver nada para o Ruffle carregar.
    Tente contactar o administrador do site para obter ajuda.
error-swf-cors =
    O Ruffle falhou ao carregar o ficheiro Flash SWF.
    Obter o ficheiro (fetch) foi provavelmente bloqueado pela pol\xEDtica CORS.
    Se \xE9 o administrador do servidor, consulte a wiki do Ruffle para obter ajuda.
error-wasm-cors =
    O Ruffle falhou ao carregar o componente de ficheiro ".wasm" necess\xE1rio.
    Obter o ficheiro (fetch) foi provavelmente bloqueado pela pol\xEDtica CORS.
    Se \xE9 o administrador do servidor, consulte a wiki do Ruffle para obter ajuda.
error-wasm-invalid =
    O Ruffle encontrou um problema grave ao tentar inicializar.
    Parece que esta p\xE1gina tem ficheiros inv\xE1lidos ou em falta para executar o Ruffle.
    Se \xE9 o administrador do servidor, consulte a wiki do Ruffle para obter ajuda.
error-wasm-download =
    O Ruffle encontrou um problema grave ao tentar inicializar.
    Isto costuma resolver-se sozinho, por isso experimente recarregar a p\xE1gina.
    Se n\xE3o acontecer, contacte o administrador do site.
error-wasm-disabled-on-edge =
    O Ruffle falhou ao carregar o componente de ficheiro ".wasm" necess\xE1rio.
    Tente corrigir isto nas defini\xE7\xF5es do navegador; clique em "Privacidade, pesquisa e servi\xE7os", deslize para baixo e desative "Melhore a sua seguran\xE7a na Web".
    Isto permitir\xE1 ao navegador carregar os ficheiros ".wasm" necess\xE1rios.
    Se o problema persistir, talvez precise de um navegador diferente.
error-wasm-unsupported-browser =
    O navegador que usa n\xE3o suporta as extens\xF5es WebAssembly de que o Ruffle necessita para executar.
    Deve mudar para um navegador suportado.
    Pode encontrar uma lista de navegadores suportados na Wiki.
error-javascript-conflict =
    O Ruffle encontrou um problema grave ao tentar inicializar.
    Parece que esta p\xE1gina usa c\xF3digo JavaScript que entra em conflito com o Ruffle.
    Se \xE9 o administrador do servidor, experimente carregar o ficheiro numa p\xE1gina em branco.
error-javascript-conflict-outdated = Pode ainda tentar carregar uma vers\xE3o mais recente do Ruffle que talvez contorne o problema (a compila\xE7\xE3o atual est\xE1 desatualizada: { $buildDate }).
error-csp-conflict =
    O Ruffle encontrou um problema grave ao tentar inicializar.
    A Pol\xEDtica de Seguran\xE7a de Conte\xFAdos deste servidor web n\xE3o permite executar o componente ".wasm" necess\xE1rio.
    Se \xE9 o administrador do servidor, consulte a wiki do Ruffle para obter ajuda.
error-unknown =
    O Ruffle encontrou um problema grave ao tentar apresentar este conte\xFAdo Flash.
    { $outdated ->
        [true] Se \xE9 o administrador do servidor, tente carregar uma vers\xE3o mais recente do Ruffle (a vers\xE3o atual est\xE1 desatualizada: { $buildDate }).
       *[false] N\xE3o era suposto ter acontecido, por isso agradec\xEDamos imenso se reportasse a falha!
    }
`,"save-manager.ftl":`save-delete-prompt = Tem a certeza de que quer eliminar esta grava\xE7\xE3o?
save-reload-prompt =
    A \xFAnica forma de { $action ->
        [delete] eliminar
       *[replace] substituir
    } esta grava\xE7\xE3o sem risco de conflito \xE9 recarregando este conte\xFAdo. Deseja continuar na mesma?
save-download = Descarregar
save-replace = Substituir
save-delete = Eliminar
save-backup-all = Descarregar todas as grava\xE7\xF5es
`,"volume-controls.ftl":`volume-controls-mute = Silenciar
volume-controls-unmute = Ativar o som
`},"ro-RO":{"context_menu.ftl":`context-menu-download-swf = Descarc\u0103 .swf
context-menu-copy-debug-info = Copiaz\u0103 informa\u021Biile de depanare
context-menu-open-save-manager = Deschide managerul de salv\u0103ri
context-menu-about-ruffle =
    { $flavor ->
        [extension] Despre extensia Ruffle ({ $version })
       *[other] Despre Ruffle ({ $version })
    }
context-menu-hide = Ascunde acest meniu
context-menu-exit-fullscreen = Ie\u0219i din ecranul complet
context-menu-enter-fullscreen = Intr\u0103 \xEEn ecran complet
context-menu-volume-controls = Comenzi pentru volum
`,"messages.ftl":`message-cant-embed =
    Ruffle nu a putut s\u0103 ruleze Flash \xEEncorporat \xEEn aceast\u0103 pagin\u0103.
    Po\u021Bi \xEEncerca s\u0103 deschizi fi\u0219ierul \xEEntr-o fil\u0103 separat\u0103, pentru a evita aceast\u0103 problem\u0103.
message-restored-from-bfcache =
    Browserul dvs. a restaurat acest con\u021Binut Flash dintr-o sesiune anterioar\u0103.
    Pentru a \xEEncepe de la zero, re\xEEnc\u0103rca\u021Bi pagina.
panic-title = Ceva a mers prost :(
more-info = Mai multe informa\u021Bii
run-anyway = Ruleaz\u0103 oricum
continue = Continu\u0103
report-bug = Raporteaz\u0103 un bug
update-ruffle = Actualizeaz\u0103 Ruffle
ruffle-demo = Demo web
ruffle-desktop = Aplica\u021Bie desktop
ruffle-wiki = Vezi wikiul Ruffle
enable-hardware-acceleration = Se pare c\u0103 accelerarea hardware este dezactivat\u0103. De\u0219i Ruffle ar putea func\u021Biona, va fi foarte lent. Pute\u021Bi afla cum s\u0103 activa\u021Bi accelerarea hardware acces\xE2nd linkul de mai jos:
enable-hardware-acceleration-link = \xCEntreb\u0103ri frecvente - Accelerarea hardware Chrome
view-error-details = Vezi detaliile erorii
open-in-new-tab = Deschide \xEEntr-o fil\u0103 nou\u0103
click-to-unmute = D\u0103 click pentru a dezmu\u021Bi
clipboard-message-title = Copierea \u0219i lipirea \xEEn Ruffle
clipboard-message-description =
    { $variant ->
       *[unsupported] Browserul dvs. nu accept\u0103 accesul complet c\u0103tre clipboard,
        [access-denied] Accesul la clipboard a fost refuzat,
    } dar pute\u021Bi oric\xE2nd s\u0103 utiliza\u021Bi aceste scurt\u0103turi:
clipboard-message-copy = { " " } pentru copiere
clipboard-message-cut = { " " } pentru decupare
clipboard-message-paste = { " " } pentru lipire
error-canvas-reload = Nu se poate re\xEEnc\u0103rca utiliz\xE2nd rendererul canvas atunci c\xE2nd acesta este deja folosit.
error-file-protocol =
    Se pare c\u0103 rulezi Ruffle pe protocolul \u201Efile:\u201D.
    Acesta nu func\u021Bioneaz\u0103, deoarece browserele blocheaz\u0103 func\u021Bionarea multor func\u021Bii din motive de securitate.
    \xCEn schimb, te invit\u0103m s\u0103 configurezi un server local sau s\u0103 folose\u0219ti fie demoul web, fie aplica\u021Bia desktop.
error-javascript-config =
    Ruffle a \xEEnt\xE2mpinat o problem\u0103 major\u0103 din cauza unei configur\u0103ri incorecte a JavaScript.
    Dac\u0103 e\u0219ti administratorul serverului, te invit\u0103m s\u0103 verifici detaliile erorii pentru a afla care parametru este defect.
    De asemenea, po\u021Bi consulta wikiul Ruffle pentru ajutor.
error-wasm-not-found =
    Ruffle a e\u0219uat la \xEEnc\u0103rcarea componentei de fi\u0219ier \u201E.wasm\u201D.
    Dac\u0103 e\u0219ti administratorul serverului, te rug\u0103m s\u0103 te asiguri c\u0103 fi\u0219ierul a fost \xEEnc\u0103rcat corect.
    Dac\u0103 problema persist\u0103, poate fi necesar s\u0103 folose\u0219ti setarea \u201EpublicPath\u201D: te rug\u0103m s\u0103 consul\u021Bi wikiul Ruffle pentru ajutor.
error-wasm-mime-type =
    Ruffle a \xEEnt\xE2mpinat o problem\u0103 major\u0103 \xEEn timp ce \xEEncerca s\u0103 se ini\u021Bializeze.
    Acest server web nu serve\u0219te fi\u0219iere \u201E.wasm\u201D cu tipul MIME corect.
    Dac\u0103 e\u0219ti administratorul serverului, te rug\u0103m s\u0103 consul\u021Bi wikiul Ruffle pentru ajutor.
error-invalid-swf =
    Ruffle nu poate analiza fi\u0219ierul solicitat.
    Cel mai probabil motiv este c\u0103 fi\u0219ierul solicitat nu este un SWF valid.
error-swf-fetch =
    Ruffle a e\u0219uat la \xEEnc\u0103rcarea fi\u0219ierului SWF.
    Motivul cel mai probabil este c\u0103 fi\u0219ierul nu mai exist\u0103, deci Ruffle nu mai are ce s\u0103 \xEEncarce.
    \xCEncearc\u0103 s\u0103 contactezi administratorul site-ului web pentru ajutor.
error-swf-cors =
    Ruffle a e\u0219uat la \xEEnc\u0103rcarea fi\u0219ierului SWF.
    Accesul de preluare a fost probabil blocat de politica CORS.
    Dac\u0103 e\u0219ti administratorul serverului, te rug\u0103m s\u0103 consul\u021Bi wikiul Ruffle pentru ajutor.
error-wasm-cors =
    Ruffle a e\u0219uat la \xEEnc\u0103rcarea componentei de fi\u0219ier \u201E.wasm\u201D.
    Accesul de preluare a fost probabil blocat de politica CORS.
    Dac\u0103 e\u0219ti administratorul serverului, te rug\u0103m s\u0103 consul\u021Bi wikiul Ruffle pentru ajutor.
error-wasm-invalid =
    Ruffle a \xEEnt\xE2mpinat o problem\u0103 major\u0103 \xEEn timp ce \xEEncerca s\u0103 se ini\u021Bializeze.
    Se pare c\u0103 aceast\u0103 pagin\u0103 are fi\u0219iere lips\u0103 sau nevalide pentru a rula Ruffle.
    Dac\u0103 e\u0219ti administratorul serverului, te rug\u0103m s\u0103 consul\u021Bi wikiul Ruffle pentru ajutor.
error-wasm-download =
    Ruffle a \xEEnt\xE2mpinat o problem\u0103 major\u0103 \xEEn timp ce \xEEncerca s\u0103 ini\u021Bializeze.
    Acest lucru se poate rezolva adesea de la sine, a\u0219a c\u0103 po\u021Bi \xEEncerca s\u0103 re\xEEncarci pagina.
    \xCEn caz contrar, te rug\u0103m s\u0103 contactezi administratorul site-ului web.
error-wasm-disabled-on-edge =
    Ruffle a e\u0219uat la \xEEnc\u0103rcarea componentei de fi\u0219ier \u201E.wasm\u201D.
    Pentru a remedia acest lucru, \xEEncearc\u0103 s\u0103 deschizi set\u0103rile browserului, s\u0103 faci clic pe \u201EConfiden\u021Bialitate, c\u0103utare \u0219i servicii\u201D, s\u0103 derulezi \xEEn jos \u0219i s\u0103 dezactivezi \u201E\xCEmbun\u0103t\u0103\u021Bi\u021Bi-v\u0103 securitatea pe web\u201D.
    Acest lucru va permite browserului s\u0103 \xEEncarce fi\u0219ierele \u201E.wasm\u201D necesare.
    Dac\u0103 problema persist\u0103, este posibil s\u0103 trebuiasc\u0103 s\u0103 folose\u0219ti un alt browser.
error-wasm-unsupported-browser =
    Browserul pe care \xEEl utiliza\u021Bi nu suport\u0103 extensiile WebAssembly pe care Ruffle le solicit\u0103 pentru a rula.
    V\u0103 rug\u0103m s\u0103 folosi\u021Bi un browser compatibil.
    Pute\u021Bi g\u0103si o list\u0103 de browsere compatibile pe Wiki.
error-javascript-conflict =
    Ruffle a \xEEnt\xE2mpinat o problem\u0103 major\u0103 \xEEn timp ce \xEEncerca s\u0103 se ini\u021Bializeze.
    Se pare c\u0103 aceast\u0103 pagin\u0103 folose\u0219te cod JavaScript care intr\u0103 \xEEn conflict cu Ruffle.
    Dac\u0103 e\u0219ti administratorul serverului, te invit\u0103m s\u0103 \xEEncerci \xEEnc\u0103rcarea fi\u0219ierului pe o pagin\u0103 goal\u0103.
error-javascript-conflict-outdated = De asemenea, po\u021Bi \xEEncerca s\u0103 \xEEncarci o versiune mai recent\u0103 de Ruffle care ar putea ocoli problema (versiunea actual\u0103 este \xEEnvechit\u0103: { $buildDate }).
error-csp-conflict =
    Ruffle a \xEEnt\xE2mpinat o problem\u0103 major\u0103 \xEEn timp ce \xEEncerca s\u0103 se ini\u021Bializeze.
    Politica de securitate a con\u021Binutului a acestui server web nu permite rularea componentei \u201E.wasm\u201D necesare.
    Dac\u0103 e\u0219ti administratorul serverului, te rug\u0103m s\u0103 consul\u021Bi wikiul Ruffle pentru ajutor.
error-url-invalid =
    Ruffle a e\u0219uat s\u0103 \xEEncarce fi\u0219ierul Flash SWF.
    Cel mai probabil motiv este c\u0103 un URL invalid pentru fi\u0219ierul SWF a fost transmis la Ruffle.
error-unknown =
    Ruffle a \xEEnt\xE2mpinat o problem\u0103 major\u0103 \xEEn timp ce \xEEncerca s\u0103 afi\u0219eze acest con\u021Binut Flash.
    { $outdated ->
        [true] Dac\u0103 e\u0219ti administratorul serverului, te rug\u0103m s\u0103 \xEEncerci s\u0103 \xEEncarci o versiune mai recent\u0103 de Ruffle (versiunea actual\u0103 este \xEEnvechit\u0103: { $buildDate }).
       *[false] Acest lucru nu ar trebui s\u0103 se \xEEnt\xE2mple, a\u0219a c\u0103 am aprecia foarte mult dac\u0103 ai putea trimite un bug!
    }
`,"save-manager.ftl":`save-delete-prompt = Sigur vrei s\u0103 \u0219tergi acest fi\u0219ier de salvare?
save-reload-prompt =
    Singura cale de a { $action ->
        [delete] \u0219terge
       *[replace] \xEEnlocui
    } acest fi\u0219ier de salvare f\u0103r\u0103 un conflict poten\u021Bial este de a re\xEEnc\u0103rca acest con\u021Binut. Dore\u0219ti s\u0103 continui oricum?
save-download = Descarc\u0103
save-replace = \xCEnlocuie\u0219te
save-delete = \u0218terge
save-backup-all = Descarc\u0103 toate fi\u0219ierele de salvare
`,"volume-controls.ftl":`volume-controls-mute = Mut
volume-controls-unmute = Activare sunet
`},"ru-RU":{"context_menu.ftl":`context-menu-download-swf = \u0421\u043A\u0430\u0447\u0430\u0442\u044C .swf
context-menu-copy-debug-info = \u041A\u043E\u043F\u0438\u0440\u043E\u0432\u0430\u0442\u044C \u043E\u0442\u043B\u0430\u0434\u043E\u0447\u043D\u0443\u044E \u0438\u043D\u0444\u043E\u0440\u043C\u0430\u0446\u0438\u044E
context-menu-open-save-manager = \u041C\u0435\u043D\u0435\u0434\u0436\u0435\u0440 \u0441\u043E\u0445\u0440\u0430\u043D\u0435\u043D\u0438\u0439
context-menu-about-ruffle =
    { $flavor ->
        [extension] \u041E \u0440\u0430\u0441\u0448\u0438\u0440\u0435\u043D\u0438\u0438 Ruffle ({ $version })
       *[other] \u041E Ruffle ({ $version })
    }
context-menu-hide = \u0421\u043A\u0440\u044B\u0442\u044C \u044D\u0442\u043E \u043C\u0435\u043D\u044E
context-menu-exit-fullscreen = \u041E\u043A\u043E\u043D\u043D\u044B\u0439 \u0440\u0435\u0436\u0438\u043C
context-menu-enter-fullscreen = \u041F\u043E\u043B\u043D\u043E\u044D\u043A\u0440\u0430\u043D\u043D\u044B\u0439 \u0440\u0435\u0436\u0438\u043C
context-menu-volume-controls = \u0413\u0440\u043E\u043C\u043A\u043E\u0441\u0442\u044C
`,"messages.ftl":`message-cant-embed =
    Ruffle \u043D\u0435 \u0441\u043C\u043E\u0433 \u0437\u0430\u043F\u0443\u0441\u0442\u0438\u0442\u044C Flash, \u0438\u0441\u043F\u043E\u043B\u044C\u0437\u0443\u0435\u043C\u044B\u0439 \u043D\u0430 \u044D\u0442\u043E\u0439 \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0435.
    \u0427\u0442\u043E\u0431\u044B \u043E\u0431\u043E\u0439\u0442\u0438 \u044D\u0442\u0443 \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u0443, \u0432\u044B \u043C\u043E\u0436\u0435\u0442\u0435 \u043F\u043E\u043F\u0440\u043E\u0431\u043E\u0432\u0430\u0442\u044C \u043E\u0442\u043A\u0440\u044B\u0442\u044C \u0444\u0430\u0439\u043B \u0432 \u043E\u0442\u0434\u0435\u043B\u044C\u043D\u043E\u0439 \u0432\u043A\u043B\u0430\u0434\u043A\u0435.
message-restored-from-bfcache =
    \u0412\u0430\u0448 \u0431\u0440\u0430\u0443\u0437\u0435\u0440 \u0432\u043E\u0441\u0441\u0442\u0430\u043D\u043E\u0432\u0438\u043B \u044D\u0442\u043E\u0442 Flash-\u043A\u043E\u043D\u0442\u0435\u043D\u0442 \u0441 \u043F\u0440\u0435\u0434\u044B\u0434\u0443\u0449\u0435\u0439 \u0441\u0435\u0441\u0441\u0438\u0438.
    \u0427\u0442\u043E\u0431\u044B \u043D\u0430\u0447\u0430\u0442\u044C \u0437\u0430\u043D\u043E\u0432\u043E, \u043F\u0435\u0440\u0435\u0437\u0430\u0433\u0440\u0443\u0437\u0438\u0442\u0435 \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0443.
panic-title = \u0427\u0442\u043E-\u0442\u043E \u043F\u043E\u0448\u043B\u043E \u043D\u0435 \u0442\u0430\u043A :(
more-info = \u041F\u043E\u0434\u0440\u043E\u0431\u043D\u0435\u0435
run-anyway = \u0412\u0441\u0451 \u0440\u0430\u0432\u043D\u043E \u0437\u0430\u043F\u0443\u0441\u0442\u0438\u0442\u044C
continue = \u041F\u0440\u043E\u0434\u043E\u043B\u0436\u0438\u0442\u044C
report-bug = \u0421\u043E\u043E\u0431\u0449\u0438\u0442\u044C \u043E\u0431 \u043E\u0448\u0438\u0431\u043A\u0435
update-ruffle = \u041E\u0431\u043D\u043E\u0432\u0438\u0442\u044C Ruffle
ruffle-demo = \u0412\u0435\u0431-\u0434\u0435\u043C\u043E
ruffle-desktop = \u041D\u0430\u0441\u0442\u043E\u043B\u044C\u043D\u043E\u0435 \u043F\u0440\u0438\u043B\u043E\u0436\u0435\u043D\u0438\u0435
ruffle-wiki = \u041E\u0442\u043A\u0440\u044B\u0442\u044C \u0432\u0438\u043A\u0438 Ruffle
enable-hardware-acceleration = \u041F\u043E\u0445\u043E\u0436\u0435, \u0447\u0442\u043E \u0430\u043F\u043F\u0430\u0440\u0430\u0442\u043D\u043E\u0435 \u0443\u0441\u043A\u043E\u0440\u0435\u043D\u0438\u0435 \u043E\u0442\u043A\u043B\u044E\u0447\u0435\u043D\u043E. \u0425\u043E\u0442\u044F Ruffle \u043C\u043E\u0436\u0435\u0442 \u0440\u0430\u0431\u043E\u0442\u0430\u0442\u044C, \u043D\u043E \u043E\u043D \u043C\u043E\u0436\u0435\u0442 \u0431\u044B\u0442\u044C \u043E\u0447\u0435\u043D\u044C \u043C\u0435\u0434\u043B\u0435\u043D\u043D\u044B\u043C. \u0412\u044B \u043C\u043E\u0436\u0435\u0442\u0435 \u0443\u0437\u043D\u0430\u0442\u044C, \u043A\u0430\u043A \u0432\u043A\u043B\u044E\u0447\u0438\u0442\u044C \u0430\u043F\u043F\u0430\u0440\u0430\u0442\u043D\u043E\u0435 \u0443\u0441\u043A\u043E\u0440\u0435\u043D\u0438\u0435, \u043F\u0435\u0440\u0435\u0439\u0434\u044F \u043F\u043E \u0441\u0441\u044B\u043B\u043A\u0435 \u043D\u0438\u0436\u0435:
enable-hardware-acceleration-link = FAQ - \u0410\u043F\u043F\u0430\u0440\u0430\u0442\u043D\u043E\u0435 \u0443\u0441\u043A\u043E\u0440\u0435\u043D\u0438\u0435 Chrome
view-error-details = \u0421\u0432\u0435\u0434\u0435\u043D\u0438\u044F \u043E\u0431 \u043E\u0448\u0438\u0431\u043A\u0435
open-in-new-tab = \u041E\u0442\u043A\u0440\u044B\u0442\u044C \u0432 \u043D\u043E\u0432\u043E\u0439 \u0432\u043A\u043B\u0430\u0434\u043A\u0435
click-to-unmute = \u0412\u043A\u043B\u044E\u0447\u0438\u0442\u044C \u0437\u0432\u0443\u043A
clipboard-message-title = \u041A\u043E\u043F\u0438\u0440\u043E\u0432\u0430\u043D\u0438\u0435 \u0438 \u0432\u0441\u0442\u0430\u0432\u043A\u0430 \u0432 Ruffle
clipboard-message-description =
    { $variant ->
       *[unsupported] \u0412\u0430\u0448 \u0431\u0440\u0430\u0443\u0437\u0435\u0440 \u043D\u0435 \u043F\u043E\u0434\u0434\u0435\u0440\u0436\u0438\u0432\u0430\u0435\u0442 \u043F\u043E\u043B\u043D\u044B\u0439 \u0434\u043E\u0441\u0442\u0443\u043F \u043A \u0431\u0443\u0444\u0435\u0440\u0443 \u043E\u0431\u043C\u0435\u043D\u0430.
        [access-denied]  \u0414\u043E\u0441\u0442\u0443\u043F \u043A \u0431\u0443\u0444\u0435\u0440\u0443 \u043E\u0431\u043C\u0435\u043D\u0430 \u0431\u044B\u043B \u043E\u0442\u043A\u043B\u043E\u043D\u0451\u043D.
    } \u0418\u0441\u043F\u043E\u043B\u044C\u0437\u0443\u0439\u0442\u0435 \u0441\u043E\u0447\u0435\u0442\u0430\u043D\u0438\u044F \u043A\u043B\u0430\u0432\u0438\u0448 \u0434\u043B\u044F \u0432\u044B\u0440\u0435\u0437\u0430\u043D\u0438\u044F, \u043A\u043E\u043F\u0438\u0440\u043E\u0432\u0430\u043D\u0438\u044F \u0438 \u0432\u0441\u0442\u0430\u0432\u043A\u0438:
clipboard-message-copy = { " " } \u043A\u043E\u043F\u0438\u0440\u043E\u0432\u0430\u0442\u044C
clipboard-message-cut = { " " } \u0432\u044B\u0440\u0435\u0437\u0430\u0442\u044C
clipboard-message-paste = { " " } \u0432\u0441\u0442\u0430\u0432\u0438\u0442\u044C
error-canvas-reload = \u041D\u0435\u0432\u043E\u0437\u043C\u043E\u0436\u043D\u043E \u043F\u0435\u0440\u0435\u0437\u0430\u0433\u0440\u0443\u0437\u0438\u0442\u044C \u0441 \u0440\u0435\u043D\u0434\u0435\u0440\u0435\u0440\u043E\u043C canvas, \u043A\u043E\u0433\u0434\u0430 \u0440\u0435\u043D\u0434\u0435\u0440\u0435\u0440 canvas \u0443\u0436\u0435 \u0438\u0441\u043F\u043E\u043B\u044C\u0437\u0443\u0435\u0442\u0441\u044F.
error-file-protocol =
    \u041F\u043E\u0445\u043E\u0436\u0435, \u0447\u0442\u043E \u0432\u044B \u0437\u0430\u043F\u0443\u0441\u043A\u0430\u0435\u0442\u0435 Ruffle \u043F\u043E \u043F\u0440\u043E\u0442\u043E\u043A\u043E\u043B\u0443 "file:".
    \u042D\u0442\u043E \u043D\u0435 \u0440\u0430\u0431\u043E\u0442\u0430\u0435\u0442, \u043F\u043E\u0441\u043A\u043E\u043B\u044C\u043A\u0443 \u0431\u0440\u0430\u0443\u0437\u0435\u0440\u044B \u0431\u043B\u043E\u043A\u0438\u0440\u0443\u044E\u0442 \u0440\u0430\u0431\u043E\u0442\u0443 \u043C\u043D\u043E\u0433\u0438\u0445 \u0444\u0443\u043D\u043A\u0446\u0438\u0439 \u043F\u043E \u0441\u043E\u043E\u0431\u0440\u0430\u0436\u0435\u043D\u0438\u044F\u043C \u0431\u0435\u0437\u043E\u043F\u0430\u0441\u043D\u043E\u0441\u0442\u0438.
    \u0412\u043C\u0435\u0441\u0442\u043E \u044D\u0442\u043E\u0433\u043E \u043C\u044B \u043F\u0440\u0435\u0434\u043B\u0430\u0433\u0430\u0435\u043C \u0432\u0430\u043C \u0438\u0441\u043F\u043E\u043B\u044C\u0437\u043E\u0432\u0430\u0442\u044C \u043D\u0430\u0441\u0442\u043E\u043B\u044C\u043D\u043E\u0435 \u043F\u0440\u0438\u043B\u043E\u0436\u0435\u043D\u0438\u0435, \u0432\u0435\u0431-\u0434\u0435\u043C\u043E \u0438\u043B\u0438 \u043D\u0430\u0441\u0442\u0440\u043E\u0438\u0442\u044C \u043B\u043E\u043A\u0430\u043B\u044C\u043D\u044B\u0439 \u0441\u0435\u0440\u0432\u0435\u0440.
error-javascript-config =
    \u0412\u043E\u0437\u043D\u0438\u043A\u043B\u0430 \u0441\u0435\u0440\u044C\u0451\u0437\u043D\u0430\u044F \u043E\u0448\u0438\u0431\u043A\u0430 \u0438\u0437-\u0437\u0430 \u043D\u0435\u043F\u0440\u0430\u0432\u0438\u043B\u044C\u043D\u043E\u0439 \u043A\u043E\u043D\u0444\u0438\u0433\u0443\u0440\u0430\u0446\u0438\u0438 JavaScript.
    \u0415\u0441\u043B\u0438 \u0432\u044B \u044F\u0432\u043B\u044F\u0435\u0442\u0435\u0441\u044C \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u043E\u043C \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043C\u044B \u043F\u0440\u0435\u0434\u043B\u0430\u0433\u0430\u0435\u043C \u0432\u0430\u043C \u043F\u0440\u043E\u0432\u0435\u0440\u0438\u0442\u044C \u0434\u0435\u0442\u0430\u043B\u0438 \u043E\u0448\u0438\u0431\u043A\u0438, \u0447\u0442\u043E\u0431\u044B \u0432\u044B\u044F\u0441\u043D\u0438\u0442\u044C, \u043A\u0430\u043A\u043E\u0439 \u043F\u0430\u0440\u0430\u043C\u0435\u0442\u0440 \u0434\u0430\u043B \u0441\u0431\u043E\u0439.
    \u0412\u044B \u0442\u0430\u043A\u0436\u0435 \u043C\u043E\u0436\u0435\u0442\u0435 \u043E\u0431\u0440\u0430\u0442\u0438\u0442\u044C\u0441\u044F \u0437\u0430 \u043F\u043E\u043C\u043E\u0449\u044C\u044E \u043A \u0432\u0438\u043A\u0438 Ruffle.
error-wasm-not-found =
    Ruffle \u043D\u0435 \u0443\u0434\u0430\u043B\u043E\u0441\u044C \u0437\u0430\u043F\u0443\u0441\u0442\u0438\u0442\u044C \u043D\u0435\u043E\u0431\u0445\u043E\u0434\u0438\u043C\u044B\u0439 \u043A\u043E\u043C\u043F\u043E\u043D\u0435\u043D\u0442 \u0444\u0430\u0439\u043B\u0430 ".wasm".
    \u0415\u0441\u043B\u0438 \u0432\u044B \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043F\u043E\u0436\u0430\u043B\u0443\u0439\u0441\u0442\u0430, \u0443\u0431\u0435\u0434\u0438\u0442\u0435\u0441\u044C, \u0447\u0442\u043E \u0444\u0430\u0439\u043B \u0431\u044B\u043B \u0437\u0430\u0433\u0440\u0443\u0436\u0435\u043D \u043F\u0440\u0430\u0432\u0438\u043B\u044C\u043D\u043E.
    \u0415\u0441\u043B\u0438 \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u0430 \u043D\u0435 \u0443\u0441\u0442\u0440\u0430\u043D\u044F\u0435\u0442\u0441\u044F, \u0432\u0430\u043C \u043C\u043E\u0436\u0435\u0442 \u043F\u043E\u0442\u0440\u0435\u0431\u043E\u0432\u0430\u0442\u044C\u0441\u044F \u0438\u0441\u043F\u043E\u043B\u044C\u0437\u043E\u0432\u0430\u0442\u044C \u043D\u0430\u0441\u0442\u0440\u043E\u0439\u043A\u0443 "publicPath": \u043E\u0431\u0440\u0430\u0442\u0438\u0442\u0435\u0441\u044C \u043A \u0432\u0438\u043A\u0438 Ruffle.
error-wasm-mime-type =
    Ruffle \u0441\u0442\u043E\u043B\u043A\u043D\u0443\u043B\u0441\u044F \u0441 \u0441\u0435\u0440\u044C\u0451\u0437\u043D\u043E\u0439 \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u043E\u0439 \u0432\u043E \u0432\u0440\u0435\u043C\u044F \u0438\u043D\u0438\u0446\u0438\u0430\u043B\u0438\u0437\u0430\u0446\u0438\u0438.
    \u042D\u0442\u043E\u0442 \u0432\u0435\u0431-\u0441\u0435\u0440\u0432\u0435\u0440 \u043D\u0435 \u043F\u0440\u0435\u0434\u043E\u0441\u0442\u0430\u0432\u043B\u044F\u0435\u0442 \u0444\u0430\u0439\u043B\u044B ".wasm" \u0441 \u043F\u0440\u0430\u0432\u0438\u043B\u044C\u043D\u044B\u043C \u0442\u0438\u043F\u043E\u043C MIME.
    \u0415\u0441\u043B\u0438 \u0432\u044B \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043E\u0431\u0440\u0430\u0442\u0438\u0442\u0435\u0441\u044C \u0437\u0430 \u043F\u043E\u043C\u043E\u0449\u044C\u044E \u043A \u0432\u0438\u043A\u0438 Ruffle.
error-invalid-swf =
    Ruffle \u043D\u0435 \u0443\u0434\u0430\u043B\u043E\u0441\u044C \u043E\u0431\u0440\u0430\u0431\u043E\u0442\u0430\u0442\u044C \u0437\u0430\u043F\u0440\u0430\u0448\u0438\u0432\u0430\u0435\u043C\u044B\u0439 \u0444\u0430\u0439\u043B.
    \u0412\u0435\u0440\u043E\u044F\u0442\u043D\u0435\u0435 \u0432\u0441\u0435\u0433\u043E, \u0434\u0430\u043D\u043D\u044B\u0439 SWF \u043F\u043E\u0432\u0440\u0435\u0436\u0434\u0451\u043D \u0438\u043B\u0438 \u043D\u0435 \u044F\u0432\u043B\u044F\u0435\u0442\u0441\u044F \u0442\u0430\u043A\u043E\u0432\u044B\u043C.
error-swf-fetch =
    Ruffle \u043D\u0435 \u0443\u0434\u0430\u043B\u043E\u0441\u044C \u0437\u0430\u043F\u0443\u0441\u0442\u0438\u0442\u044C SWF-\u0444\u0430\u0439\u043B Flash.
    \u0412\u0435\u0440\u043E\u044F\u0442\u043D\u0435\u0435 \u0432\u0441\u0435\u0433\u043E, \u0444\u0430\u0439\u043B \u0431\u043E\u043B\u044C\u0448\u0435 \u043D\u0435 \u0441\u0443\u0449\u0435\u0441\u0442\u0432\u0443\u0435\u0442, \u043F\u043E\u044D\u0442\u043E\u043C\u0443 Ruffle \u043D\u0435\u0447\u0435\u0433\u043E \u0437\u0430\u0433\u0440\u0443\u0436\u0430\u0442\u044C.
    \u041F\u043E\u043F\u0440\u043E\u0431\u0443\u0439\u0442\u0435 \u0441\u0432\u044F\u0437\u0430\u0442\u044C\u0441\u044F \u0441 \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u043E\u043C \u0441\u0430\u0439\u0442\u0430 \u0434\u043B\u044F \u043F\u043E\u043B\u0443\u0447\u0435\u043D\u0438\u044F \u043F\u043E\u043C\u043E\u0449\u0438.
error-swf-cors =
    Ruffle \u043D\u0435 \u0443\u0434\u0430\u043B\u043E\u0441\u044C \u0437\u0430\u043F\u0443\u0441\u0442\u0438\u0442\u044C SWF-\u0444\u0430\u0439\u043B Flash.
    \u0421\u043A\u043E\u0440\u0435\u0435 \u0432\u0441\u0435\u0433\u043E, \u0434\u043E\u0441\u0442\u0443\u043F \u043A \u0444\u0430\u0439\u043B\u0443 \u0431\u044B\u043B \u0437\u0430\u0431\u043B\u043E\u043A\u0438\u0440\u043E\u0432\u0430\u043D \u043F\u043E\u043B\u0438\u0442\u0438\u043A\u043E\u0439 CORS.
    \u0415\u0441\u043B\u0438 \u0432\u044B \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043E\u0431\u0440\u0430\u0442\u0438\u0442\u0435\u0441\u044C \u0437\u0430 \u043F\u043E\u043C\u043E\u0449\u044C\u044E \u043A \u0432\u0438\u043A\u0438 Ruffle.
error-wasm-cors =
    Ruffle \u043D\u0435 \u0443\u0434\u0430\u043B\u043E\u0441\u044C \u0437\u0430\u0433\u0440\u0443\u0437\u0438\u0442\u044C \u043D\u0435\u043E\u0431\u0445\u043E\u0434\u0438\u043C\u044B\u0439 \u043A\u043E\u043C\u043F\u043E\u043D\u0435\u043D\u0442 \u0444\u0430\u0439\u043B\u0430 ".wasm".
    \u0421\u043A\u043E\u0440\u0435\u0435 \u0432\u0441\u0435\u0433\u043E, \u0434\u043E\u0441\u0442\u0443\u043F \u043A \u0444\u0430\u0439\u043B\u0443 \u0431\u044B\u043B \u0437\u0430\u0431\u043B\u043E\u043A\u0438\u0440\u043E\u0432\u0430\u043D \u043F\u043E\u043B\u0438\u0442\u0438\u043A\u043E\u0439 CORS.
    \u0415\u0441\u043B\u0438 \u0432\u044B \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043E\u0431\u0440\u0430\u0442\u0438\u0442\u0435\u0441\u044C \u0437\u0430 \u043F\u043E\u043C\u043E\u0449\u044C\u044E \u043A \u0432\u0438\u043A\u0438 Ruffle.
error-wasm-invalid =
    Ruffle \u0441\u0442\u043E\u043B\u043A\u043D\u0443\u043B\u0441\u044F \u0441 \u0441\u0435\u0440\u044C\u0451\u0437\u043D\u043E\u0439 \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u043E\u0439 \u0432\u043E \u0432\u0440\u0435\u043C\u044F \u0438\u043D\u0438\u0446\u0438\u0430\u043B\u0438\u0437\u0430\u0446\u0438\u0438.
    \u041F\u043E\u0445\u043E\u0436\u0435, \u0447\u0442\u043E \u043D\u0430 \u044D\u0442\u043E\u0439 \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0435 \u043E\u0442\u0441\u0443\u0442\u0441\u0442\u0432\u0443\u044E\u0442 \u0444\u0430\u0439\u043B\u044B \u0434\u043B\u044F \u0437\u0430\u043F\u0443\u0441\u043A\u0430 Ruffle \u0438\u043B\u0438 \u043E\u043D\u0438 \u043D\u0435\u0434\u0435\u0439\u0441\u0442\u0432\u0438\u0442\u0435\u043B\u044C\u043D\u044B.
    \u0415\u0441\u043B\u0438 \u0432\u044B \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043E\u0431\u0440\u0430\u0442\u0438\u0442\u0435\u0441\u044C \u0437\u0430 \u043F\u043E\u043C\u043E\u0449\u044C\u044E \u043A \u0432\u0438\u043A\u0438 Ruffle.
error-wasm-download =
    Ruffle \u0441\u0442\u043E\u043B\u043A\u043D\u0443\u043B\u0441\u044F \u0441 \u0441\u0435\u0440\u044C\u0451\u0437\u043D\u043E\u0439 \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u043E\u0439 \u0432\u043E \u0432\u0440\u0435\u043C\u044F \u0438\u043D\u0438\u0446\u0438\u0430\u043B\u0438\u0437\u0430\u0446\u0438\u0438.
    \u0427\u0430\u0449\u0435 \u0432\u0441\u0435\u0433\u043E \u044D\u0442\u0430 \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u0430 \u0443\u0441\u0442\u0440\u0430\u043D\u044F\u0435\u0442\u0441\u044F \u0441\u0430\u043C\u0430 \u0441\u043E\u0431\u043E\u044E, \u043F\u043E\u044D\u0442\u043E\u043C\u0443 \u0432\u044B \u043C\u043E\u0436\u0435\u0442\u0435 \u043F\u0440\u043E\u0441\u0442\u043E \u043F\u0435\u0440\u0435\u0437\u0430\u0433\u0440\u0443\u0437\u0438\u0442\u044C \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0443.
    \u0415\u0441\u043B\u0438 \u043E\u0448\u0438\u0431\u043A\u0430 \u043F\u0440\u043E\u0434\u043E\u043B\u0436\u0430\u0435\u0442 \u043F\u043E\u044F\u0432\u043B\u044F\u0442\u044C\u0441\u044F, \u0441\u0432\u044F\u0436\u0438\u0442\u0435\u0441\u044C \u0441 \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u043E\u043C \u0441\u0430\u0439\u0442\u0430.
error-wasm-disabled-on-edge =
    Ruffle \u043D\u0435 \u0443\u0434\u0430\u043B\u043E\u0441\u044C \u0437\u0430\u0433\u0440\u0443\u0437\u0438\u0442\u044C \u043D\u0435\u043E\u0431\u0445\u043E\u0434\u0438\u043C\u044B\u0439 \u043A\u043E\u043C\u043F\u043E\u043D\u0435\u043D\u0442 \u0444\u0430\u0439\u043B\u0430 ".wasm".
    \u0427\u0442\u043E\u0431\u044B \u0438\u0441\u043F\u0440\u0430\u0432\u0438\u0442\u044C \u044D\u0442\u0443 \u043E\u0448\u0438\u0431\u043A\u0443, \u043F\u043E\u043F\u0440\u043E\u0431\u0443\u0439\u0442\u0435 \u043E\u0442\u043A\u043B\u044E\u0447\u0438\u0442\u044C \u0432 \u043D\u0430\u0441\u0442\u0440\u043E\u0439\u043A\u0430\u0445 \u0431\u0440\u0430\u0443\u0437\u0435\u0440\u0430 \u0434\u043E\u043F\u043E\u043B\u043D\u0438\u0442\u0435\u043B\u044C\u043D\u0443\u044E \u043A\u043E\u043D\u0444\u0438\u0434\u0435\u043D\u0446\u0438\u0430\u043B\u044C\u043D\u043E\u0441\u0442\u044C. \u042D\u0442\u043E \u043F\u043E\u0437\u0432\u043E\u043B\u0438\u0442 \u0431\u0440\u0430\u0443\u0437\u0435\u0440\u0443 \u0437\u0430\u0433\u0440\u0443\u0437\u0438\u0442\u044C \u043D\u0435\u043E\u0431\u0445\u043E\u0434\u0438\u043C\u044B\u0435 WASM-\u0444\u0430\u0439\u043B\u044B.
    \u0415\u0441\u043B\u0438 \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u0430 \u043E\u0441\u0442\u0430\u043B\u0430\u0441\u044C, \u0432\u0430\u043C \u043C\u043E\u0436\u0435\u0442 \u043F\u043E\u0442\u0440\u0435\u0431\u043E\u0432\u0430\u0442\u044C\u0441\u044F \u0434\u0440\u0443\u0433\u043E\u0439 \u0431\u0440\u0430\u0443\u0437\u0435\u0440.
error-wasm-unsupported-browser =
    \u0412\u0430\u0448 \u0431\u0440\u0430\u0443\u0437\u0435\u0440 \u043D\u0435 \u043F\u043E\u0434\u0434\u0435\u0440\u0436\u0438\u0432\u0430\u0435\u0442 \u0440\u0430\u0441\u0448\u0438\u0440\u0435\u043D\u0438\u044F WebAssembly, \u043D\u0435\u043E\u0431\u0445\u043E\u0434\u0438\u043C\u044B\u0435 \u0434\u043B\u044F \u0437\u0430\u043F\u0443\u0441\u043A\u0430 Ruffle.
    \u041F\u043E\u0436\u0430\u043B\u0443\u0439\u0441\u0442\u0430, \u043F\u0435\u0440\u0435\u043A\u043B\u044E\u0447\u0438\u0442\u0435\u0441\u044C \u043D\u0430 \u043F\u043E\u0434\u0434\u0435\u0440\u0436\u0438\u0432\u0430\u0435\u043C\u044B\u0439 \u0431\u0440\u0430\u0443\u0437\u0435\u0440.
    \u0421\u043F\u0438\u0441\u043E\u043A \u043F\u043E\u0434\u0434\u0435\u0440\u0436\u0438\u0432\u0430\u0435\u043C\u044B\u0445 \u0431\u0440\u0430\u0443\u0437\u0435\u0440\u043E\u0432 \u0432\u044B \u043C\u043E\u0436\u0435\u0442\u0435 \u043D\u0430\u0439\u0442\u0438 \u0432 \u0412\u0438\u043A\u0438.
error-javascript-conflict =
    Ruffle \u0441\u0442\u043E\u043B\u043A\u043D\u0443\u043B\u0441\u044F \u0441 \u0441\u0435\u0440\u044C\u0451\u0437\u043D\u043E\u0439 \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u043E\u0439 \u0432\u043E \u0432\u0440\u0435\u043C\u044F \u0438\u043D\u0438\u0446\u0438\u0430\u043B\u0438\u0437\u0430\u0446\u0438\u0438.
    \u041F\u043E\u0445\u043E\u0436\u0435, \u0447\u0442\u043E \u044D\u0442\u0430 \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0430 \u0438\u0441\u043F\u043E\u043B\u044C\u0437\u0443\u0435\u0442 \u043A\u043E\u043D\u0444\u043B\u0438\u043A\u0442\u0443\u044E\u0449\u0438\u0439 \u0441 Ruffle \u043A\u043E\u0434 JavaScript.
    \u0415\u0441\u043B\u0438 \u0432\u044B \u044F\u0432\u043B\u044F\u0435\u0442\u0435\u0441\u044C \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u043E\u043C \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043C\u044B \u043F\u0440\u0435\u0434\u043B\u0430\u0433\u0430\u0435\u043C \u0432\u0430\u043C \u043F\u043E\u043F\u0440\u043E\u0431\u043E\u0432\u0430\u0442\u044C \u0437\u0430\u043F\u0443\u0441\u0442\u0438\u0442\u044C \u0444\u0430\u0439\u043B \u043D\u0430 \u043F\u0443\u0441\u0442\u043E\u0439 \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0435.
error-javascript-conflict-outdated = \u0412\u044B \u0442\u0430\u043A\u0436\u0435 \u043C\u043E\u0436\u0435\u0442\u0435 \u043F\u043E\u043F\u0440\u043E\u0431\u043E\u0432\u0430\u0442\u044C \u0437\u0430\u0433\u0440\u0443\u0437\u0438\u0442\u044C \u043F\u043E\u0441\u043B\u0435\u0434\u043D\u044E\u044E \u0432\u0435\u0440\u0441\u0438\u044E Ruffle, \u043A\u043E\u0442\u043E\u0440\u0430\u044F \u043C\u043E\u0436\u0435\u0442 \u043E\u0431\u043E\u0439\u0442\u0438 \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u0443 (\u0442\u0435\u043A\u0443\u0449\u0430\u044F \u0432\u0435\u0440\u0441\u0438\u044F \u0443\u0441\u0442\u0430\u0440\u0435\u043B\u0430: { $buildDate }).
error-csp-conflict =
    Ruffle \u0441\u0442\u043E\u043B\u043A\u043D\u0443\u043B\u0441\u044F \u0441 \u0441\u0435\u0440\u044C\u0451\u0437\u043D\u043E\u0439 \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u043E\u0439 \u0432\u043E \u0432\u0440\u0435\u043C\u044F \u0438\u043D\u0438\u0446\u0438\u0430\u043B\u0438\u0437\u0430\u0446\u0438\u0438.
    \u041F\u043E\u043B\u0438\u0442\u0438\u043A\u0430 \u0431\u0435\u0437\u043E\u043F\u0430\u0441\u043D\u043E\u0441\u0442\u0438 \u0441\u043E\u0434\u0435\u0440\u0436\u0438\u043C\u043E\u0433\u043E \u044D\u0442\u043E\u0433\u043E \u0432\u0435\u0431-\u0441\u0435\u0440\u0432\u0435\u0440\u0430 \u043D\u0435 \u043F\u043E\u0437\u0432\u043E\u043B\u044F\u0435\u0442 \u0438\u0441\u043F\u043E\u043B\u044C\u0437\u043E\u0432\u0430\u0442\u044C \u0442\u0440\u0435\u0431\u0443\u0435\u043C\u044B\u0435 \u043A\u043E\u043C\u043F\u043E\u043D\u0435\u043D\u0442\u044B \u0434\u043B\u044F \u0437\u0430\u043F\u0443\u0441\u043A\u0430 ".wasm".
    \u0415\u0441\u043B\u0438 \u0432\u044B \u044F\u0432\u043B\u044F\u0435\u0442\u0435\u0441\u044C \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u043E\u043C \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043E\u0431\u0440\u0430\u0442\u0438\u0442\u0435\u0441\u044C \u0437\u0430 \u043F\u043E\u043C\u043E\u0449\u044C\u044E \u043A \u0432\u0438\u043A\u0438 Ruffle.
error-unknown =
    Ruffle \u0441\u0442\u043E\u043B\u043A\u043D\u0443\u043B\u0441\u044F \u0441 \u0441\u0435\u0440\u044C\u0451\u0437\u043D\u043E\u0439 \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u043E\u0439 \u043F\u0440\u0438 \u043F\u043E\u043F\u044B\u0442\u043A\u0435 \u043E\u0442\u043E\u0431\u0440\u0430\u0437\u0438\u0442\u044C \u044D\u0442\u043E\u0442 Flash-\u043A\u043E\u043D\u0442\u0435\u043D\u0442.
    { $outdated ->
        [true] \u0415\u0441\u043B\u0438 \u0432\u044B \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043F\u043E\u043F\u0440\u043E\u0431\u0443\u0439\u0442\u0435 \u0437\u0430\u0433\u0440\u0443\u0437\u0438\u0442\u044C \u0431\u043E\u043B\u0435\u0435 \u043D\u043E\u0432\u0443\u044E \u0432\u0435\u0440\u0441\u0438\u044E Ruffle (\u0442\u0435\u043A\u0443\u0449\u0430\u044F \u0432\u0435\u0440\u0441\u0438\u044F \u0443\u0441\u0442\u0430\u0440\u0435\u043B\u0430: { $buildDate }).
       *[false] \u042D\u0442\u043E\u0433\u043E \u043D\u0435 \u0434\u043E\u043B\u0436\u043D\u043E \u043F\u0440\u043E\u0438\u0441\u0445\u043E\u0434\u0438\u0442\u044C, \u043F\u043E\u044D\u0442\u043E\u043C\u0443 \u043C\u044B \u0431\u0443\u0434\u0435\u043C \u043E\u0447\u0435\u043D\u044C \u043F\u0440\u0438\u0437\u043D\u0430\u0442\u0435\u043B\u044C\u043D\u044B, \u0435\u0441\u043B\u0438 \u0432\u044B \u0441\u043E\u043E\u0431\u0449\u0438\u0442\u0435 \u043D\u0430\u043C \u043E\u0431 \u043E\u0448\u0438\u0431\u043A\u0435!
    }
`,"save-manager.ftl":`save-delete-prompt = \u0423\u0434\u0430\u043B\u0438\u0442\u044C \u044D\u0442\u043E\u0442 \u0444\u0430\u0439\u043B \u0441\u043E\u0445\u0440\u0430\u043D\u0435\u043D\u0438\u044F?
save-reload-prompt =
    \u0415\u0434\u0438\u043D\u0441\u0442\u0432\u0435\u043D\u043D\u044B\u0439 \u0441\u043F\u043E\u0441\u043E\u0431 { $action ->
        [delete] \u0443\u0434\u0430\u043B\u0438\u0442\u044C
       *[replace] \u0437\u0430\u043C\u0435\u043D\u0438\u0442\u044C
    } \u044D\u0442\u043E\u0442 \u0444\u0430\u0439\u043B \u0441\u043E\u0445\u0440\u0430\u043D\u0435\u043D\u0438\u044F \u0431\u0435\u0437 \u043F\u043E\u0442\u0435\u043D\u0446\u0438\u0430\u043B\u044C\u043D\u043E\u0433\u043E \u043A\u043E\u043D\u0444\u043B\u0438\u043A\u0442\u0430 \u2013 \u043F\u0435\u0440\u0435\u0437\u0430\u043F\u0443\u0441\u0442\u0438\u0442\u044C \u0437\u0430\u043F\u0443\u0449\u0435\u043D\u043D\u044B\u0439 \u043A\u043E\u043D\u0442\u0435\u043D\u0442. \u0412\u0441\u0451 \u0440\u0430\u0432\u043D\u043E \u043F\u0440\u043E\u0434\u043E\u043B\u0436\u0438\u0442\u044C?
save-download = \u0421\u043A\u0430\u0447\u0430\u0442\u044C
save-replace = \u0417\u0430\u043C\u0435\u043D\u0438\u0442\u044C
save-delete = \u0423\u0434\u0430\u043B\u0438\u0442\u044C
save-backup-all = \u0421\u043A\u0430\u0447\u0430\u0442\u044C \u0432\u0441\u0435 \u0441\u043E\u0445\u0440\u0430\u043D\u0435\u043D\u0438\u044F
`,"volume-controls.ftl":`volume-controls-mute = \u0411\u0435\u0437 \u0437\u0432\u0443\u043A\u0430
volume-controls-unmute = \u0412\u043A\u043B\u044E\u0447\u0438\u0442\u044C \u0437\u0432\u0443\u043A
`},"sk-SK":{"context_menu.ftl":`context-menu-download-swf = Stiahnu\u0165 SWF
context-menu-copy-debug-info = Skop\xEDrova\u0165 debug info
context-menu-open-save-manager = Otvori\u0165 spr\xE1vcu ulo\u017Een\xED
context-menu-about-ruffle =
    { $flavor ->
        [extension] O Ruffle roz\u0161\xEDren\xED ({ $version })
       *[other] O Ruffle ({ $version })
    }
context-menu-hide = Skry\u0165 menu
context-menu-exit-fullscreen = Ukon\u010Di\u0165 re\u017Eim celej obrazovky
context-menu-enter-fullscreen = Prejs\u0165 do re\u017Eimu celej obrazovky
context-menu-volume-controls = Ovl\xE1danie hlasitosti
`,"messages.ftl":`message-cant-embed =
    Ruffle nemohol spusti\u0165 Flash vlo\u017Een\xFD na tejto str\xE1nke.
    M\xF4\u017Eete sa pok\xFAsi\u0165 otvori\u0165 s\xFAbor na samostatnej karte, aby ste sa vyhli tomuto probl\xE9mu.
message-restored-from-bfcache =
    V\xE1\u0161 prehliada\u010D obnovil tento Flash obsah z predch\xE1dzaj\xFAcej rel\xE1cie.
    Ak chcete za\u010Da\u0165 znovu, op\xE4tovne na\u010D\xEDtajte str\xE1nku.
panic-title = Nie\u010Do sa pokazilo :(
more-info = Viac inform\xE1ci\xED
run-anyway = Spusti\u0165 aj tak
continue = Pokra\u010Dova\u0165
report-bug = Nahl\xE1si\u0165 chybu
update-ruffle = Aktualizova\u0165 Ruffle
ruffle-demo = Web Demo
ruffle-desktop = Desktopov\xE1 aplik\xE1cia
ruffle-wiki = Zobrazi\u0165 Ruffle Wiki
enable-hardware-acceleration = Zd\xE1 sa, \u017Ee hardv\xE9rov\xE1 akceler\xE1cia je vypnut\xE1. Aj ke\u010F Ruffle funguje spr\xE1vne, m\xF4\u017Ee by\u0165 neprimerane pomal\xFD. Ako povoli\u0165 hardv\xE9rov\xFA akceler\xE1ciu zist\xEDte na tomto odkaze:
enable-hardware-acceleration-link = \u010Cast\xE9 ot\xE1zky - Hardv\xE9rov\xE1 akceler\xE1cia Chrome
view-error-details = Zobrazi\u0165 podrobnosti o chybe
open-in-new-tab = Otvori\u0165 na novej karte
click-to-unmute = Kliknut\xEDm zapnete zvuk
clipboard-message-title = Kop\xEDrovanie a vkladanie v Ruffle
clipboard-message-description =
    { $variant ->
       *[unsupported] V\xE1\u0161 prehliada\u010D nepodporuje pln\xFD pr\xEDstup k schr\xE1nke,
        [access-denied] Pr\xEDstup k schr\xE1nke bol odmietnut\xFD,
    } ale namiesto toho m\xF4\u017Eete v\u017Edy pou\u017Ei\u0165 tieto skratky:
clipboard-message-copy = { " " } pre kop\xEDrovanie
clipboard-message-cut = { " " } pre vystrihovanie
clipboard-message-paste = { " " } pre vlo\u017Eenie
error-canvas-reload = Nie je mo\u017En\xE9 znova na\u010D\xEDta\u0165 pomocou vykres\u013Eova\u010Da pl\xE1tna, ke\u010F sa vykres\u013Eovanie pl\xE1tna u\u017E pou\u017E\xEDva.
error-file-protocol =
    Zd\xE1 sa, \u017Ee pou\u017E\xEDvate Ruffle na protokole "file:".
    To nie je mo\u017En\xE9, preto\u017Ee prehliada\u010De blokuj\xFA fungovanie mnoh\xFDch funkci\xED z bezpe\u010Dnostn\xFDch d\xF4vodov.
    Namiesto toho v\xE1m odpor\xFA\u010Dame nastavi\u0165 lok\xE1lny server alebo pou\u017Ei\u0165 web demo \u010Di desktopov\xFA aplik\xE1ciu.
error-javascript-config =
    Ruffle narazil na probl\xE9m v d\xF4sledku nespr\xE1vnej konfigur\xE1cie JavaScriptu.
    Ak ste spr\xE1vcom servera, odpor\xFA\u010Dame v\xE1m skontrolova\u0165 podrobnosti o chybe, aby ste zistili, ktor\xFD parameter je chybn\xFD.
    Pomoc m\xF4\u017Eete z\xEDska\u0165 aj na wiki Ruffle.
error-wasm-not-found =
    Ruffle sa nepodarilo na\u010D\xEDta\u0165 po\u017Eadovan\xFD komponent s\xFAboru \u201E.wasm\u201C.
    Ak ste spr\xE1vcom servera, skontrolujte, \u010Di bol s\xFAbor spr\xE1vne nahran\xFD.
    Ak probl\xE9m pretrv\xE1va, mo\u017Eno budete musie\u0165 pou\u017Ei\u0165 nastavenie \u201EpublicPath\u201C: pomoc n\xE1jdete na wiki Ruffle.
error-wasm-mime-type =
    Ruffle narazil na probl\xE9m pri pokuse o inicializ\xE1ciu.
    Tento webov\xFD server neposkytuje s\xFAbory \u201E.wasm\u201C so spr\xE1vnym typom MIME.
    Ak ste spr\xE1vcom servera, pomoc n\xE1jdete na Ruffle wiki.
error-invalid-swf =
    Ruffle nem\xF4\u017Ee spracova\u0165 po\u017Eadovan\xFD s\xFAbor.
    Najpravdepodobnej\u0161\xEDm d\xF4vodom je, \u017Ee po\u017Eadovan\xFD s\xFAbor nie je platn\xFDm s\xFAborom SWF.
error-swf-fetch =
    Ruffle sa nepodarilo na\u010D\xEDta\u0165 SWF s\xFAbor Flash.
    Najpravdepodobnej\u0161\xEDm d\xF4vodom je, \u017Ee s\xFAbor u\u017E neexistuje, tak\u017Ee Ruffle nem\xE1 \u010Do na\u010D\xEDta\u0165.
    Sk\xFAste po\u017Eiada\u0165 o pomoc spr\xE1vcu webovej lokality.
error-swf-cors =
    Ruffle sa nepodarilo na\u010D\xEDta\u0165 SWF s\xFAbor Flash.
    Pr\xEDstup k na\u010D\xEDtaniu bol pravdepodobne zablokovan\xFD politikou CORS.
    Ak ste spr\xE1vcom servera, pomoc n\xE1jdete na Ruffle wiki.
error-wasm-cors =
    Ruffle sa nepodarilo na\u010D\xEDta\u0165 po\u017Eadovan\xFD komponent s\xFAboru \u201E.wasm\u201C.
    Pr\xEDstup k na\u010D\xEDtaniu bol pravdepodobne zablokovan\xFD politikou CORS.
    Ak ste spr\xE1vcom servera, pomoc n\xE1jdete na Ruffle wiki.
error-wasm-invalid =
    Ruffle narazil na probl\xE9m pri pokuse o inicializ\xE1ciu.
    Zd\xE1 sa, \u017Ee na tejto str\xE1nke ch\xFDbaj\xFA alebo s\xFA neplatn\xE9 s\xFAbory na spustenie Ruffle.
    Ak ste spr\xE1vcom servera, pomoc n\xE1jdete na Ruffle wiki.
error-wasm-download =
    Ruffle narazil na probl\xE9m pri pokuse o inicializ\xE1ciu.
    Probl\xE9m sa m\xF4\u017Ee vyrie\u0161i\u0165 aj s\xE1m, tak\u017Ee m\xF4\u017Eete sk\xFAsi\u0165 str\xE1nku na\u010D\xEDta\u0165 znova.
    V opa\u010Dnom pr\xEDpade kontaktujte administr\xE1tora str\xE1nky.
error-wasm-disabled-on-edge =
    Ruffle sa nepodarilo na\u010D\xEDta\u0165 po\u017Eadovan\xFD komponent s\xFAboru \u201E.wasm\u201C.
    Ak chcete tento probl\xE9m vyrie\u0161i\u0165, sk\xFAste otvori\u0165 nastavenia prehliada\u010Da, kliknite na polo\u017Eku \u201EOchrana osobn\xFDch \xFAdajov, vyh\u013Ead\xE1vanie a slu\u017Eby\u201C, prejdite nadol a vypnite mo\u017Enos\u0165 \u201EZv\xFD\u0161te svoju bezpe\u010Dnos\u0165 na webe\u201C.
    V\xE1\u0161mu prehliada\u010Du to umo\u017En\xED na\u010D\xEDta\u0165 po\u017Eadovan\xE9 s\xFAbory \u201E.wasm\u201C.
    Ak probl\xE9m pretrv\xE1va, mo\u017Eno budete musie\u0165 pou\u017Ei\u0165 in\xFD prehliada\u010D.
error-wasm-unsupported-browser =
    Prehliada\u010D, ktor\xFD pou\u017E\xEDvate, nepodporuje roz\u0161\xEDrenie WebAssembly, ktor\xE9 Ruffle vy\u017Eaduje na spustenie.
    Prejdite na podporovan\xFD prehliada\u010D.
    Zoznam podporovan\xFDch prehliada\u010Dov n\xE1jdete na Wiki.
error-javascript-conflict =
    Ruffle narazil na probl\xE9m pri pokuse o inicializ\xE1ciu.
    Zd\xE1 sa, \u017Ee t\xE1to str\xE1nka pou\u017E\xEDva k\xF3d JavaScript, ktor\xFD je v konflikte s Ruffle.
    Ak ste spr\xE1vcom servera, odpor\xFA\u010Dame v\xE1m sk\xFAsi\u0165 na\u010D\xEDta\u0165 s\xFAbor na pr\xE1zdnu str\xE1nku.
error-javascript-conflict-outdated = M\xF4\u017Eete sa tie\u017E pok\xFAsi\u0165 nahra\u0165 nov\u0161iu verziu Ruffle, ktor\xE1 m\xF4\u017Ee dan\xFD probl\xE9m vyrie\u0161i\u0165 (aktu\xE1lny build je zastaran\xFD: { $buildDate }).
error-csp-conflict =
    Ruffle narazil na probl\xE9m pri pokuse o inicializ\xE1ciu.
    Z\xE1sady zabezpe\u010Denia obsahu tohto webov\xE9ho servera nepovo\u013Euj\xFA spustenie po\u017Eadovan\xE9ho komponentu \u201E.wasm\u201C.
    Ak ste spr\xE1vcom servera, pomoc n\xE1jdete na Ruffle wiki.
error-unknown =
    Ruffle narazil na probl\xE9m pri pokuse zobrazi\u0165 tento Flash obsah.
    { $outdated ->
         [true] Ak ste spr\xE1vcom servera, sk\xFAste nahra\u0165 nov\u0161iu verziu Ruffle (aktu\xE1lny build je zastaran\xFD: { $buildDate }).
        *[false] Toto by sa nemalo sta\u0165, tak\u017Ee by sme naozaj ocenili, keby ste mohli nahl\xE1si\u0165 chybu!
    }
`,"save-manager.ftl":`save-delete-prompt = Naozaj chcete odstr\xE1ni\u0165 tento s\xFAbor s ulo\u017Een\xFDmi poz\xEDciami?
save-reload-prompt =
    Jedin\xFD sp\xF4sob, ako { $action ->
         [delete] vymaza\u0165
        *[replace] nahradi\u0165
    } tento s\xFAbor s ulo\u017Een\xFDmi poz\xEDciami bez potenci\xE1lneho konfliktu je op\xE4tovn\xE9 na\u010D\xEDtanie tohto obsahu. Chcete napriek tomu pokra\u010Dova\u0165?
save-download = Stiahnu\u0165
save-replace = Nahradi\u0165
save-delete = Vymaza\u0165
save-backup-all = Stiahnu\u0165 v\u0161etky s\xFAbory s ulo\u017Een\xFDmi poz\xEDciami
`,"volume-controls.ftl":`volume-controls-mute = Stlmi\u0165
volume-controls-unmute = Zru\u0161i\u0165 stlmenie
`},"sl-SI":{"context_menu.ftl":`context-menu-download-swf = Prenesi SWF
context-menu-copy-debug-info = Kopiraj informacije o odpravljanju napak
context-menu-open-save-manager = Odpri upravitelja shranjevanja
context-menu-about-ruffle =
    { $flavor ->
        [extension] O raz\u0161iritvi Ruffle ({ $version })
       *[other] O Ruffle ({ $version })
    }
context-menu-hide = Skrij ta meni
context-menu-exit-fullscreen = Izhod iz celozaslonskega na\u010Dina
context-menu-enter-fullscreen = Vstopi v celozaslonski na\u010Din
context-menu-volume-controls = Nadzor glasnosti
`,"messages.ftl":`message-cant-embed =
    Ruffle ni mogel zagnati Flash vsebine, vgrajene v to stran.
    Lahko poskusite odpreti datoteko v lo\u010Denem zavihku, da se izognete tej te\u017Eavi.
message-restored-from-bfcache =
    Va\u0161 brskalnik je obnovil to Flash vsebino iz prej\u0161nje seje.
    Da bi za\u010Deli na novo, ponovno nalo\u017Eite stran.
panic-title = Nekaj je \u0161lo narobe :(
more-info = Ve\u010D informacij
run-anyway = Vseeno za\u017Eeni
continue = Nadaljuj
report-bug = Prijavi napako
update-ruffle = Posodobite Ruffle
ruffle-demo = Spletni demo
ruffle-desktop = Namizna aplikacija
ruffle-wiki = Oglejte si Ruffle Wiki
enable-hardware-acceleration = Zdi se, da je strojna pospe\u0161itev onemogo\u010Dena. Ruffle bo sicer deloval, vendar bo lahko zelo po\u010Dasen. Kako omogo\u010Diti strojno pospe\u0161itev, lahko izveste na spodnji povezavi:
enable-hardware-acceleration-link = Pogosta vpra\u0161anja \u2013 Pospe\u0161evanje strojne opreme v brskalniku Chrome
view-error-details = Poglej podrobnosti napake
open-in-new-tab = Odpri v novem zavihku
click-to-unmute = Kliknite za vklop zvoka
clipboard-message-title = Kopiranje in lepljenje v Ruffle
clipboard-message-description =
    { $variant ->
       *[unsupported] Va\u0161 brskalnik ne podpira polnega dostopa do odlo\u017Ei\u0161\u010Da,
        [access-denied] Dostop do odlo\u017Ei\u0161\u010Da je bil zavrnjen,
    } vendar lahko namesto tega vedno uporabite te bli\u017Enjice:
clipboard-message-copy = { " " } za kopiranje
clipboard-message-cut = { " " } za izrez
clipboard-message-paste = { " " } za lepljenje
error-canvas-reload = Ne morem ponovno nalo\u017Eiti z upodabljalnikom platna, \u010De je upodabljalnik platna \u017Ee v uporabi.
error-file-protocol =
    Zdi se, da uporabljate Ruffle na protokolu "file:".
    To ne deluje, ker brskalniki iz varnostnih razlogov blokirajo delovanje mnogih funkcij.
    Namesto tega vam priporo\u010Damo, da nastavite lokalni stre\u017Enik ali uporabite spletno demo ali namizno aplikacijo.
error-javascript-config =
    Ruffle je naletel na ve\u010Djo te\u017Eavo zaradi nepravilne konfiguracije JavaScript.
    \u010Ce ste skrbnik stre\u017Enika, vas prosimo, da preverite podrobnosti napake in ugotovite, kateri parameter je kriv.
    Za pomo\u010D lahko poi\u0161\u010Dete tudi wiki Ruffle.
error-wasm-not-found =
    Ruffle ni uspel nalo\u017Eiti potrebne datoteke ".wasm".
    \u010Ce ste skrbnik stre\u017Enika, preverite, ali je datoteka pravilno nalo\u017Eena.
    \u010Ce te\u017Eava \u0161e vedno obstaja, boste morda morali uporabiti nastavitev "publicPath": za pomo\u010D si oglejte wiki Ruffle.
error-wasm-mime-type =
    Ruffle je med poskusom inicializacije naletel na ve\u010Djo te\u017Eavo.
    Ta spletni stre\u017Enik ne servira datotek ".wasm" s pravilnim tipom MIME.
    \u010Ce ste skrbnik stre\u017Enika, poi\u0161\u010Dite pomo\u010D v Ruffle wiki.
error-invalid-swf =
    Ruffle ne more raz\u010Dleniti zahtevane datoteke.
    Najverjetnej\u0161i razlog je, da zahtevana datoteka ni veljavna datoteka SWF.
error-swf-fetch =
    Ruffle ni uspel nalo\u017Eiti datoteke Flash SWF.
    Najverjetnej\u0161i razlog je, da datoteka ne obstaja ve\u010D, zato Ruffle nima kaj nalo\u017Eiti.
    Za pomo\u010D se obrnite na skrbnika spletnega mesta.
error-swf-cors =
    Ruffle ni uspel nalo\u017Eiti datoteke Flash SWF.
    Dostop do prenosa je verjetno blokiran s politiko CORS.
    \u010Ce ste skrbnik stre\u017Enika, poi\u0161\u010Dite pomo\u010D v Ruffle wiki.
error-wasm-cors =
    Ruffle ni uspel nalo\u017Eiti potrebne datote\u010Dne komponente datoteke ".wasm\u201C.
    Dostop do prenosa je verjetno blokiran s politiko CORS.
    \u010Ce ste skrbnik stre\u017Enika, poi\u0161\u010Dite pomo\u010D v Ruffle wiki.
error-wasm-invalid =
    Ruffle je med poskusom inicializacije naletel na ve\u010Djo te\u017Eavo.
    Zdi se, da na tej strani manjkajo datoteke ali so datoteke za zagon Ruffle neveljavne.
    \u010Ce ste skrbnik stre\u017Enika, poi\u0161\u010Dite pomo\u010D v Ruffle wiki.
error-wasm-download =
    Ruffle je med poskusom inicializacije naletel na ve\u010Djo te\u017Eavo.
    Ta se pogosto re\u0161i sama, zato lahko poskusite ponovno nalo\u017Eiti stran.
    V nasprotnem primeru se obrnite na skrbnika spletnega mesta.
error-wasm-disabled-on-edge =
    Ruffle ni uspel nalo\u017Eiti potrebne datote\u010Dne komponente ".wasm".
    Da bi to popravili, odprite nastavitve brskalnika, kliknite "Zasebnost, iskanje in storitve", pomaknite se navzdol in izklopite "Izbolj\u0161ajte svojo varnost na spletu".
    Tako bo brskalnik lahko nalo\u017Eil potrebne datoteke ".wasm".
    \u010Ce te\u017Eava \u0161e vedno obstaja, boste morda morali uporabiti drug brskalnik.
error-wasm-unsupported-browser =
    Brskalnik, ki ga uporabljate, ne podpira raz\u0161iritev WebAssembly, ki jih Ruffle potrebuje za delovanje.
    Preklopite na podprt brskalnik.
    Seznam podprtih brskalnikov najdete na Wiki.
error-javascript-conflict =
    Ruffle je med poskusom inicializacije naletel na ve\u010Djo te\u017Eavo.
    Zdi se, da ta stran uporablja JavaScript kodo, ki je v nasprotju z Ruffle.
    \u010Ce ste skrbnik stre\u017Enika, vas prosimo, da poskusite nalo\u017Eiti datoteko na prazno stran.
error-javascript-conflict-outdated = Lahko poskusite nalo\u017Eiti novej\u0161o razli\u010Dico Ruffle, ki bo morda odpravila te\u017Eavo (trenutna razli\u010Dica je zastarela: { $buildDate }).
error-csp-conflict =
    Ruffle je med poskusom inicializacije naletel na ve\u010Djo te\u017Eavo.
    Varnostna politika vsebine tega spletnega stre\u017Enika ne dovoljuje izvajanja potrebne komponente ".wasm".
    \u010Ce ste skrbnik stre\u017Enika, poi\u0161\u010Dite pomo\u010D v Ruffle wiki.
error-url-invalid =
    Ruffle ni uspel nalo\u017Eiti datoteke Flash SWF.
    Najverjetnej\u0161i razlog je, da je bil Ruffleju posredovan neveljaven URL za datoteko SWF.
error-unknown =
    Ruffle je naletel na ve\u010Djo te\u017Eavo pri prikazovanju te vsebine Flash.
    { $outdated ->
        [true] \u010Ce ste skrbnik stre\u017Enika, poskusite nalo\u017Eiti novej\u0161o razli\u010Dico Ruffle (trenutna razli\u010Dica je zastarela: { $buildDate }).
       *[false] To se ne bi smelo zgoditi, zato bi bili zelo hvale\u017Eni, \u010De bi prijavili napako!
    }
`,"save-manager.ftl":`save-delete-prompt = Ali ste prepri\u010Dani, da \u017Eelite izbrisati to shranjeno datoteko?
save-reload-prompt =
    Edini na\u010Din, da { $action ->
        [delete] izbri\u0161ete
       *[replace] zamenjate
    } to shranjeno datoteko brez morebitnega konflikta, je, da ponovno nalo\u017Eite to vsebino. \u017Delite vseeno nadaljevati?
save-download = Prenesi
save-replace = Zamenjaj
save-delete = Izbri\u0161i
save-backup-all = Prenesi vse shranjene datoteke
`,"volume-controls.ftl":`volume-controls-mute = Uti\u0161aj
volume-controls-unmute = Vklopi zvok
`},"sr-CS":{"context_menu.ftl":`context-menu-download-swf = Preuzmite .swf datoteku
context-menu-copy-debug-info = Kopirajte informacije za otklanjanje gre\u0161aka
context-menu-open-save-manager = Otvori menad\u017Eer skladi\u0161ta
context-menu-about-ruffle =
    { $flavor ->
    [extension] O ekstenziji Ruffle ({ $version })
    *[other] O Ruffle ({ $version })
    }
context-menu-hide = Sakrij ovaj meni
context-menu-exit-fullscreen = Iza\u0111i iz re\u017Eima celog ekrana
context-menu-enter-fullscreen = Pre\u0111i na ceo ekran
context-menu-volume-controls = Kontrole ja\u010Dine zvuka
`,"messages.ftl":`message-cant-embed =
    Ruffle nije mogao da pokrene Fle\u0161 ugra\u0111en na ovoj stranici.
    Mo\u017Eete poku\u0161ati da otvorite datoteku u posebnoj kartici da biste izbegli ovaj problem.
message-restored-from-bfcache =
    Va\u0161 pregleda\u010D je vratio ovaj Fle\u0161 sadr\u017Eaj iz prethodne sesije.
    Molimo vas da ponovo u\u010Ditate stranicu za novi po\u010Detak.
panic-title = Ne\u0161to je po\u0161lo po zlu :(
more-info = Dodatne informacije
run-anyway = Ipak pokreni
continue = Nastavi
report-bug = Prijavi gre\u0161ku
update-ruffle = A\u017Eurirajte Ruffle
ruffle-demo = Veb demo
ruffle-desktop = Desktop aplikacija
ruffle-wiki = Pogledajte Ruffle Wiki
enable-hardware-acceleration = Izgleda da je hardversko ubrzanje onemogu\u0107eno. Iako Ruffle mo\u017Eda radi, mo\u017Ee biti veoma spor. Mo\u017Eete saznati kako da omogu\u0107ite hardversko ubrzanje prate\u0107i donju vezu:
enable-hardware-acceleration-link = \u010Cesta pitanja - Hardversko ubrzanje u Chrome-u
view-error-details = Prika\u017Ei detalje gre\u0161ke
open-in-new-tab = Otvori u novoj kartici
click-to-unmute = Kliknite da biste uklju\u010Dili zvuk
clipboard-message-title = Kopiranje i nalepljivanje u Ruffle-u
clipboard-message-description =
    { $variant ->
    *[unsupported] Va\u0161 pregleda\u010D ne podr\u017Eava potpun pristup me\u0111uspremniku,
    [access-denied] Pristup baferu je zabranjen,
    } ali uvek mo\u017Eete koristiti ove pre\u010Dice:
clipboard-message-copy = { " " } za kopiju
clipboard-message-cut = { " " } za se\u010Denje
clipboard-message-paste = { " " } za lepljenje
error-canvas-reload = Ne mo\u017Ee se ponovo u\u010Ditati renderer za platno kada je renderer za platno ve\u0107 u upotrebi.
error-file-protocol =
    Izgleda da koristite Ruffle na protokolu "file:".
    Ovo ne funkcioni\u0161e jer pregleda\u010Di blokiraju mnoge funkcije iz bezbednosnih razloga.
    Umesto toga, preporu\u010Dujemo pode\u0161avanje lokalnog servera ili kori\u0161\u0107enje veb demo verzije ili desktop aplikacije.
error-javascript-config =
    Ruffle je nai\u0161ao na ozbiljan problem zbog pogre\u0161ne konfiguracije JavaSkripta.
    Ako ste administrator servera, preporu\u010Dujemo vam da proverite detalje gre\u0161ke kako biste saznali koji parametar uzrokuje problem. Tako\u0111e mo\u017Eete da konsultujete Ruffleov viki za pomo\u0107.
error-wasm-not-found =
    Ruffle nije uspeo da u\u010Dita potrebnu komponentu datoteke ".wasm".
    Ako ste administrator servera, proverite da li je datoteka ispravno otpremljena.
    Ako problem i dalje postoji, mo\u017Eda \u0107ete morati da koristite pode\u0161avanje "publicPath": pogledajte Ruffleovu viki stranicu za pomo\u0107.
error-wasm-mime-type =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja inicijalizacije.
    Ovaj veb server ne slu\u017Ei ".wasm" datoteke sa ispravnim MIME tipom.
    Ako ste administrator servera, obratite se Ruffleovom vikiju za pomo\u0107.
error-invalid-swf =
    Ruffle ne mo\u017Ee da analizira tra\u017Eenu datoteku.
    Najverovatniji razlog je taj \u0161to tra\u017Eena datoteka nije va\u017Ee\u0107i SWF.
error-swf-fetch =
    Ruffle nije uspeo da u\u010Dita Fle\u0161 SWF datoteku.
    Najverovatniji razlog je taj \u0161to datoteka vi\u0161e ne postoji, pa Ruffle nema \u0161ta da u\u010Dita.
    Poku\u0161ajte da kontaktirate administratora veb stranice za pomo\u0107.
error-swf-cors =
    Ruffle nije uspeo da u\u010Dita Fle\u0161 SWF datoteku.
    Pristup preuzimanju je verovatno blokiran CORS politikom.
    Ako ste administrator servera, pogledajte Ruffleovu viki stranicu za pomo\u0107.
error-wasm-cors =
    Ruffle nije uspeo da u\u010Dita potrebnu komponentu datoteke ".wasm".
    Pristup preuzimanju je verovatno blokiran CORS politikom.
    Ako ste administrator servera, pogledajte Ruffleovu viki stranicu za pomo\u0107.
error-wasm-invalid =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja inicijalizacije.
    Izgleda da ovoj stranici nedostaju ili su neva\u017Ee\u0107e datoteke za pokretanje Rufflea.
    Ako ste administrator servera, pogledajte Ruffleov viki za pomo\u0107.
error-wasm-download =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja inicijalizacije.
    Ovo se \u010Desto mo\u017Ee re\u0161iti jednostavnim ponovnim u\u010Ditavanjem stranice.
    U suprotnom, kontaktirajte administratora sajta.
error-wasm-disabled-on-edge =
    Ruffle nije uspeo da u\u010Dita potrebnu komponentnu datoteku ".wasm".
    Da biste re\u0161ili ovaj problem, poku\u0161ajte da otvorite pode\u0161avanja pregleda\u010Da, kliknete na "Privatnost, pretraga i usluge", pomerite se nadole i isklju\u010Dite "Pobolj\u0161aj bezbednost veba".
    Ovo \u0107e omogu\u0107iti va\u0161em pregleda\u010Du da u\u010Dita potrebne ".wasm" datoteke.
    Ako problem i dalje postoji, mo\u017Eda \u0107ete morati da koristite drugi pregleda\u010D.
error-wasm-unsupported-browser =
    Pregleda\u010D koji koristite ne podr\u017Eava WebAssembly ekstenzije potrebne za rad Ruffle-a.
    Molimo vas da pre\u0111ete na podr\u017Eani pregleda\u010D.
    Lista podr\u017Eanih pregleda\u010Da mo\u017Ee se na\u0107i na Viki stranici.
error-javascript-conflict =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja inicijalizacije.
    Izgleda da ova stranica koristi JavaSkript kod koji je u sukobu sa Ruffleom.
    Ako ste administrator servera, pozivamo vas da poku\u0161ate da otpremite datoteku na praznu stranicu.
error-javascript-conflict-outdated = Tako\u0111e mo\u017Eete poku\u0161ati da otpremite noviju verziju programa Ruffle koja bi mogla da re\u0161i problem (trenutna verzija je zastarela: { $buildDate }).
error-csp-conflict =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja inicijalizacije.
    Politike bezbednosti sadr\u017Eaja ovog veb servera ne dozvoljavaju pokretanje potrebne komponente ".wasm".
    Ako ste administrator servera, obratite se Ruffleovom vikiju za pomo\u0107.
error-unknown =
    Ruffle je nai\u0161ao na ozbiljan problem prilikom poku\u0161aja prikazivanja ovog Fle\u0161 sadr\u017Eaja.
    { $outdated ->
    [true] Ako ste administrator servera, poku\u0161ajte da otpremite noviju verziju Rufflea (trenutna verzija je zastarela: { $buildDate }).
    *[false] Ovo ne bi trebalo da se de\u0161ava, pa bismo vam bili veoma zahvalni ako biste prijavili gre\u0161ku!
    }
`,"save-manager.ftl":`save-delete-prompt = Da li ste sigurni da \u017Eelite da obri\u0161ete ovu datoteku za \u010Duvanje?
save-reload-prompt =
    Jedini na\u010Din da { $action ->
        [delete] obri\u0161ete
       *[replace] zamenite
    } ovu sa\u010Duvanu datoteku bez mogu\u0107ih konflikata jeste da ponovo u\u010Ditate ovaj sadr\u017Eaj. Da li \u017Eelite da ipak nastavite?
save-download = Preuzmite
save-replace = Zameni
save-delete = Obri\u0161i
save-backup-all = Preuzmi sve sa\u010Duvane datoteke
`,"volume-controls.ftl":`volume-controls-mute = Isklju\u010Di zvuk
volume-controls-unmute = Uklju\u010Di zvuk
`},"sr-SP":{"context_menu.ftl":`context-menu-download-swf = \u041F\u0440\u0435\u0443\u0437\u043C\u0438\u0442\u0435 .swf \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0443
context-menu-copy-debug-info = \u041A\u043E\u043F\u0438\u0440\u0430\u0458\u0442\u0435 \u0438\u043D\u0444\u043E\u0440\u043C\u0430\u0446\u0438\u0458\u0435 \u0437\u0430 \u043E\u0442\u043A\u043B\u0430\u045A\u0430\u045A\u0435 \u0433\u0440\u0435\u0448\u0430\u043A\u0430
context-menu-open-save-manager = \u041E\u0442\u0432\u043E\u0440\u0438 \u043C\u0435\u043D\u0430\u045F\u0435\u0440 \u0441\u043A\u043B\u0430\u0434\u0438\u0448\u0442\u0430
context-menu-about-ruffle =
    { $flavor ->
    [extension] \u041E \u0435\u043A\u0441\u0442\u0435\u043D\u0437\u0438\u0458\u0438 Ruffle ({ $version })
    *[other] \u041E Ruffle ({ $version })
    }
context-menu-hide = \u0421\u0430\u043A\u0440\u0438\u0458 \u043E\u0432\u0430\u0458 \u043C\u0435\u043D\u0438
context-menu-exit-fullscreen = \u0418\u0437\u0430\u0452\u0438 \u0438\u0437 \u0440\u0435\u0436\u0438\u043C\u0430 \u0446\u0435\u043B\u043E\u0433 \u0435\u043A\u0440\u0430\u043D\u0430
context-menu-enter-fullscreen = \u041F\u0440\u0435\u0452\u0438 \u043D\u0430 \u0446\u0435\u043E \u0435\u043A\u0440\u0430\u043D
context-menu-volume-controls = \u041A\u043E\u043D\u0442\u0440\u043E\u043B\u0435 \u0458\u0430\u0447\u0438\u043D\u0435 \u0437\u0432\u0443\u043A\u0430
`,"messages.ftl":`message-cant-embed =
    Ruffle \u043D\u0438\u0458\u0435 \u043C\u043E\u0433\u0430\u043E \u0434\u0430 \u043F\u043E\u043A\u0440\u0435\u043D\u0435 \u0424\u043B\u0435\u0448 \u0443\u0433\u0440\u0430\u0452\u0435\u043D \u043D\u0430 \u043E\u0432\u043E\u0458 \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0438.
    \u041C\u043E\u0436\u0435\u0442\u0435 \u043F\u043E\u043A\u0443\u0448\u0430\u0442\u0438 \u0434\u0430 \u043E\u0442\u0432\u043E\u0440\u0438\u0442\u0435 \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0443 \u0443 \u043F\u043E\u0441\u0435\u0431\u043D\u043E\u0458 \u043A\u0430\u0440\u0442\u0438\u0446\u0438 \u0434\u0430 \u0431\u0438\u0441\u0442\u0435 \u0438\u0437\u0431\u0435\u0433\u043B\u0438 \u043E\u0432\u0430\u0458 \u043F\u0440\u043E\u0431\u043B\u0435\u043C.
message-restored-from-bfcache =
    \u0412\u0430\u0448 \u043F\u0440\u0435\u0433\u043B\u0435\u0434\u0430\u0447 \u0458\u0435 \u0432\u0440\u0430\u0442\u0438\u043E \u043E\u0432\u0430\u0458 \u0424\u043B\u0435\u0448 \u0441\u0430\u0434\u0440\u0436\u0430\u0458 \u0438\u0437 \u043F\u0440\u0435\u0442\u0445\u043E\u0434\u043D\u0435 \u0441\u0435\u0441\u0438\u0458\u0435.
    \u041C\u043E\u043B\u0438\u043C\u043E \u0432\u0430\u0441 \u0434\u0430 \u043F\u043E\u043D\u043E\u0432\u043E \u0443\u0447\u0438\u0442\u0430\u0442\u0435 \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0443 \u0437\u0430 \u043D\u043E\u0432\u0438 \u043F\u043E\u0447\u0435\u0442\u0430\u043A.
panic-title = \u041D\u0435\u0448\u0442\u043E \u0458\u0435 \u043F\u043E\u0448\u043B\u043E \u043F\u043E \u0437\u043B\u0443 :(
more-info = \u0414\u043E\u0434\u0430\u0442\u043D\u0435 \u0438\u043D\u0444\u043E\u0440\u043C\u0430\u0446\u0438\u0458\u0435
run-anyway = \u0418\u043F\u0430\u043A \u043F\u043E\u043A\u0440\u0435\u043D\u0438
continue = \u041D\u0430\u0441\u0442\u0430\u0432\u0438
report-bug = \u041F\u0440\u0438\u0458\u0430\u0432\u0438 \u0433\u0440\u0435\u0448\u043A\u0443
update-ruffle = \u0410\u0436\u0443\u0440\u0438\u0440\u0430\u0458\u0442\u0435 Ruffle
ruffle-demo = \u0412\u0435\u0431 \u0434\u0435\u043C\u043E
ruffle-desktop = \u0414\u0435\u0441\u043A\u0442\u043E\u043F \u0430\u043F\u043B\u0438\u043A\u0430\u0446\u0438\u0458\u0430
ruffle-wiki = \u041F\u043E\u0433\u043B\u0435\u0434\u0430\u0458\u0442\u0435 Ruffle Wiki
enable-hardware-acceleration = \u0418\u0437\u0433\u043B\u0435\u0434\u0430 \u0434\u0430 \u0458\u0435 \u0445\u0430\u0440\u0434\u0432\u0435\u0440\u0441\u043A\u043E \u0443\u0431\u0440\u0437\u0430\u045A\u0435 \u043E\u043D\u0435\u043C\u043E\u0433\u0443\u045B\u0435\u043D\u043E. \u0418\u0430\u043A\u043E Ruffle \u043C\u043E\u0436\u0434\u0430 \u0440\u0430\u0434\u0438, \u043C\u043E\u0436\u0435 \u0431\u0438\u0442\u0438 \u0432\u0435\u043E\u043C\u0430 \u0441\u043F\u043E\u0440. \u041C\u043E\u0436\u0435\u0442\u0435 \u0441\u0430\u0437\u043D\u0430\u0442\u0438 \u043A\u0430\u043A\u043E \u0434\u0430 \u043E\u043C\u043E\u0433\u0443\u045B\u0438\u0442\u0435 \u0445\u0430\u0440\u0434\u0432\u0435\u0440\u0441\u043A\u043E \u0443\u0431\u0440\u0437\u0430\u045A\u0435 \u043F\u0440\u0430\u0442\u0435\u045B\u0438 \u0434\u043E\u045A\u0443 \u0432\u0435\u0437\u0443:
enable-hardware-acceleration-link = \u0427\u0435\u0441\u0442\u0430 \u043F\u0438\u0442\u0430\u045A\u0430 - \u0425\u0430\u0440\u0434\u0432\u0435\u0440\u0441\u043A\u043E \u0443\u0431\u0440\u0437\u0430\u045A\u0435 \u0443 Chrome-\u0443
view-error-details = \u041F\u0440\u0438\u043A\u0430\u0436\u0438 \u0434\u0435\u0442\u0430\u0459\u0435 \u0433\u0440\u0435\u0448\u043A\u0435
open-in-new-tab = \u041E\u0442\u0432\u043E\u0440\u0438 \u0443 \u043D\u043E\u0432\u043E\u0458 \u043A\u0430\u0440\u0442\u0438\u0446\u0438
click-to-unmute = \u041A\u043B\u0438\u043A\u043D\u0438\u0442\u0435 \u0434\u0430 \u0431\u0438\u0441\u0442\u0435 \u0443\u043A\u0459\u0443\u0447\u0438\u043B\u0438 \u0437\u0432\u0443\u043A
clipboard-message-title = \u041A\u043E\u043F\u0438\u0440\u0430\u045A\u0435 \u0438 \u043D\u0430\u043B\u0435\u043F\u0459\u0438\u0432\u0430\u045A\u0435 \u0443 Ruffle-\u0443
clipboard-message-description =
    { $variant ->
    *[unsupported] \u0412\u0430\u0448 \u043F\u0440\u0435\u0433\u043B\u0435\u0434\u0430\u0447 \u043D\u0435 \u043F\u043E\u0434\u0440\u0436\u0430\u0432\u0430 \u043F\u043E\u0442\u043F\u0443\u043D \u043F\u0440\u0438\u0441\u0442\u0443\u043F \u043C\u0435\u0452\u0443\u0441\u043F\u0440\u0435\u043C\u043D\u0438\u043A\u0443,
    [access-denied] \u041F\u0440\u0438\u0441\u0442\u0443\u043F \u0431\u0430\u0444\u0435\u0440\u0443 \u0458\u0435 \u0437\u0430\u0431\u0440\u0430\u045A\u0435\u043D,
    } \u0430\u043B\u0438 \u0443\u0432\u0435\u043A \u043C\u043E\u0436\u0435\u0442\u0435 \u043A\u043E\u0440\u0438\u0441\u0442\u0438\u0442\u0438 \u043E\u0432\u0435 \u043F\u0440\u0435\u0447\u0438\u0446\u0435:
clipboard-message-copy = { " " } \u0437\u0430 \u043A\u043E\u043F\u0438\u0458\u0443
clipboard-message-cut = { " " } \u0437\u0430 \u0441\u0435\u0447\u0435\u045A\u0435
clipboard-message-paste = { " " } \u0437\u0430 \u043B\u0435\u043F\u0459\u0435\u045A\u0435
error-canvas-reload = \u041D\u0435 \u043C\u043E\u0436\u0435 \u0441\u0435 \u043F\u043E\u043D\u043E\u0432\u043E \u0443\u0447\u0438\u0442\u0430\u0442\u0438 \u0440\u0435\u043D\u0434\u0435\u0440\u0435\u0440 \u0437\u0430 \u043F\u043B\u0430\u0442\u043D\u043E \u043A\u0430\u0434\u0430 \u0458\u0435 \u0440\u0435\u043D\u0434\u0435\u0440\u0435\u0440 \u0437\u0430 \u043F\u043B\u0430\u0442\u043D\u043E \u0432\u0435\u045B \u0443 \u0443\u043F\u043E\u0442\u0440\u0435\u0431\u0438.
error-file-protocol =
    \u0418\u0437\u0433\u043B\u0435\u0434\u0430 \u0434\u0430 \u043A\u043E\u0440\u0438\u0441\u0442\u0438\u0442\u0435 Ruffle \u043D\u0430 \u043F\u0440\u043E\u0442\u043E\u043A\u043E\u043B\u0443 "file:".
    \u041E\u0432\u043E \u043D\u0435 \u0444\u0443\u043D\u043A\u0446\u0438\u043E\u043D\u0438\u0448\u0435 \u0458\u0435\u0440 \u043F\u0440\u0435\u0433\u043B\u0435\u0434\u0430\u0447\u0438 \u0431\u043B\u043E\u043A\u0438\u0440\u0430\u0458\u0443 \u043C\u043D\u043E\u0433\u0435 \u0444\u0443\u043D\u043A\u0446\u0438\u0458\u0435 \u0438\u0437 \u0431\u0435\u0437\u0431\u0435\u0434\u043D\u043E\u0441\u043D\u0438\u0445 \u0440\u0430\u0437\u043B\u043E\u0433\u0430.
    \u0423\u043C\u0435\u0441\u0442\u043E \u0442\u043E\u0433\u0430, \u043F\u0440\u0435\u043F\u043E\u0440\u0443\u0447\u0443\u0458\u0435\u043C\u043E \u043F\u043E\u0434\u0435\u0448\u0430\u0432\u0430\u045A\u0435 \u043B\u043E\u043A\u0430\u043B\u043D\u043E\u0433 \u0441\u0435\u0440\u0432\u0435\u0440\u0430 \u0438\u043B\u0438 \u043A\u043E\u0440\u0438\u0448\u045B\u0435\u045A\u0435 \u0432\u0435\u0431 \u0434\u0435\u043C\u043E \u0432\u0435\u0440\u0437\u0438\u0458\u0435 \u0438\u043B\u0438 \u0434\u0435\u0441\u043A\u0442\u043E\u043F \u0430\u043F\u043B\u0438\u043A\u0430\u0446\u0438\u0458\u0435.
error-javascript-config =
    Ruffle \u0458\u0435 \u043D\u0430\u0438\u0448\u0430\u043E \u043D\u0430 \u043E\u0437\u0431\u0438\u0459\u0430\u043D \u043F\u0440\u043E\u0431\u043B\u0435\u043C \u0437\u0431\u043E\u0433 \u043F\u043E\u0433\u0440\u0435\u0448\u043D\u0435 \u043A\u043E\u043D\u0444\u0438\u0433\u0443\u0440\u0430\u0446\u0438\u0458\u0435 \u0408\u0430\u0432\u0430\u0421\u043A\u0440\u0438\u043F\u0442\u0430.
    \u0410\u043A\u043E \u0441\u0442\u0435 \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043F\u0440\u0435\u043F\u043E\u0440\u0443\u0447\u0443\u0458\u0435\u043C\u043E \u0432\u0430\u043C \u0434\u0430 \u043F\u0440\u043E\u0432\u0435\u0440\u0438\u0442\u0435 \u0434\u0435\u0442\u0430\u0459\u0435 \u0433\u0440\u0435\u0448\u043A\u0435 \u043A\u0430\u043A\u043E \u0431\u0438\u0441\u0442\u0435 \u0441\u0430\u0437\u043D\u0430\u043B\u0438 \u043A\u043E\u0458\u0438 \u043F\u0430\u0440\u0430\u043C\u0435\u0442\u0430\u0440 \u0443\u0437\u0440\u043E\u043A\u0443\u0458\u0435 \u043F\u0440\u043E\u0431\u043B\u0435\u043C. \u0422\u0430\u043A\u043E\u0452\u0435 \u043C\u043E\u0436\u0435\u0442\u0435 \u0434\u0430 \u043A\u043E\u043D\u0441\u0443\u043B\u0442\u0443\u0458\u0435\u0442\u0435 Ruffle\u043E\u0432 \u0432\u0438\u043A\u0438 \u0437\u0430 \u043F\u043E\u043C\u043E\u045B.
error-wasm-not-found =
    Ruffle \u043D\u0438\u0458\u0435 \u0443\u0441\u043F\u0435\u043E \u0434\u0430 \u0443\u0447\u0438\u0442\u0430 \u043F\u043E\u0442\u0440\u0435\u0431\u043D\u0443 \u043A\u043E\u043C\u043F\u043E\u043D\u0435\u043D\u0442\u0443 \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0435 ".wasm".
    \u0410\u043A\u043E \u0441\u0442\u0435 \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043F\u0440\u043E\u0432\u0435\u0440\u0438\u0442\u0435 \u0434\u0430 \u043B\u0438 \u0458\u0435 \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0430 \u0438\u0441\u043F\u0440\u0430\u0432\u043D\u043E \u043E\u0442\u043F\u0440\u0435\u043C\u0459\u0435\u043D\u0430.
    \u0410\u043A\u043E \u043F\u0440\u043E\u0431\u043B\u0435\u043C \u0438 \u0434\u0430\u0459\u0435 \u043F\u043E\u0441\u0442\u043E\u0458\u0438, \u043C\u043E\u0436\u0434\u0430 \u045B\u0435\u0442\u0435 \u043C\u043E\u0440\u0430\u0442\u0438 \u0434\u0430 \u043A\u043E\u0440\u0438\u0441\u0442\u0438\u0442\u0435 \u043F\u043E\u0434\u0435\u0448\u0430\u0432\u0430\u045A\u0435 "publicPath": \u043F\u043E\u0433\u043B\u0435\u0434\u0430\u0458\u0442\u0435 Ruffle\u043E\u0432\u0443 \u0432\u0438\u043A\u0438 \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0443 \u0437\u0430 \u043F\u043E\u043C\u043E\u045B.
error-wasm-mime-type =
    Ruffle \u0458\u0435 \u043D\u0430\u0438\u0448\u0430\u043E \u043D\u0430 \u043E\u0437\u0431\u0438\u0459\u0430\u043D \u043F\u0440\u043E\u0431\u043B\u0435\u043C \u043F\u0440\u0438\u043B\u0438\u043A\u043E\u043C \u043F\u043E\u043A\u0443\u0448\u0430\u0458\u0430 \u0438\u043D\u0438\u0446\u0438\u0458\u0430\u043B\u0438\u0437\u0430\u0446\u0438\u0458\u0435.
    \u041E\u0432\u0430\u0458 \u0432\u0435\u0431 \u0441\u0435\u0440\u0432\u0435\u0440 \u043D\u0435 \u0441\u043B\u0443\u0436\u0438 ".wasm" \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0435 \u0441\u0430 \u0438\u0441\u043F\u0440\u0430\u0432\u043D\u0438\u043C MIME \u0442\u0438\u043F\u043E\u043C.
    \u0410\u043A\u043E \u0441\u0442\u0435 \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043E\u0431\u0440\u0430\u0442\u0438\u0442\u0435 \u0441\u0435 Ruffle\u043E\u0432\u043E\u043C \u0432\u0438\u043A\u0438\u0458\u0443 \u0437\u0430 \u043F\u043E\u043C\u043E\u045B.
error-invalid-swf =
    Ruffle \u043D\u0435 \u043C\u043E\u0436\u0435 \u0434\u0430 \u0430\u043D\u0430\u043B\u0438\u0437\u0438\u0440\u0430 \u0442\u0440\u0430\u0436\u0435\u043D\u0443 \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0443.
    \u041D\u0430\u0458\u0432\u0435\u0440\u043E\u0432\u0430\u0442\u043D\u0438\u0458\u0438 \u0440\u0430\u0437\u043B\u043E\u0433 \u0458\u0435 \u0442\u0430\u0458 \u0448\u0442\u043E \u0442\u0440\u0430\u0436\u0435\u043D\u0430 \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0430 \u043D\u0438\u0458\u0435 \u0432\u0430\u0436\u0435\u045B\u0438 SWF.
error-swf-fetch =
    Ruffle \u043D\u0438\u0458\u0435 \u0443\u0441\u043F\u0435\u043E \u0434\u0430 \u0443\u0447\u0438\u0442\u0430 \u0424\u043B\u0435\u0448 SWF \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0443.
    \u041D\u0430\u0458\u0432\u0435\u0440\u043E\u0432\u0430\u0442\u043D\u0438\u0458\u0438 \u0440\u0430\u0437\u043B\u043E\u0433 \u0458\u0435 \u0442\u0430\u0458 \u0448\u0442\u043E \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0430 \u0432\u0438\u0448\u0435 \u043D\u0435 \u043F\u043E\u0441\u0442\u043E\u0458\u0438, \u043F\u0430 Ruffle \u043D\u0435\u043C\u0430 \u0448\u0442\u0430 \u0434\u0430 \u0443\u0447\u0438\u0442\u0430.
    \u041F\u043E\u043A\u0443\u0448\u0430\u0458\u0442\u0435 \u0434\u0430 \u043A\u043E\u043D\u0442\u0430\u043A\u0442\u0438\u0440\u0430\u0442\u0435 \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u0430 \u0432\u0435\u0431 \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0435 \u0437\u0430 \u043F\u043E\u043C\u043E\u045B.
error-swf-cors =
    Ruffle \u043D\u0438\u0458\u0435 \u0443\u0441\u043F\u0435\u043E \u0434\u0430 \u0443\u0447\u0438\u0442\u0430 \u0424\u043B\u0435\u0448 SWF \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0443.
    \u041F\u0440\u0438\u0441\u0442\u0443\u043F \u043F\u0440\u0435\u0443\u0437\u0438\u043C\u0430\u045A\u0443 \u0458\u0435 \u0432\u0435\u0440\u043E\u0432\u0430\u0442\u043D\u043E \u0431\u043B\u043E\u043A\u0438\u0440\u0430\u043D CORS \u043F\u043E\u043B\u0438\u0442\u0438\u043A\u043E\u043C.
    \u0410\u043A\u043E \u0441\u0442\u0435 \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043F\u043E\u0433\u043B\u0435\u0434\u0430\u0458\u0442\u0435 Ruffle\u043E\u0432\u0443 \u0432\u0438\u043A\u0438 \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0443 \u0437\u0430 \u043F\u043E\u043C\u043E\u045B.
error-wasm-cors =
    Ruffle \u043D\u0438\u0458\u0435 \u0443\u0441\u043F\u0435\u043E \u0434\u0430 \u0443\u0447\u0438\u0442\u0430 \u043F\u043E\u0442\u0440\u0435\u0431\u043D\u0443 \u043A\u043E\u043C\u043F\u043E\u043D\u0435\u043D\u0442\u0443 \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0435 ".wasm".
    \u041F\u0440\u0438\u0441\u0442\u0443\u043F \u043F\u0440\u0435\u0443\u0437\u0438\u043C\u0430\u045A\u0443 \u0458\u0435 \u0432\u0435\u0440\u043E\u0432\u0430\u0442\u043D\u043E \u0431\u043B\u043E\u043A\u0438\u0440\u0430\u043D CORS \u043F\u043E\u043B\u0438\u0442\u0438\u043A\u043E\u043C.
    \u0410\u043A\u043E \u0441\u0442\u0435 \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043F\u043E\u0433\u043B\u0435\u0434\u0430\u0458\u0442\u0435 Ruffle\u043E\u0432\u0443 \u0432\u0438\u043A\u0438 \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0443 \u0437\u0430 \u043F\u043E\u043C\u043E\u045B.
error-wasm-invalid =
    Ruffle \u0458\u0435 \u043D\u0430\u0438\u0448\u0430\u043E \u043D\u0430 \u043E\u0437\u0431\u0438\u0459\u0430\u043D \u043F\u0440\u043E\u0431\u043B\u0435\u043C \u043F\u0440\u0438\u043B\u0438\u043A\u043E\u043C \u043F\u043E\u043A\u0443\u0448\u0430\u0458\u0430 \u0438\u043D\u0438\u0446\u0438\u0458\u0430\u043B\u0438\u0437\u0430\u0446\u0438\u0458\u0435.
    \u0418\u0437\u0433\u043B\u0435\u0434\u0430 \u0434\u0430 \u043E\u0432\u043E\u0458 \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0438 \u043D\u0435\u0434\u043E\u0441\u0442\u0430\u0458\u0443 \u0438\u043B\u0438 \u0441\u0443 \u043D\u0435\u0432\u0430\u0436\u0435\u045B\u0435 \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0435 \u0437\u0430 \u043F\u043E\u043A\u0440\u0435\u0442\u0430\u045A\u0435 Ruffle\u0430.
    \u0410\u043A\u043E \u0441\u0442\u0435 \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043F\u043E\u0433\u043B\u0435\u0434\u0430\u0458\u0442\u0435 Ruffle\u043E\u0432 \u0432\u0438\u043A\u0438 \u0437\u0430 \u043F\u043E\u043C\u043E\u045B.
error-wasm-download =
    Ruffle \u0458\u0435 \u043D\u0430\u0438\u0448\u0430\u043E \u043D\u0430 \u043E\u0437\u0431\u0438\u0459\u0430\u043D \u043F\u0440\u043E\u0431\u043B\u0435\u043C \u043F\u0440\u0438\u043B\u0438\u043A\u043E\u043C \u043F\u043E\u043A\u0443\u0448\u0430\u0458\u0430 \u0438\u043D\u0438\u0446\u0438\u0458\u0430\u043B\u0438\u0437\u0430\u0446\u0438\u0458\u0435.
    \u041E\u0432\u043E \u0441\u0435 \u0447\u0435\u0441\u0442\u043E \u043C\u043E\u0436\u0435 \u0440\u0435\u0448\u0438\u0442\u0438 \u0458\u0435\u0434\u043D\u043E\u0441\u0442\u0430\u0432\u043D\u0438\u043C \u043F\u043E\u043D\u043E\u0432\u043D\u0438\u043C \u0443\u0447\u0438\u0442\u0430\u0432\u0430\u045A\u0435\u043C \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0435.
    \u0423 \u0441\u0443\u043F\u0440\u043E\u0442\u043D\u043E\u043C, \u043A\u043E\u043D\u0442\u0430\u043A\u0442\u0438\u0440\u0430\u0458\u0442\u0435 \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u0430 \u0441\u0430\u0458\u0442\u0430.
error-wasm-disabled-on-edge =
    Ruffle \u043D\u0438\u0458\u0435 \u0443\u0441\u043F\u0435\u043E \u0434\u0430 \u0443\u0447\u0438\u0442\u0430 \u043F\u043E\u0442\u0440\u0435\u0431\u043D\u0443 \u043A\u043E\u043C\u043F\u043E\u043D\u0435\u043D\u0442\u043D\u0443 \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0443 ".wasm".
    \u0414\u0430 \u0431\u0438\u0441\u0442\u0435 \u0440\u0435\u0448\u0438\u043B\u0438 \u043E\u0432\u0430\u0458 \u043F\u0440\u043E\u0431\u043B\u0435\u043C, \u043F\u043E\u043A\u0443\u0448\u0430\u0458\u0442\u0435 \u0434\u0430 \u043E\u0442\u0432\u043E\u0440\u0438\u0442\u0435 \u043F\u043E\u0434\u0435\u0448\u0430\u0432\u0430\u045A\u0430 \u043F\u0440\u0435\u0433\u043B\u0435\u0434\u0430\u0447\u0430, \u043A\u043B\u0438\u043A\u043D\u0435\u0442\u0435 \u043D\u0430 "\u041F\u0440\u0438\u0432\u0430\u0442\u043D\u043E\u0441\u0442, \u043F\u0440\u0435\u0442\u0440\u0430\u0433\u0430 \u0438 \u0443\u0441\u043B\u0443\u0433\u0435", \u043F\u043E\u043C\u0435\u0440\u0438\u0442\u0435 \u0441\u0435 \u043D\u0430\u0434\u043E\u043B\u0435 \u0438 \u0438\u0441\u043A\u0459\u0443\u0447\u0438\u0442\u0435 "\u041F\u043E\u0431\u043E\u0459\u0448\u0430\u0458 \u0431\u0435\u0437\u0431\u0435\u0434\u043D\u043E\u0441\u0442 \u0432\u0435\u0431\u0430".
    \u041E\u0432\u043E \u045B\u0435 \u043E\u043C\u043E\u0433\u0443\u045B\u0438\u0442\u0438 \u0432\u0430\u0448\u0435\u043C \u043F\u0440\u0435\u0433\u043B\u0435\u0434\u0430\u0447\u0443 \u0434\u0430 \u0443\u0447\u0438\u0442\u0430 \u043F\u043E\u0442\u0440\u0435\u0431\u043D\u0435 ".wasm" \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0435.
    \u0410\u043A\u043E \u043F\u0440\u043E\u0431\u043B\u0435\u043C \u0438 \u0434\u0430\u0459\u0435 \u043F\u043E\u0441\u0442\u043E\u0458\u0438, \u043C\u043E\u0436\u0434\u0430 \u045B\u0435\u0442\u0435 \u043C\u043E\u0440\u0430\u0442\u0438 \u0434\u0430 \u043A\u043E\u0440\u0438\u0441\u0442\u0438\u0442\u0435 \u0434\u0440\u0443\u0433\u0438 \u043F\u0440\u0435\u0433\u043B\u0435\u0434\u0430\u0447.
error-wasm-unsupported-browser =
    \u041F\u0440\u0435\u0433\u043B\u0435\u0434\u0430\u0447 \u043A\u043E\u0458\u0438 \u043A\u043E\u0440\u0438\u0441\u0442\u0438\u0442\u0435 \u043D\u0435 \u043F\u043E\u0434\u0440\u0436\u0430\u0432\u0430 WebAssembly \u0435\u043A\u0441\u0442\u0435\u043D\u0437\u0438\u0458\u0435 \u043F\u043E\u0442\u0440\u0435\u0431\u043D\u0435 \u0437\u0430 \u0440\u0430\u0434 Ruffle-\u0430.
    \u041C\u043E\u043B\u0438\u043C\u043E \u0432\u0430\u0441 \u0434\u0430 \u043F\u0440\u0435\u0452\u0435\u0442\u0435 \u043D\u0430 \u043F\u043E\u0434\u0440\u0436\u0430\u043D\u0438 \u043F\u0440\u0435\u0433\u043B\u0435\u0434\u0430\u0447.
    \u041B\u0438\u0441\u0442\u0430 \u043F\u043E\u0434\u0440\u0436\u0430\u043D\u0438\u0445 \u043F\u0440\u0435\u0433\u043B\u0435\u0434\u0430\u0447\u0430 \u043C\u043E\u0436\u0435 \u0441\u0435 \u043D\u0430\u045B\u0438 \u043D\u0430 \u0412\u0438\u043A\u0438 \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0438.
error-javascript-conflict =
    Ruffle \u0458\u0435 \u043D\u0430\u0438\u0448\u0430\u043E \u043D\u0430 \u043E\u0437\u0431\u0438\u0459\u0430\u043D \u043F\u0440\u043E\u0431\u043B\u0435\u043C \u043F\u0440\u0438\u043B\u0438\u043A\u043E\u043C \u043F\u043E\u043A\u0443\u0448\u0430\u0458\u0430 \u0438\u043D\u0438\u0446\u0438\u0458\u0430\u043B\u0438\u0437\u0430\u0446\u0438\u0458\u0435.
    \u0418\u0437\u0433\u043B\u0435\u0434\u0430 \u0434\u0430 \u043E\u0432\u0430 \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0430 \u043A\u043E\u0440\u0438\u0441\u0442\u0438 \u0408\u0430\u0432\u0430\u0421\u043A\u0440\u0438\u043F\u0442 \u043A\u043E\u0434 \u043A\u043E\u0458\u0438 \u0458\u0435 \u0443 \u0441\u0443\u043A\u043E\u0431\u0443 \u0441\u0430 Ruffle\u043E\u043C.
    \u0410\u043A\u043E \u0441\u0442\u0435 \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043F\u043E\u0437\u0438\u0432\u0430\u043C\u043E \u0432\u0430\u0441 \u0434\u0430 \u043F\u043E\u043A\u0443\u0448\u0430\u0442\u0435 \u0434\u0430 \u043E\u0442\u043F\u0440\u0435\u043C\u0438\u0442\u0435 \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0443 \u043D\u0430 \u043F\u0440\u0430\u0437\u043D\u0443 \u0441\u0442\u0440\u0430\u043D\u0438\u0446\u0443.
error-javascript-conflict-outdated = \u0422\u0430\u043A\u043E\u0452\u0435 \u043C\u043E\u0436\u0435\u0442\u0435 \u043F\u043E\u043A\u0443\u0448\u0430\u0442\u0438 \u0434\u0430 \u043E\u0442\u043F\u0440\u0435\u043C\u0438\u0442\u0435 \u043D\u043E\u0432\u0438\u0458\u0443 \u0432\u0435\u0440\u0437\u0438\u0458\u0443 \u043F\u0440\u043E\u0433\u0440\u0430\u043C\u0430 Ruffle \u043A\u043E\u0458\u0430 \u0431\u0438 \u043C\u043E\u0433\u043B\u0430 \u0434\u0430 \u0440\u0435\u0448\u0438 \u043F\u0440\u043E\u0431\u043B\u0435\u043C (\u0442\u0440\u0435\u043D\u0443\u0442\u043D\u0430 \u0432\u0435\u0440\u0437\u0438\u0458\u0430 \u0458\u0435 \u0437\u0430\u0441\u0442\u0430\u0440\u0435\u043B\u0430: { $buildDate }).
error-csp-conflict =
    Ruffle \u0458\u0435 \u043D\u0430\u0438\u0448\u0430\u043E \u043D\u0430 \u043E\u0437\u0431\u0438\u0459\u0430\u043D \u043F\u0440\u043E\u0431\u043B\u0435\u043C \u043F\u0440\u0438\u043B\u0438\u043A\u043E\u043C \u043F\u043E\u043A\u0443\u0448\u0430\u0458\u0430 \u0438\u043D\u0438\u0446\u0438\u0458\u0430\u043B\u0438\u0437\u0430\u0446\u0438\u0458\u0435.
    \u041F\u043E\u043B\u0438\u0442\u0438\u043A\u0435 \u0431\u0435\u0437\u0431\u0435\u0434\u043D\u043E\u0441\u0442\u0438 \u0441\u0430\u0434\u0440\u0436\u0430\u0458\u0430 \u043E\u0432\u043E\u0433 \u0432\u0435\u0431 \u0441\u0435\u0440\u0432\u0435\u0440\u0430 \u043D\u0435 \u0434\u043E\u0437\u0432\u043E\u0459\u0430\u0432\u0430\u0458\u0443 \u043F\u043E\u043A\u0440\u0435\u0442\u0430\u045A\u0435 \u043F\u043E\u0442\u0440\u0435\u0431\u043D\u0435 \u043A\u043E\u043C\u043F\u043E\u043D\u0435\u043D\u0442\u0435 ".wasm".
    \u0410\u043A\u043E \u0441\u0442\u0435 \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043E\u0431\u0440\u0430\u0442\u0438\u0442\u0435 \u0441\u0435 Ruffle\u043E\u0432\u043E\u043C \u0432\u0438\u043A\u0438\u0458\u0443 \u0437\u0430 \u043F\u043E\u043C\u043E\u045B.
error-unknown =
    Ruffle \u0458\u0435 \u043D\u0430\u0438\u0448\u0430\u043E \u043D\u0430 \u043E\u0437\u0431\u0438\u0459\u0430\u043D \u043F\u0440\u043E\u0431\u043B\u0435\u043C \u043F\u0440\u0438\u043B\u0438\u043A\u043E\u043C \u043F\u043E\u043A\u0443\u0448\u0430\u0458\u0430 \u043F\u0440\u0438\u043A\u0430\u0437\u0438\u0432\u0430\u045A\u0430 \u043E\u0432\u043E\u0433 \u0424\u043B\u0435\u0448 \u0441\u0430\u0434\u0440\u0436\u0430\u0458\u0430.
    { $outdated ->
    [true] \u0410\u043A\u043E \u0441\u0442\u0435 \u0430\u0434\u043C\u0438\u043D\u0438\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043F\u043E\u043A\u0443\u0448\u0430\u0458\u0442\u0435 \u0434\u0430 \u043E\u0442\u043F\u0440\u0435\u043C\u0438\u0442\u0435 \u043D\u043E\u0432\u0438\u0458\u0443 \u0432\u0435\u0440\u0437\u0438\u0458\u0443 Ruffle\u0430 (\u0442\u0440\u0435\u043D\u0443\u0442\u043D\u0430 \u0432\u0435\u0440\u0437\u0438\u0458\u0430 \u0458\u0435 \u0437\u0430\u0441\u0442\u0430\u0440\u0435\u043B\u0430: { $buildDate }).
    *[false] \u041E\u0432\u043E \u043D\u0435 \u0431\u0438 \u0442\u0440\u0435\u0431\u0430\u043B\u043E \u0434\u0430 \u0441\u0435 \u0434\u0435\u0448\u0430\u0432\u0430, \u043F\u0430 \u0431\u0438\u0441\u043C\u043E \u0432\u0430\u043C \u0431\u0438\u043B\u0438 \u0432\u0435\u043E\u043C\u0430 \u0437\u0430\u0445\u0432\u0430\u043B\u043D\u0438 \u0430\u043A\u043E \u0431\u0438\u0441\u0442\u0435 \u043F\u0440\u0438\u0458\u0430\u0432\u0438\u043B\u0438 \u0433\u0440\u0435\u0448\u043A\u0443!
    }
`,"save-manager.ftl":`save-delete-prompt = \u0414\u0430 \u043B\u0438 \u0441\u0442\u0435 \u0441\u0438\u0433\u0443\u0440\u043D\u0438 \u0434\u0430 \u0436\u0435\u043B\u0438\u0442\u0435 \u0434\u0430 \u043E\u0431\u0440\u0438\u0448\u0435\u0442\u0435 \u043E\u0432\u0443 \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0443 \u0437\u0430 \u0447\u0443\u0432\u0430\u045A\u0435?
save-reload-prompt =
    \u0408\u0435\u0434\u0438\u043D\u0438 \u043D\u0430\u0447\u0438\u043D \u0434\u0430 { $action ->
        [delete] \u043E\u0431\u0440\u0438\u0448\u0435\u0442\u0435
       *[replace] \u0437\u0430\u043C\u0435\u043D\u0438\u0442\u0435
    } \u043E\u0432\u0443 \u0441\u0430\u0447\u0443\u0432\u0430\u043D\u0443 \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0443 \u0431\u0435\u0437 \u043C\u043E\u0433\u0443\u045B\u0438\u0445 \u043A\u043E\u043D\u0444\u043B\u0438\u043A\u0430\u0442\u0430 \u0458\u0435\u0441\u0442\u0435 \u0434\u0430 \u043F\u043E\u043D\u043E\u0432\u043E \u0443\u0447\u0438\u0442\u0430\u0442\u0435 \u043E\u0432\u0430\u0458 \u0441\u0430\u0434\u0440\u0436\u0430\u0458. \u0414\u0430 \u043B\u0438 \u0436\u0435\u043B\u0438\u0442\u0435 \u0434\u0430 \u0438\u043F\u0430\u043A \u043D\u0430\u0441\u0442\u0430\u0432\u0438\u0442\u0435?
save-download = \u041F\u0440\u0435\u0443\u0437\u043C\u0438\u0442\u0435
save-replace = \u0417\u0430\u043C\u0435\u043D\u0438
save-delete = \u041E\u0431\u0440\u0438\u0448\u0438
save-backup-all = \u041F\u0440\u0435\u0443\u0437\u043C\u0438 \u0441\u0432\u0435 \u0441\u0430\u0447\u0443\u0432\u0430\u043D\u0435 \u0434\u0430\u0442\u043E\u0442\u0435\u043A\u0435
`,"volume-controls.ftl":`volume-controls-mute = \u0418\u0441\u043A\u0459\u0443\u0447\u0438 \u0437\u0432\u0443\u043A
volume-controls-unmute = \u0423\u043A\u0459\u0443\u0447\u0438 \u0437\u0432\u0443\u043A
`},"sv-SE":{"context_menu.ftl":`context-menu-download-swf = Ladda ned SWF-fil
context-menu-copy-debug-info = Kopiera fels\xF6kningsinformation
context-menu-open-save-manager = \xD6ppna sparfilshanteraren
context-menu-about-ruffle =
    { $flavor ->
        [extension] Om Ruffle-till\xE4gget ({ $version })
       *[other] Om Ruffle ({ $version })
    }
context-menu-hide = D\xF6lj den h\xE4r menyn
context-menu-exit-fullscreen = Avsluta helsk\xE4rm
context-menu-enter-fullscreen = Helsk\xE4rm
context-menu-volume-controls = Ljudkontroller
`,"messages.ftl":`message-cant-embed =
    Ruffle kunde inte k\xF6ra Flash-inneh\xE5llet som \xE4r inb\xE4ddat p\xE5 den h\xE4r sidan.
    Du kan f\xF6rs\xF6ka kringg\xE5 problemet genom att \xF6ppna filen p\xE5 en separat flik.
message-restored-from-bfcache =
    Din webbl\xE4sare \xE5terst\xE4llde detta Flash-inneh\xE5ll fr\xE5n en tidigare session.
    F\xF6r att b\xF6rja p\xE5 nytt, ladda om sidan.
panic-title = N\xE5got gick fel :(
more-info = Mer information
run-anyway = K\xF6r \xE4nd\xE5
continue = Forts\xE4tt
report-bug = Rapportera fel
update-ruffle = Uppdatera Ruffle
ruffle-demo = Webbdemo
ruffle-desktop = Skrivbordsprogram
ruffle-wiki = Visa Ruffles wiki
enable-hardware-acceleration = H\xE5rdvaruaccelerationen verkar vara avst\xE4ngd. Ruffle kan fortfarande fungera, men det kan g\xE5 mycket l\xE5ngsamt. F\xF6lj l\xE4nken nedan f\xF6r information om hur du aktiverar h\xE5rdvaruacceleration:
enable-hardware-acceleration-link = FAQ \u2013 h\xE5rdvaruacceleration i Chrome
view-error-details = Visa felinformation
open-in-new-tab = \xD6ppna i en ny flik
click-to-unmute = Klicka f\xF6r att sl\xE5 p\xE5 ljudet
clipboard-message-title = Kopiera och klistra in i Ruffle
clipboard-message-description =
    { $variant ->
       *[unsupported] Din webbl\xE4sare har inte fullst\xE4ndig \xE5tkomst till urklippet,
        [access-denied] Urklipps\xE5tkomst har nekats,
    } men du kan anv\xE4nda dessa kortkommandon ist\xE4llet:
clipboard-message-copy = { " " } f\xF6r att kopiera
clipboard-message-cut = { " " } f\xF6r att klippa ut
clipboard-message-paste = { " " } f\xF6r att klistra in
error-canvas-reload = Kan inte ladda om med canvas-renderaren n\xE4r den redan anv\xE4nds.
error-file-protocol =
    Det verkar som att du k\xF6r Ruffle via protokollet \u201Dfile:\u201D.
    Det fungerar inte eftersom webbl\xE4sare av s\xE4kerhetssk\xE4l blockerar m\xE5nga n\xF6dv\xE4ndiga funktioner.
    Konfigurera i st\xE4llet en lokal server eller anv\xE4nd webbdemon eller skrivbordsprogrammet.
error-javascript-config =
    Ruffle har st\xF6tt p\xE5 ett allvarligt problem p\xE5 grund av en felaktig JavaScript-konfiguration.
    Om du \xE4r serveradministrat\xF6r kan du kontrollera felinformationen f\xF6r att se vilken parameter som orsakar felet.
    Du kan \xE4ven f\xE5 hj\xE4lp i Ruffles wiki.
error-wasm-not-found =
    Ruffle kunde inte l\xE4sa in den n\xF6dv\xE4ndiga \u201D.wasm\u201D-filen.
    Om du \xE4r serveradministrat\xF6r b\xF6r du kontrollera att filen har laddats upp korrekt.
    Om problemet kvarst\xE5r kan du beh\xF6va anv\xE4nda inst\xE4llningen \u201DpublicPath\u201D. Mer information finns i Ruffles wiki.
error-wasm-mime-type =
    Ruffle har st\xF6tt p\xE5 ett allvarligt problem under initieringen.
    Webbservern levererar inte \u201D.wasm\u201D-filer med r\xE4tt MIME-typ.
    Om du \xE4r serveradministrat\xF6r finns mer information i Ruffles wiki.
error-invalid-swf =
    Ruffle kan inte tolka den beg\xE4rda filen.
    Den troligaste orsaken \xE4r att filen inte \xE4r en giltig SWF-fil.
error-swf-fetch =
    Ruffle kunde inte l\xE4sa in Flash-SWF-filen.
    Den troligaste orsaken \xE4r att filen inte l\xE4ngre finns och d\xE4rf\xF6r inte kan l\xE4sas in.
    Kontakta webbplatsens administrat\xF6r f\xF6r hj\xE4lp.
error-swf-cors =
    Ruffle kunde inte l\xE4sa in Flash-SWF-filen.
    H\xE4mtningen har troligen blockerats av CORS-policyn.
    Om du \xE4r serveradministrat\xF6r finns mer information i Ruffles wiki.
error-wasm-cors =
    Ruffle kunde inte l\xE4sa in den n\xF6dv\xE4ndiga \u201D.wasm\u201D-filen.
    H\xE4mtningen har troligen blockerats av CORS-policyn.
    Om du \xE4r serveradministrat\xF6r finns mer information i Ruffles wiki.
error-wasm-invalid =
    Ruffle har st\xF6tt p\xE5 ett allvarligt problem under initieringen.
    Sidan verkar sakna giltiga filer som kr\xE4vs f\xF6r att k\xF6ra Ruffle.
    Om du \xE4r serveradministrat\xF6r finns mer information i Ruffles wiki.
error-wasm-download =
    Ruffle har st\xF6tt p\xE5 ett stort fel under initieringen.
    Detta kan ofta l\xF6sas av sig sj\xE4lv s\xE5 du kan prova att ladda om sidan.
    Kontakta annars v\xE4nligen webbplatsens administrat\xF6r.
error-wasm-disabled-on-edge =
    Ruffle kunde inte l\xE4sa in den n\xF6dv\xE4ndiga \u201D.wasm\u201D-filen.
    F\xF6rs\xF6k \xE5tg\xE4rda problemet genom att \xF6ppna webbl\xE4sarens inst\xE4llningar, klicka p\xE5 \u201DSekretess, s\xF6kning och tj\xE4nster\u201D, rulla ned och st\xE4nga av \u201DF\xF6rb\xE4ttra s\xE4kerheten p\xE5 webben\u201D.
    D\xE5 kan webbl\xE4saren l\xE4sa in de n\xF6dv\xE4ndiga \u201D.wasm\u201D-filerna.
    Om problemet kvarst\xE5r kan du beh\xF6va anv\xE4nda en annan webbl\xE4sare.
error-wasm-unsupported-browser =
    Webbl\xE4saren st\xF6der inte de WebAssembly-till\xE4gg som kr\xE4vs f\xF6r att k\xF6ra Ruffle.
    Byt till en webbl\xE4sare som st\xF6ds.
    En lista \xF6ver kompatibla webbl\xE4sare finns i wikin.
error-javascript-conflict =
    Ruffle har st\xF6tt p\xE5 ett allvarligt problem under initieringen.
    Sidan verkar anv\xE4nda JavaScript-kod som st\xE5r i konflikt med Ruffle.
    Om du \xE4r serveradministrat\xF6r kan du f\xF6rs\xF6ka l\xE4sa in filen p\xE5 en tom sida.
error-javascript-conflict-outdated = Du kan ocks\xE5 f\xF6rs\xF6ka ladda upp en nyare version av Ruffle, vilket kan kringg\xE5 problemet (nuvarande version \xE4r utdaterad: { $buildDate }).
error-csp-conflict =
    Ruffle har st\xF6tt p\xE5 ett allvarligt problem under initieringen.
    Webbserverns inneh\xE5llss\xE4kerhetspolicy till\xE5ter inte att den n\xF6dv\xE4ndiga \u201D.wasm\u201D-komponenten k\xF6rs.
    Om du \xE4r serveradministrat\xF6r finns mer information i Ruffles wiki.
error-url-invalid =
    Ruffle kunde inte l\xE4sa in Flash-SWF-filen.
    Den troligaste orsaken \xE4r att Ruffle fick en ogiltig URL till SWF-filen.
error-unknown =
    Ruffle har st\xF6tt p\xE5 ett stort fel medan den f\xF6rs\xF6kte visa Flash-inneh\xE5llet.
    { $outdated ->
        [true] Om du \xE4r serveradministrat\xF6ren f\xF6rs\xF6k att ladda upp en nyare version av Ruffle (nuvarande version \xE4r utdaterad: { $buildDate }).
       *[false] Detta \xE4r inte t\xE4nkt att h\xE4nda s\xE5 vi skulle verkligen uppskatta om du kunde rapportera in en bugg!
    }
`,"save-manager.ftl":`save-delete-prompt = \xC4r du s\xE4ker p\xE5 att du vill radera sparfilen?
save-reload-prompt =
    Det enda s\xE4ttet att { $action ->
        [delete] radera
       *[replace] ers\xE4tta
    } denna sparfil utan potentiell konflikt \xE4r att ladda om inneh\xE5llet. Vill du forts\xE4tta \xE4nd\xE5?
save-download = Ladda ned
save-replace = Ers\xE4tt
save-delete = Ta bort
save-backup-all = Ladda ned alla sparfiler
`,"volume-controls.ftl":`volume-controls-mute = St\xE4ng av ljud
volume-controls-unmute = S\xE4tt p\xE5 ljud
`},"th-TH":{"context_menu.ftl":`context-menu-volume-controls = \u0E1B\u0E38\u0E48\u0E21\u0E23\u0E30\u0E14\u0E31\u0E1A\u0E40\u0E2A\u0E35\u0E22\u0E07
`,"messages.ftl":`ruffle-demo = \u0E40\u0E27\u0E47\u0E1A\u0E15\u0E31\u0E27\u0E2D\u0E22\u0E48\u0E32\u0E07
ruffle-wiki = \u0E14\u0E39\u0E27\u0E34\u0E01\u0E34 Ruffle
`,"save-manager.ftl":`save-delete-prompt = \u0E04\u0E38\u0E13\u0E41\u0E19\u0E48\u0E43\u0E08\u0E2B\u0E23\u0E37\u0E2D\u0E27\u0E48\u0E32\u0E08\u0E30\u0E25\u0E1A\u0E44\u0E1F\u0E25\u0E4C\u0E19\u0E35\u0E49?
`,"volume-controls.ftl":`volume-controls-mute = \u0E1B\u0E34\u0E14\u0E40\u0E2A\u0E35\u0E22\u0E07
volume-controls-unmute = \u0E43\u0E0A\u0E49\u0E40\u0E2A\u0E35\u0E22\u0E07
`},"tr-TR":{"context_menu.ftl":`context-menu-download-swf = .swf'i indir
context-menu-copy-debug-info = Hata ay\u0131klama bilgisini kopyala
context-menu-open-save-manager = Kay\u0131t y\xF6neticisini a\xE7
context-menu-about-ruffle =
    { $flavor ->
        [extension] Ruffle Uzant\u0131s\u0131 Hakk\u0131nda ({ $version })
       *[other] Ruffle Hakk\u0131nda ({ $version })
    }
context-menu-hide = Bu men\xFCy\xFC gizle
context-menu-exit-fullscreen = Tam ekrandan \xE7\u0131k
context-menu-enter-fullscreen = Tam ekran yap
context-menu-volume-controls = Ses kontrolleri
`,"messages.ftl":`message-cant-embed =
    Ruffle, bu sayfaya g\xF6m\xFCl\xFC Flash'\u0131 \xE7al\u0131\u015Ft\u0131ramad\u0131.
    Bu sorunu ortadan kald\u0131rmak i\xE7in dosyay\u0131 ayr\u0131 bir sekmede a\xE7may\u0131 deneyebilirsiniz.
message-restored-from-bfcache =
    Taray\u0131c\u0131n\u0131z bu Flash i\xE7eri\u011Fini \xF6nceki bir oturumdan geri y\xFCkledi.
    S\u0131f\u0131rdan ba\u015Flamak i\xE7in sayfay\u0131 yeniden y\xFCkleyin.
panic-title = Bir \u015Feyler yanl\u0131\u015F gitti :(
more-info = Daha fazla bilgi
run-anyway = Yine de \xE7al\u0131\u015Ft\u0131r
continue = Devam et
report-bug = Hata bildir
update-ruffle = Ruffle'\u0131 g\xFCncelle
ruffle-demo = A\u011F Demosu
ruffle-desktop = Masa\xFCst\xFC uygulamas\u0131
ruffle-wiki = Ruffle wiki'yi g\xF6r\xFCnt\xFCle
enable-hardware-acceleration = Donan\u0131m h\u0131zland\u0131rmas\u0131 etkin de\u011Fil gibi g\xF6r\xFCn\xFCyor. Ruffle \xE7al\u0131\u015Fabilir ancak \xE7ok yava\u015F olabilir. Donan\u0131m h\u0131zland\u0131rmas\u0131n\u0131 nas\u0131l etkinle\u015Ftirebilece\u011Finizi bu linkten \xF6\u011Frenebilirsiniz:
enable-hardware-acceleration-link = SSS - Chrome Donan\u0131m H\u0131zland\u0131rmas\u0131
view-error-details = Hata ayr\u0131nt\u0131lar\u0131n\u0131 g\xF6r\xFCnt\xFCle
open-in-new-tab = Yeni sekmede a\xE7
click-to-unmute = Sesi a\xE7mak i\xE7in t\u0131klay\u0131n
clipboard-message-title = Ruffle'da kopyalama ve yap\u0131\u015Ft\u0131rma
clipboard-message-description =
    { $variant ->
    *[unsupported] Taray\u0131c\u0131n\u0131z tam panoya eri\u015Fimi desteklemiyor,
    [access-denied] Pano eri\u015Fimi reddedildi,
    } ancak pano yerine her zaman bu k\u0131sayollar\u0131 kullanabilirsiniz:
clipboard-message-copy = { " " } kopyalamak i\xE7in
clipboard-message-cut = { " " } kesmek i\xE7in
clipboard-message-paste = { " " } yap\u0131\u015Ft\u0131rmak i\xE7in
error-canvas-reload = Tuval olu\u015Fturucusu kullan\u0131mda oldu\u011Funda tuval olu\u015Fturucusu ile yeniden y\xFCkleme yap\u0131lamaz.
error-file-protocol =
    G\xF6r\xFCn\xFC\u015Fe g\xF6re Ruffle'\u0131 "dosya:" protokol\xFCnde \xE7al\u0131\u015Ft\u0131r\u0131yorsunuz.
    Taray\u0131c\u0131lar g\xFCvenlik nedenleriyle bir\xE7ok \xF6zelli\u011Fin \xE7al\u0131\u015Fmas\u0131n\u0131 engelledi\u011Finden bu i\u015Fe yaramaz.
    Bunun yerine, sizi yerel bir sunucu kurmaya veya a\u011F\u0131n demosunu ya da masa\xFCst\xFC uygulamas\u0131n\u0131 kullanmaya davet ediyoruz.
error-javascript-config =
    Ruffle, yanl\u0131\u015F bir JavaScript yap\u0131land\u0131rmas\u0131 nedeniyle \xF6nemli bir sorunla kar\u015F\u0131la\u015Ft\u0131.
    Sunucu y\xF6neticisiyseniz, hangi parametrenin hatal\u0131 oldu\u011Funu bulmak i\xE7in sizi hata ayr\u0131nt\u0131lar\u0131n\u0131 kontrol etmeye davet ediyoruz.
    Yard\u0131m i\xE7in Ruffle wiki'sine de ba\u015Fvurabilirsiniz.
error-wasm-not-found =
    Ruffle gerekli ".wasm" dosya bile\u015Fenini y\xFCkleyemedi.
    Sunucu y\xF6neticisi iseniz, l\xFCtfen dosyan\u0131n do\u011Fru bir \u015Fekilde y\xFCklendi\u011Finden emin olun.
    Sorun devam ederse, "publicPath" ayar\u0131n\u0131 kullanman\u0131z gerekebilir: yard\u0131m i\xE7in l\xFCtfen Ruffle wiki'sine ba\u015Fvurun.
error-wasm-mime-type =
    Ruffle, ba\u015Flatmaya \xE7al\u0131\u015F\u0131rken \xF6nemli bir sorunla kar\u015F\u0131la\u015Ft\u0131.
    Bu web sunucusu, do\u011Fru MIME tipinde ".wasm" dosyalar\u0131 sunmuyor.
    Sunucu y\xF6neticisiyseniz, yard\u0131m i\xE7in l\xFCtfen Ruffle wiki'sine ba\u015Fvurun.
error-invalid-swf =
    Ruffle istenen dosyay\u0131 ayr\u0131\u015Ft\u0131ram\u0131yor.
    Bunun en olas\u0131 nedeni, istenen dosyan\u0131n ge\xE7erli bir SWF olmamas\u0131d\u0131r.
error-swf-fetch =
    Ruffle, Flash SWF dosyas\u0131n\u0131 y\xFCkleyemedi.
    Bunun en olas\u0131 nedeni, dosyan\u0131n art\u0131k mevcut olmamas\u0131 ve bu nedenle Ruffle'\u0131n y\xFCkleyece\u011Fi hi\xE7bir \u015Feyin olmamas\u0131d\u0131r.
    Yard\u0131m i\xE7in web sitesi y\xF6neticisiyle ileti\u015Fime ge\xE7meyi deneyin.
error-swf-cors =
    Ruffle, Flash SWF dosyas\u0131n\u0131 y\xFCkleyemedi.
    Getirme eri\u015Fimi muhtemelen CORS politikas\u0131 taraf\u0131ndan engellenmi\u015Ftir.
    Sunucu y\xF6neticisiyseniz, yard\u0131m i\xE7in l\xFCtfen Ruffle wiki'sine ba\u015Fvurun.
error-wasm-cors =
    Ruffle gerekli ".wasm" dosya bile\u015Fenini y\xFCkleyemedi.
    Getirme eri\u015Fimi muhtemelen CORS politikas\u0131 taraf\u0131ndan engellenmi\u015Ftir.
    Sunucu y\xF6neticisiyseniz, yard\u0131m i\xE7in l\xFCtfen Ruffle wiki'sine ba\u015Fvurun.
error-wasm-invalid =
    Ruffle, ba\u015Flatmaya \xE7al\u0131\u015F\u0131rken \xF6nemli bir sorunla kar\u015F\u0131la\u015Ft\u0131.
    G\xF6r\xFCn\xFC\u015Fe g\xF6re bu sayfada Ruffle'\u0131 \xE7al\u0131\u015Ft\u0131rmak i\xE7in eksik veya ge\xE7ersiz dosyalar var.
    Sunucu y\xF6neticisiyseniz, yard\u0131m i\xE7in l\xFCtfen Ruffle wiki'sine ba\u015Fvurun.
error-wasm-download =
    Ruffle, ba\u015Flatmaya \xE7al\u0131\u015F\u0131rken \xF6nemli bir sorunla kar\u015F\u0131la\u015Ft\u0131.
    Bu genellikle kendi kendine \xE7\xF6z\xFClebilir, bu nedenle sayfay\u0131 yeniden y\xFCklemeyi deneyebilirsiniz.
    Aksi takdirde, l\xFCtfen site y\xF6neticisiyle ileti\u015Fime ge\xE7in.
error-wasm-disabled-on-edge =
    Ruffle gerekli ".wasm" dosya bile\u015Fenini y\xFCkleyemedi.
    Bunu d\xFCzeltmek i\xE7in taray\u0131c\u0131n\u0131z\u0131n ayarlar\u0131n\u0131 a\xE7\u0131n, "Gizlilik, arama ve hizmetler"i t\u0131klay\u0131n, a\u015Fa\u011F\u0131 kayd\u0131r\u0131n ve "Web'de g\xFCvenli\u011Finizi art\u0131r\u0131n"\u0131 kapatmay\u0131 deneyin.
    Bu, taray\u0131c\u0131n\u0131z\u0131n gerekli ".wasm" dosyalar\u0131n\u0131 y\xFCklemesine izin verecektir.
    Sorun devam ederse, farkl\u0131 bir taray\u0131c\u0131 kullanman\u0131z gerekebilir.
error-wasm-unsupported-browser =
    Kulland\u0131\u011F\u0131n\u0131z taray\u0131c\u0131, Ruffle'\u0131n \xE7al\u0131\u015Fmas\u0131 i\xE7in gereken WebAssembly uzant\u0131lar\u0131n\u0131 desteklemiyor.
    L\xFCtfen desteklenen bir taray\u0131c\u0131ya ge\xE7in.
    Wiki'de desteklenen taray\u0131c\u0131lar\u0131n bir listesini bulabilirsiniz.
error-javascript-conflict =
    Ruffle, ba\u015Flatmaya \xE7al\u0131\u015F\u0131rken \xF6nemli bir sorunla kar\u015F\u0131la\u015Ft\u0131.
    G\xF6r\xFCn\xFC\u015Fe g\xF6re bu sayfa, Ruffle ile \xE7ak\u0131\u015Fan JavaScript kodu kullan\u0131yor.
    Sunucu y\xF6neticisiyseniz, sizi dosyay\u0131 bo\u015F bir sayfaya y\xFCklemeyi denemeye davet ediyoruz.
error-javascript-conflict-outdated = Ayr\u0131ca sorunu giderebilecek daha yeni bir Ruffle s\xFCr\xFCm\xFC y\xFCklemeyi de deneyebilirsiniz (mevcut yap\u0131m eskimi\u015F: { $buildDate }).
error-csp-conflict =
    Ruffle, ba\u015Flatmaya \xE7al\u0131\u015F\u0131rken \xF6nemli bir sorunla kar\u015F\u0131la\u015Ft\u0131.
    Bu web sunucusunun \u0130\xE7erik G\xFCvenli\u011Fi Politikas\u0131, gerekli ".wasm" bile\u015Feninin \xE7al\u0131\u015Fmas\u0131na izin vermiyor.
    Sunucu y\xF6neticisiyseniz, yard\u0131m i\xE7in l\xFCtfen Ruffle wiki'sine bak\u0131n.
error-url-invalid =
    Ruffle, Flash SWF dosyas\u0131n\u0131 y\xFCkleyemedi.
    Bunun en olas\u0131 nedeni, SWF dosyas\u0131 i\xE7in Ruffle'a ge\xE7ersiz bir URL iletilmi\u015F olmas\u0131d\u0131r.
error-unknown =
    Ruffle, bu Flash i\xE7eri\u011Fini g\xF6r\xFCnt\xFClemeye \xE7al\u0131\u015F\u0131rken \xF6nemli bir sorunla kar\u015F\u0131la\u015Ft\u0131.
    { $outdated ->
        [true] Sunucu y\xF6neticisiyseniz, l\xFCtfen Ruffle'\u0131n daha yeni bir s\xFCr\xFCm\xFCn\xFC y\xFCklemeyi deneyin (mevcut yap\u0131m eskimi\u015F: { $buildDate }).
       *[false] Bunun olmamas\u0131 gerekiyor, bu y\xFCzden bir hata bildirebilirseniz \xE7ok memnun oluruz!
    }
`,"save-manager.ftl":`save-delete-prompt = Bu kay\u0131t dosyas\u0131n\u0131 silmek istedi\u011Finize emin misiniz?
save-reload-prompt =
    Bu kaydetme dosyas\u0131n\u0131 potansiyel \xE7ak\u0131\u015Fma olmadan { $action ->
        [delete] silmenin
       *[replace] de\u011Fi\u015Ftirmenin
    } tek yolu, bu i\xE7eri\u011Fi yeniden y\xFCklemektir. Yine de devam etmek istiyor musunuz?
save-download = \u0130ndir
save-replace = De\u011Fi\u015Ftir
save-delete = Sil
save-backup-all = T\xFCm kay\u0131t dosyalar\u0131n\u0131 indir
`,"volume-controls.ftl":`volume-controls-mute = Sustur
volume-controls-unmute = Susturmay\u0131 kald\u0131r
`},"tt-RU":{"context_menu.ftl":`context-menu-download-swf = SWF \u0444\u0430\u0439\u043B\u043D\u044B \u0439\u04E9\u043A\u043B\u04D9\u04AF
context-menu-copy-debug-info = \u0414\u0435\u0431\u0430\u0433 \u043C\u04D9\u0433\u044A\u043B\u04AF\u043C\u0430\u0442\u044B\u043D \u043A\u04AF\u0447\u0435\u0440\u04AF
context-menu-open-save-manager = \u0421\u0430\u043A\u043B\u0430\u0443 \u043C\u0435\u043D\u0435\u0434\u0436\u0435\u0440\u044B\u043D \u0430\u0447\u0443
context-menu-about-ruffle =
    { $flavor ->
        [extension] Ruffle \u04E9\u0441\u0442\u04D9\u043C\u04D9\u0441\u0435 \u0442\u0443\u0440\u044B\u043D\u0434\u0430 ({ $version })
       *[other] Ruffle \u0442\u0443\u0440\u044B\u043D\u0434\u0430 ({ $version })
    }
context-menu-hide = \u0411\u0443 \u043C\u0435\u043D\u044E\u043D\u044B \u044F\u0448\u0435\u0440
context-menu-exit-fullscreen = \u0422\u0443\u043B\u044B \u044D\u043A\u0440\u0430\u043D\u043D\u0430\u043D \u0447\u044B\u0433\u0443
context-menu-enter-fullscreen = \u0422\u0443\u043B\u044B \u044D\u043A\u0440\u0430\u043D\u043D\u0430\u043D \u043A\u04AF\u0447\u04AF
context-menu-volume-controls = \u0422\u0430\u0432\u044B\u0448 \u043A\u04E9\u0439\u043B\u04D9\u04AF\u043B\u04D9\u0440\u0435
`,"messages.ftl":`panic-title = \u041D\u04D9\u0440\u0441\u04D9\u0434\u0435\u0440 \u0434\u04E9\u0440\u0435\u0441 \u044D\u0448\u043B\u04D9\u043C\u04D9\u0433\u04D9\u043D :(
more-info = \u0422\u0443\u043B\u044B\u0440\u0430\u043A
run-anyway = \u0411\u0430\u0440\u044B\u0431\u0435\u0440 \u044D\u0448\u043B\u04D9\u0442
continue = \u0414\u04D9\u0432\u0430\u043C \u0438\u0442\u04AF
report-bug = \u0425\u0430\u0442\u0430 \u0442\u0443\u0440\u044B\u043D\u0434\u0430 \u0445\u04D9\u0431\u04D9\u0440 \u0438\u0442\u04AF
open-in-new-tab = \u042F\u04A3\u0430 \u0441\u0430\u043B\u044B\u043D\u043C\u0430\u0434\u0430 \u0430\u0447\u0443
`,"save-manager.ftl":"","volume-controls.ftl":`volume-controls-mute = \u0422\u0430\u0432\u044B\u0448\u043D\u044B \u044F\u0431\u0443
volume-controls-unmute = \u0422\u0430\u0432\u044B\u0448\u043D\u044B \u0430\u0447\u0443
`},"uk-UA":{"context_menu.ftl":`context-menu-download-swf = \u0417\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0438\u0442\u0438 SWF
context-menu-copy-debug-info = \u041A\u043E\u043F\u0456\u044E\u0432\u0430\u0442\u0438 \u0456\u043D\u0444\u043E\u0440\u043C\u0430\u0446\u0456\u044E \u043F\u0440\u043E \u043D\u0430\u043B\u0430\u0433\u043E\u0434\u0436\u0435\u043D\u043D\u044F
context-menu-open-save-manager = \u0412\u0456\u0434\u043A\u0440\u0438\u0442\u0438 \u043C\u0435\u043D\u0435\u0434\u0436\u0435\u0440 \u0437\u0431\u0435\u0440\u0435\u0436\u0435\u043D\u043D\u044F
context-menu-about-ruffle =
    { $flavor ->
        [extension] \u041F\u0440\u043E \u0440\u043E\u0437\u0448\u0438\u0440\u0435\u043D\u043D\u044F Ruffle ({ $version })
       *[other] \u041F\u0440\u043E Ruffle ({ $version })
    }
context-menu-hide = \u041F\u0440\u0438\u0445\u043E\u0432\u0430\u0442\u0438 \u0446\u0435 \u043C\u0435\u043D\u044E
context-menu-exit-fullscreen = \u0412\u0438\u0439\u0442\u0438 \u0437 \u043F\u043E\u0432\u043D\u043E\u0435\u043A\u0440\u0430\u043D\u043D\u043E\u0433\u043E \u0440\u0435\u0436\u0438\u043C\u0443
context-menu-enter-fullscreen = \u041F\u0435\u0440\u0435\u0439\u0442\u0438 \u0432 \u043F\u043E\u0432\u043D\u043E\u0435\u043A\u0440\u0430\u043D\u043D\u0438\u0439 \u0440\u0435\u0436\u0438\u043C
context-menu-volume-controls = \u0415\u043B\u0435\u043C\u0435\u043D\u0442\u0438 \u043A\u0435\u0440\u0443\u0432\u0430\u043D\u043D\u044F \u0433\u0443\u0447\u043D\u0456\u0441\u0442\u044E
`,"messages.ftl":`message-cant-embed = Ruffle \u043D\u0435 \u0432\u0434\u0430\u043B\u043E\u0441\u044F \u0437\u0430\u043F\u0443\u0441\u0442\u0438\u0442\u0438 Flash, \u0432\u0431\u0443\u0434\u043E\u0432\u0430\u043D\u0438\u0439 \u0443 \u0446\u044E \u0441\u0442\u043E\u0440\u0456\u043D\u043A\u0443. \u0412\u0438 \u043C\u043E\u0436\u0435\u0442\u0435 \u0441\u043F\u0440\u043E\u0431\u0443\u0432\u0430\u0442\u0438 \u0432\u0456\u0434\u043A\u0440\u0438\u0442\u0438 \u0444\u0430\u0439\u043B \u0432 \u043E\u043A\u0440\u0435\u043C\u0456\u0439 \u0432\u043A\u043B\u0430\u0434\u0446\u0456, \u0449\u043E\u0431 \u0443\u043D\u0438\u043A\u043D\u0443\u0442\u0438 \u0446\u0456\u0454\u0457 \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u0438.
message-restored-from-bfcache =
    \u0412\u0430\u0448 \u0431\u0440\u0430\u0443\u0437\u0435\u0440 \u0432\u0456\u0434\u043D\u043E\u0432\u0438\u0432 \u0446\u0435\u0439 Flash-\u0432\u043C\u0456\u0441\u0442 \u0456\u0437 \u043F\u043E\u043F\u0435\u0440\u0435\u0434\u043D\u044C\u043E\u0457 \u0441\u0435\u0441\u0456\u0457.
    \u0429\u043E\u0431 \u043F\u043E\u0447\u0430\u0442\u0438 \u0437\u0430\u043D\u043E\u0432\u043E, \u043E\u043D\u043E\u0432\u0456\u0442\u044C \u0441\u0442\u043E\u0440\u0456\u043D\u043A\u0443.
panic-title = \u0429\u043E\u0441\u044C \u043F\u0456\u0448\u043B\u043E \u043D\u0435 \u0442\u0430\u043A :(
more-info = \u0411\u0456\u043B\u044C\u0448\u0435 \u0456\u043D\u0444\u043E\u0440\u043C\u0430\u0446\u0456\u0457
run-anyway = \u0417\u0430\u043F\u0443\u0441\u0442\u0438\u0442\u0438 \u0432\u0441\u0435 \u043E\u0434\u043D\u043E
continue = \u041F\u0440\u043E\u0434\u043E\u0432\u0436\u0438\u0442\u0438
report-bug = \u041F\u043E\u0432\u0456\u0434\u043E\u043C\u0438\u0442\u0438 \u043F\u0440\u043E \u043F\u043E\u043C\u0438\u043B\u043A\u0443
update-ruffle = \u041E\u043D\u043E\u0432\u0438\u0442\u0438 Ruffle
ruffle-demo = \u0412\u0435\u0431\u0434\u0435\u043C\u043E\u043D\u0441\u0442\u0440\u0430\u0446\u0456\u044F
ruffle-desktop = \u0417\u0430\u0441\u0442\u043E\u0441\u0443\u043D\u043E\u043A \u0440\u043E\u0431\u043E\u0447\u043E\u0433\u043E \u0441\u0442\u043E\u043B\u0443
ruffle-wiki = \u041F\u0435\u0440\u0435\u0433\u043B\u044F\u043D\u0443\u0442\u0438 Ruffle Wiki
enable-hardware-acceleration = \u0421\u0445\u043E\u0436\u0435, \u0430\u043F\u0430\u0440\u0430\u0442\u043D\u0435 \u043F\u0440\u0438\u0441\u043A\u043E\u0440\u0435\u043D\u043D\u044F \u0432\u0438\u043C\u043A\u043D\u0435\u043D\u043E. \u0425\u043E\u0447\u0430 Ruffle \u043C\u043E\u0436\u0435 \u043F\u0440\u0430\u0446\u044E\u0432\u0430\u0442\u0438, \u0446\u0435 \u043C\u043E\u0436\u0435 \u0431\u0443\u0442\u0438 \u0434\u0443\u0436\u0435 \u043F\u043E\u0432\u0456\u043B\u044C\u043D\u0438\u043C. \u0412\u0438 \u043C\u043E\u0436\u0435\u0442\u0435 \u0434\u0456\u0437\u043D\u0430\u0442\u0438\u0441\u044F, \u044F\u043A \u0443\u0432\u0456\u043C\u043A\u043D\u0443\u0442\u0438 \u0430\u043F\u0430\u0440\u0430\u0442\u043D\u0435 \u043F\u0440\u0438\u0441\u043A\u043E\u0440\u0435\u043D\u043D\u044F, \u043F\u0435\u0440\u0435\u0439\u0448\u043E\u0432\u0448\u0438 \u0437\u0430 \u043F\u043E\u0441\u0438\u043B\u0430\u043D\u043D\u044F\u043C \u043D\u0438\u0436\u0447\u0435:
enable-hardware-acceleration-link = FAQ - \u0410\u043F\u0430\u0440\u0430\u0442\u043D\u0435 \u043F\u0440\u0438\u0441\u043A\u043E\u0440\u0435\u043D\u043D\u044F Chrome
view-error-details = \u041F\u0435\u0440\u0435\u0433\u043B\u044F\u043D\u0443\u0442\u0438 \u0434\u0435\u0442\u0430\u043B\u0456 \u043F\u043E\u043C\u0438\u043B\u043A\u0438
open-in-new-tab = \u0412\u0456\u0434\u043A\u0440\u0438\u0442\u0438 \u0432 \u043D\u043E\u0432\u0456\u0439 \u0432\u043A\u043B\u0430\u0434\u0446\u0456
click-to-unmute = \u041D\u0430\u0442\u0438\u0441\u043D\u0456\u0442\u044C, \u0449\u043E\u0431 \u0443\u0432\u0456\u043C\u043A\u043D\u0443\u0442\u0438 \u0437\u0432\u0443\u043A
clipboard-message-title = \u041A\u043E\u043F\u0456\u044E\u0432\u0430\u043D\u043D\u044F \u0442\u0430 \u0432\u0441\u0442\u0430\u0432\u043B\u0435\u043D\u043D\u044F \u0432 Ruffle
clipboard-message-description =
    { $variant ->
       *[unsupported] \u0412\u0430\u0448 \u0431\u0440\u0430\u0443\u0437\u0435\u0440 \u043D\u0435 \u043F\u0456\u0434\u0442\u0440\u0438\u043C\u0443\u0454 \u043F\u043E\u0432\u043D\u0438\u0439 \u0434\u043E\u0441\u0442\u0443\u043F \u0434\u043E \u0431\u0443\u0444\u0435\u0440\u0430 \u043E\u0431\u043C\u0456\u043D\u0443,
        [access-denied] \u0423 \u0434\u043E\u0441\u0442\u0443\u043F\u0456 \u0434\u043E \u0431\u0443\u0444\u0435\u0440\u0430 \u043E\u0431\u043C\u0456\u043D\u0443 \u0432\u0456\u0434\u043C\u043E\u0432\u043B\u0435\u043D\u043E,
    } \u0430\u043B\u0435 \u0432\u0438 \u0437\u0430\u0432\u0436\u0434\u0438 \u043C\u043E\u0436\u0435\u0442\u0435 \u0441\u043A\u043E\u0440\u0438\u0441\u0442\u0430\u0442\u0438\u0441\u044F \u0446\u0438\u043C\u0438 \u044F\u0440\u043B\u0438\u043A\u0430\u043C\u0438:
clipboard-message-copy = { " " } \u0434\u043B\u044F \u043A\u043E\u043F\u0456\u044E\u0432\u0430\u043D\u043D\u044F
clipboard-message-cut = { " " } \u0434\u043B\u044F \u0432\u0438\u0440\u0456\u0437\u0430\u043D\u043D\u044F
clipboard-message-paste = { " " } \u0434\u043B\u044F \u0432\u0441\u0442\u0430\u0432\u043B\u0435\u043D\u043D\u044F
error-canvas-reload = \u041D\u0435\u043C\u043E\u0436\u043B\u0438\u0432\u043E \u043E\u043D\u043E\u0432\u0438\u0442\u0438 \u0437 Canvas \u0440\u0435\u043D\u0434\u0435\u0440\u0435\u0440\u043E\u043C, \u043A\u043E\u043B\u0438 Canvas \u0440\u0435\u043D\u0434\u0435\u0440\u0435\u0440 \u0432\u0436\u0435 \u0432\u0438\u043A\u043E\u0440\u0438\u0441\u0442\u043E\u0432\u0443\u0454\u0442\u044C\u0441\u044F.
error-file-protocol = \u0417\u0434\u0430\u0454\u0442\u044C\u0441\u044F, \u0432\u0438 \u0437\u0430\u043F\u0443\u0441\u043A\u0430\u0454\u0442\u0435 Ruffle \u0437\u0430 \u043F\u0440\u043E\u0442\u043E\u043A\u043E\u043B\u043E\u043C "file:". \u0426\u0435 \u043D\u0435 \u043F\u0440\u0430\u0446\u044E\u0454, \u043E\u0441\u043A\u0456\u043B\u044C\u043A\u0438 \u0431\u0440\u0430\u0443\u0437\u0435\u0440\u0438 \u0431\u043B\u043E\u043A\u0443\u044E\u0442\u044C \u0440\u043E\u0431\u043E\u0442\u0443 \u0431\u0430\u0433\u0430\u0442\u044C\u043E\u0445 \u0444\u0443\u043D\u043A\u0446\u0456\u0439 \u0437 \u043C\u0456\u0440\u043A\u0443\u0432\u0430\u043D\u044C \u0431\u0435\u0437\u043F\u0435\u043A\u0438. \u0417\u0430\u043C\u0456\u0441\u0442\u044C \u0446\u044C\u043E\u0433\u043E \u043C\u0438 \u0437\u0430\u043F\u0440\u043E\u0448\u0443\u0454\u043C\u043E \u0432\u0430\u0441 \u043D\u0430\u043B\u0430\u0448\u0442\u0443\u0432\u0430\u0442\u0438 \u043B\u043E\u043A\u0430\u043B\u044C\u043D\u0438\u0439 \u0441\u0435\u0440\u0432\u0435\u0440 \u0430\u0431\u043E \u0441\u043A\u043E\u0440\u0438\u0441\u0442\u0430\u0442\u0438\u0441\u044F \u0432\u0435\u0431\u0434\u0435\u043C\u043E\u043D\u0441\u0442\u0440\u0430\u0446\u0456\u0454\u044E \u0447\u0438 \u0437\u0430\u0441\u0442\u043E\u0441\u0443\u043D\u043A\u043E\u043C \u0440\u043E\u0431\u043E\u0447\u043E\u0433\u043E \u0441\u0442\u043E\u043B\u0443.
error-javascript-config = Ruffle \u0437\u0456\u0442\u043A\u043D\u0443\u0432\u0441\u044F \u0437 \u0441\u0435\u0440\u0439\u043E\u0437\u043D\u043E\u044E \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u043E\u044E \u0447\u0435\u0440\u0435\u0437 \u043D\u0435\u043F\u0440\u0430\u0432\u0438\u043B\u044C\u043D\u0443 \u043A\u043E\u043D\u0444\u0456\u0433\u0443\u0440\u0430\u0446\u0456\u044E JavaScript. \u042F\u043A\u0449\u043E \u0432\u0438 \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043C\u0438 \u043F\u0440\u043E\u043F\u043E\u043D\u0443\u0454\u043C\u043E \u0432\u0430\u043C \u043F\u0435\u0440\u0435\u0432\u0456\u0440\u0438\u0442\u0438 \u0434\u0435\u0442\u0430\u043B\u0456 \u043F\u043E\u043C\u0438\u043B\u043A\u0438, \u0449\u043E\u0431 \u0434\u0456\u0437\u043D\u0430\u0442\u0438\u0441\u044F, \u044F\u043A\u0438\u0439 \u043F\u0430\u0440\u0430\u043C\u0435\u0442\u0440 \u0454 \u043D\u0435\u0441\u043F\u0440\u0430\u0432\u043D\u0438\u043C. \u0412\u0438 \u0442\u0430\u043A\u043E\u0436 \u043C\u043E\u0436\u0435\u0442\u0435 \u0437\u0432\u0435\u0440\u043D\u0443\u0442\u0438\u0441\u044F \u0437\u0430 \u0434\u043E\u043F\u043E\u043C\u043E\u0433\u043E\u044E \u0434\u043E Ruffle Wiki.
error-wasm-not-found = Ruffle \u043D\u0435 \u0432\u0434\u0430\u043B\u043E\u0441\u044F \u0437\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0438\u0442\u0438 \u043D\u0435\u043E\u0431\u0445\u0456\u0434\u043D\u0438\u0439 \u0444\u0430\u0439\u043B\u043E\u0432\u0438\u0439 \u043A\u043E\u043C\u043F\u043E\u043D\u0435\u043D\u0442 ".wasm". \u042F\u043A\u0449\u043E \u0432\u0438 \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043F\u0435\u0440\u0435\u043A\u043E\u043D\u0430\u0439\u0442\u0435\u0441\u044F, \u0449\u043E \u0444\u0430\u0439\u043B \u0437\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0435\u043D\u043E \u043F\u0440\u0430\u0432\u0438\u043B\u044C\u043D\u043E. \u042F\u043A\u0449\u043E \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u0430 \u043D\u0435 \u0437\u043D\u0438\u043A\u0430\u0454, \u043C\u043E\u0436\u043B\u0438\u0432\u043E, \u0432\u0430\u043C \u0437\u043D\u0430\u0434\u043E\u0431\u0438\u0442\u044C\u0441\u044F \u0441\u043A\u043E\u0440\u0438\u0441\u0442\u0430\u0442\u0438\u0441\u044F \u043D\u0430\u043B\u0430\u0448\u0442\u0443\u0432\u0430\u043D\u043D\u044F\u043C "publicPath": \u0431\u0443\u0434\u044C \u043B\u0430\u0441\u043A\u0430, \u0437\u0432\u0435\u0440\u043D\u0456\u0442\u044C\u0441\u044F \u0434\u043E Ruffle Wiki, \u0449\u043E\u0431 \u043E\u0442\u0440\u0438\u043C\u0430\u0442\u0438 \u0434\u043E\u043F\u043E\u043C\u043E\u0433\u0443.
error-wasm-mime-type = Ruffle \u0437\u0456\u0442\u043A\u043D\u0443\u0432\u0441\u044F \u0437 \u0441\u0435\u0440\u0439\u043E\u0437\u043D\u043E\u044E \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u043E\u044E \u043F\u0456\u0434 \u0447\u0430\u0441 \u0441\u043F\u0440\u043E\u0431\u0438 \u0456\u043D\u0456\u0446\u0456\u0430\u043B\u0456\u0437\u0430\u0446\u0456\u0457. \u0426\u0435\u0439 \u0432\u0435\u0431\u0441\u0435\u0440\u0432\u0435\u0440 \u043D\u0435 \u043E\u0431\u0441\u043B\u0443\u0433\u043E\u0432\u0443\u0454 \u0444\u0430\u0439\u043B\u0438 ".wasm" \u0456\u0437 \u043F\u0440\u0430\u0432\u0438\u043B\u044C\u043D\u0438\u043C \u0442\u0438\u043F\u043E\u043C MIME. \u042F\u043A\u0449\u043E \u0432\u0438 \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u0437\u0432\u0435\u0440\u043D\u0456\u0442\u044C\u0441\u044F \u0434\u043E Ruffle Wiki, \u0449\u043E\u0431 \u043E\u0442\u0440\u0438\u043C\u0430\u0442\u0438 \u0434\u043E\u043F\u043E\u043C\u043E\u0433\u0443.
error-invalid-swf = Ruffle \u043D\u0435 \u043C\u043E\u0436\u0435 \u043F\u0440\u043E\u0430\u043D\u0430\u043B\u0456\u0437\u0443\u0432\u0430\u0442\u0438 \u0444\u0430\u0439\u043B \u0437\u0430\u043F\u0438\u0442\u0443. \u041D\u0430\u0439\u0456\u043C\u043E\u0432\u0456\u0440\u043D\u0456\u0448\u0430 \u043F\u0440\u0438\u0447\u0438\u043D\u0430 \u043F\u043E\u043B\u044F\u0433\u0430\u0454 \u0432 \u0442\u043E\u043C\u0443, \u0449\u043E \u0444\u0430\u0439\u043B \u0437\u0430\u043F\u0438\u0442\u0443 \u043D\u0435 \u0454 \u0434\u0456\u0439\u0441\u043D\u0438\u043C SWF.
error-swf-fetch = Ruffle \u043D\u0435 \u0432\u0434\u0430\u043B\u043E\u0441\u044F \u0437\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0438\u0442\u0438 \u0444\u0430\u0439\u043B Flash SWF. \u041D\u0430\u0439\u0456\u043C\u043E\u0432\u0456\u0440\u043D\u0456\u0448\u0430 \u043F\u0440\u0438\u0447\u0438\u043D\u0430 \u043F\u043E\u043B\u044F\u0433\u0430\u0454 \u0432 \u0442\u043E\u043C\u0443, \u0449\u043E \u0444\u0430\u0439\u043B \u0431\u0456\u043B\u044C\u0448\u0435 \u043D\u0435 \u0456\u0441\u043D\u0443\u0454, \u0442\u043E\u043C\u0443 Ruffle \u043D\u0435\u043C\u0430 \u0447\u043E\u0433\u043E \u0437\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0438\u0442\u0438. \u0421\u043F\u0440\u043E\u0431\u0443\u0439\u0442\u0435 \u0437\u0432\u0435\u0440\u043D\u0443\u0442\u0438\u0441\u044F \u043F\u043E \u0434\u043E\u043F\u043E\u043C\u043E\u0433\u0443 \u0434\u043E \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u0430 \u0441\u0430\u0439\u0442\u0443.
error-swf-cors = Ruffle \u043D\u0435 \u0432\u0434\u0430\u043B\u043E\u0441\u044F \u0437\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0438\u0442\u0438 \u0444\u0430\u0439\u043B Flash SWF. \u041C\u043E\u0436\u043B\u0438\u0432\u043E, \u0434\u043E\u0441\u0442\u0443\u043F \u0434\u043E \u043E\u0442\u0440\u0438\u043C\u0430\u043D\u043D\u044F \u0431\u0443\u043B\u043E \u0437\u0430\u0431\u043B\u043E\u043A\u043E\u0432\u0430\u043D\u043E \u043F\u043E\u043B\u0456\u0442\u0438\u043A\u043E\u044E CORS. \u042F\u043A\u0449\u043E \u0432\u0438 \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u0437\u0432\u0435\u0440\u043D\u0456\u0442\u044C\u0441\u044F \u0434\u043E Ruffle Wiki, \u0449\u043E\u0431 \u043E\u0442\u0440\u0438\u043C\u0430\u0442\u0438 \u0434\u043E\u043F\u043E\u043C\u043E\u0433\u0443.
error-wasm-cors = Ruffle \u043D\u0435 \u0432\u0434\u0430\u043B\u043E\u0441\u044F \u0437\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0438\u0442\u0438 \u043D\u0435\u043E\u0431\u0445\u0456\u0434\u043D\u0438\u0439 \u0444\u0430\u0439\u043B\u043E\u0432\u0438\u0439 \u043A\u043E\u043C\u043F\u043E\u043D\u0435\u043D\u0442 ".wasm". \u041C\u043E\u0436\u043B\u0438\u0432\u043E, \u0434\u043E\u0441\u0442\u0443\u043F \u0434\u043E \u043E\u0442\u0440\u0438\u043C\u0430\u043D\u043D\u044F \u0431\u0443\u043B\u043E \u0437\u0430\u0431\u043B\u043E\u043A\u043E\u0432\u0430\u043D\u043E \u043F\u043E\u043B\u0456\u0442\u0438\u043A\u043E\u044E CORS. \u042F\u043A\u0449\u043E \u0432\u0438 \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u0437\u0432\u0435\u0440\u043D\u0456\u0442\u044C\u0441\u044F \u0434\u043E Ruffle Wiki, \u0449\u043E\u0431 \u043E\u0442\u0440\u0438\u043C\u0430\u0442\u0438 \u0434\u043E\u043F\u043E\u043C\u043E\u0433\u0443.
error-wasm-invalid = Ruffle \u0437\u0456\u0442\u043A\u043D\u0443\u0432\u0441\u044F \u0437 \u0441\u0435\u0440\u0439\u043E\u0437\u043D\u043E\u044E \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u043E\u044E \u043F\u0456\u0434 \u0447\u0430\u0441 \u0441\u043F\u0440\u043E\u0431\u0438 \u0456\u043D\u0456\u0446\u0456\u0430\u043B\u0456\u0437\u0430\u0446\u0456\u0457. \u0417\u0434\u0430\u0454\u0442\u044C\u0441\u044F, \u043D\u0430 \u0446\u0456\u0439 \u0441\u0442\u043E\u0440\u0456\u043D\u0446\u0456 \u0432\u0456\u0434\u0441\u0443\u0442\u043D\u0456 \u0430\u0431\u043E \u043D\u0435\u0434\u0456\u0439\u0441\u043D\u0456 \u0444\u0430\u0439\u043B\u0438 \u0434\u043B\u044F \u0437\u0430\u043F\u0443\u0441\u043A\u0443 Ruffle. \u042F\u043A\u0449\u043E \u0432\u0438 \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u0437\u0432\u0435\u0440\u043D\u0456\u0442\u044C\u0441\u044F \u0434\u043E Ruffle Wiki, \u0449\u043E\u0431 \u043E\u0442\u0440\u0438\u043C\u0430\u0442\u0438 \u0434\u043E\u043F\u043E\u043C\u043E\u0433\u0443.
error-wasm-download = Ruffle \u0437\u0456\u0442\u043A\u043D\u0443\u0432\u0441\u044F \u0437 \u0441\u0435\u0440\u0439\u043E\u0437\u043D\u043E\u044E \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u043E\u044E \u043F\u0456\u0434 \u0447\u0430\u0441 \u0441\u043F\u0440\u043E\u0431\u0438 \u0456\u043D\u0456\u0446\u0456\u0430\u043B\u0456\u0437\u0430\u0446\u0456\u0457. \u0427\u0430\u0441\u0442\u043E \u0446\u0435 \u043C\u043E\u0436\u0435 \u0432\u0438\u0440\u0456\u0448\u0438\u0442\u0438\u0441\u044F \u0441\u0430\u043C\u043E \u0441\u043E\u0431\u043E\u044E, \u0442\u043E\u043C\u0443 \u0432\u0438 \u043C\u043E\u0436\u0435\u0442\u0435 \u0441\u043F\u0440\u043E\u0431\u0443\u0432\u0430\u0442\u0438 \u043E\u043D\u043E\u0432\u0438\u0442\u0438 \u0441\u0442\u043E\u0440\u0456\u043D\u043A\u0443. \u0412 \u0456\u043D\u0448\u043E\u043C\u0443 \u0432\u0438\u043F\u0430\u0434\u043A\u0443 \u0437\u0432\u0435\u0440\u043D\u0456\u0442\u044C\u0441\u044F \u0434\u043E \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u0430 \u0441\u0430\u0439\u0442\u0443.
error-wasm-disabled-on-edge = Ruffle \u043D\u0435 \u0432\u0434\u0430\u043B\u043E\u0441\u044F \u0437\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0438\u0442\u0438 \u043D\u0435\u043E\u0431\u0445\u0456\u0434\u043D\u0438\u0439 \u0444\u0430\u0439\u043B\u043E\u0432\u0438\u0439 \u043A\u043E\u043C\u043F\u043E\u043D\u0435\u043D\u0442 ".wasm". \u0429\u043E\u0431 \u0432\u0438\u043F\u0440\u0430\u0432\u0438\u0442\u0438 \u0446\u0435, \u0441\u043F\u0440\u043E\u0431\u0443\u0439\u0442\u0435 \u0432\u0456\u0434\u043A\u0440\u0438\u0442\u0438 \u043D\u0430\u043B\u0430\u0448\u0442\u0443\u0432\u0430\u043D\u043D\u044F \u0432\u0430\u0448\u043E\u0433\u043E \u0431\u0440\u0430\u0443\u0437\u0435\u0440\u0430, \u043D\u0430\u0442\u0438\u0441\u043D\u0443\u0442\u0438 \xAB\u041A\u043E\u043D\u0444\u0456\u0434\u0435\u043D\u0446\u0456\u0439\u043D\u0456\u0441\u0442\u044C, \u043F\u043E\u0448\u0443\u043A \u0456 \u0441\u043B\u0443\u0436\u0431\u0438\xBB, \u043F\u0440\u043E\u043A\u0440\u0443\u0442\u0438\u0442\u0438 \u0432\u043D\u0438\u0437 \u0456 \u0432\u0438\u043C\u043A\u043D\u0443\u0442\u0438 \xAB\u041F\u0456\u0434\u0432\u0438\u0449\u0438\u0442\u0438 \u0431\u0435\u0437\u043F\u0435\u043A\u0443 \u0432 \u0456\u043D\u0442\u0435\u0440\u043D\u0435\u0442\u0456\xBB. \u0426\u0435 \u0434\u043E\u0437\u0432\u043E\u043B\u0438\u0442\u044C \u0432\u0430\u0448\u043E\u043C\u0443 \u0431\u0440\u0430\u0443\u0437\u0435\u0440\u0443 \u0437\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0438\u0442\u0438 \u043D\u0435\u043E\u0431\u0445\u0456\u0434\u043D\u0456 \u0444\u0430\u0439\u043B\u0438 \xAB.wasm\xBB. \u042F\u043A\u0449\u043E \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u0430 \u043D\u0435 \u0437\u043D\u0438\u043A\u0430\u0454, \u043C\u043E\u0436\u043B\u0438\u0432\u043E, \u0432\u0430\u043C \u0434\u043E\u0432\u0435\u0434\u0435\u0442\u044C\u0441\u044F \u0441\u043A\u043E\u0440\u0438\u0441\u0442\u0430\u0442\u0438\u0441\u044F \u0456\u043D\u0448\u0438\u043C \u0431\u0440\u0430\u0443\u0437\u0435\u0440\u043E\u043C.
error-wasm-unsupported-browser =
    \u0412\u0430\u0448 \u0431\u0440\u0430\u0443\u0437\u0435\u0440 \u043D\u0435 \u043F\u0456\u0434\u0442\u0440\u0438\u043C\u0443\u0454 \u0440\u043E\u0437\u0448\u0438\u0440\u0435\u043D\u043D\u044F WebAssembly, \u043D\u0435\u043E\u0431\u0445\u0456\u0434\u043D\u0456 \u0434\u043B\u044F \u0440\u043E\u0431\u043E\u0442\u0438 Ruffle.
    \u0411\u0443\u0434\u044C \u043B\u0430\u0441\u043A\u0430, \u043F\u0435\u0440\u0435\u043A\u043B\u044E\u0447\u0456\u0442\u044C\u0441\u044F \u043D\u0430 \u043F\u0456\u0434\u0442\u0440\u0438\u043C\u0443\u0432\u0430\u043D\u0438\u0439 \u0431\u0440\u0430\u0443\u0437\u0435\u0440.
    \u0421\u043F\u0438\u0441\u043E\u043A \u043F\u0456\u0434\u0442\u0440\u0438\u043C\u0443\u0432\u0430\u043D\u0438\u0445 \u0431\u0440\u0430\u0443\u0437\u0435\u0440\u0456\u0432 \u043C\u043E\u0436\u043D\u0430 \u0437\u043D\u0430\u0439\u0442\u0438 \u0443 \u0412\u0456\u043A\u0456.
error-javascript-conflict = Ruffle \u0437\u0456\u0442\u043A\u043D\u0443\u0432\u0441\u044F \u0437 \u0441\u0435\u0440\u0439\u043E\u0437\u043D\u043E\u044E \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u043E\u044E \u043F\u0456\u0434 \u0447\u0430\u0441 \u0441\u043F\u0440\u043E\u0431\u0438 \u0456\u043D\u0456\u0446\u0456\u0430\u043B\u0456\u0437\u0430\u0446\u0456\u0457. \u0421\u0445\u043E\u0436\u0435, \u0449\u043E \u0446\u044F \u0441\u0442\u043E\u0440\u0456\u043D\u043A\u0430 \u0432\u0438\u043A\u043E\u0440\u0438\u0441\u0442\u043E\u0432\u0443\u0454 \u043A\u043E\u0434 JavaScript, \u044F\u043A\u0438\u0439 \u043A\u043E\u043D\u0444\u043B\u0456\u043A\u0442\u0443\u0454 \u0437 Ruffle. \u042F\u043A\u0449\u043E \u0432\u0438 \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u043C\u0438 \u0437\u0430\u043F\u0440\u043E\u0448\u0443\u0454\u043C\u043E \u0432\u0430\u0441 \u0441\u043F\u0440\u043E\u0431\u0443\u0432\u0430\u0442\u0438 \u0437\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0438\u0442\u0438 \u0444\u0430\u0439\u043B \u043D\u0430 \u043F\u043E\u0440\u043E\u0436\u043D\u0456\u0439 \u0441\u0442\u043E\u0440\u0456\u043D\u0446\u0456.
error-javascript-conflict-outdated = \u0412\u0438 \u0442\u0430\u043A\u043E\u0436 \u043C\u043E\u0436\u0435\u0442\u0435 \u0441\u043F\u0440\u043E\u0431\u0443\u0432\u0430\u0442\u0438 \u0437\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0438\u0442\u0438 \u043D\u043E\u0432\u0456\u0448\u0443 \u0432\u0435\u0440\u0441\u0456\u044E Ruffle, \u044F\u043A\u0430 \u043C\u043E\u0436\u0435 \u0443\u043D\u0438\u043A\u043D\u0443\u0442\u0438 \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u0438 (\u043F\u043E\u0442\u043E\u0447\u043D\u0430 \u0437\u0431\u0456\u0440\u043A\u0430 \u0437\u0430\u0441\u0442\u0430\u0440\u0456\u043B\u0430: { $buildDate }).
error-csp-conflict = Ruffle \u0437\u0456\u0442\u043A\u043D\u0443\u0432\u0441\u044F \u0437 \u0441\u0435\u0440\u0439\u043E\u0437\u043D\u043E\u044E \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u043E\u044E \u043F\u0456\u0434 \u0447\u0430\u0441 \u0441\u043F\u0440\u043E\u0431\u0438 \u0456\u043D\u0456\u0446\u0456\u0430\u043B\u0456\u0437\u0430\u0446\u0456\u0457. \u041F\u043E\u043B\u0456\u0442\u0438\u043A\u0430 \u0431\u0435\u0437\u043F\u0435\u043A\u0438 \u043A\u043E\u043D\u0442\u0435\u043D\u0442\u0443 \u0446\u044C\u043E\u0433\u043E \u0432\u0435\u0431\u0441\u0435\u0440\u0432\u0435\u0440\u0430 \u043D\u0435 \u0434\u043E\u0437\u0432\u043E\u043B\u044F\u0454 \u0437\u0430\u043F\u0443\u0441\u043A\u0430\u0442\u0438 \u043D\u0435\u043E\u0431\u0445\u0456\u0434\u043D\u0438\u0439 \u043A\u043E\u043C\u043F\u043E\u043D\u0435\u043D\u0442 ".wasm". \u042F\u043A\u0449\u043E \u0432\u0438 \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u0437\u0432\u0435\u0440\u043D\u0456\u0442\u044C\u0441\u044F \u0434\u043E Ruffle Wiki, \u0449\u043E\u0431 \u043E\u0442\u0440\u0438\u043C\u0430\u0442\u0438 \u0434\u043E\u043F\u043E\u043C\u043E\u0433\u0443.
error-url-invalid =
    Ruffle \u043D\u0435 \u0432\u0434\u0430\u043B\u043E\u0441\u044F \u0437\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0438\u0442\u0438 Flash SWF-\u0444\u0430\u0439\u043B.
    \u041D\u0430\u0439\u0456\u043C\u043E\u0432\u0456\u0440\u043D\u0456\u0448\u0435, \u0434\u043E Ruffle \u0431\u0443\u043B\u043E \u043F\u0435\u0440\u0435\u0434\u0430\u043D\u043E \u043D\u0435\u043F\u0440\u0430\u0432\u0438\u043B\u044C\u043D\u0438\u0439 URL SWF-\u0444\u0430\u0439\u043B\u0443.
error-unknown =
    Ruffle \u0437\u0456\u0442\u043A\u043D\u0443\u0432\u0441\u044F \u0437 \u0441\u0435\u0440\u0439\u043E\u0437\u043D\u043E\u044E \u043F\u0440\u043E\u0431\u043B\u0435\u043C\u043E\u044E \u043F\u0456\u0434 \u0447\u0430\u0441 \u0441\u043F\u0440\u043E\u0431\u0438 \u0432\u0456\u0434\u043E\u0431\u0440\u0430\u0437\u0438\u0442\u0438 \u0446\u0435\u0439 Flash \u043A\u043E\u043D\u0442\u0435\u043D\u0442.
    { $outdated ->
        [true] \u042F\u043A\u0449\u043E \u0432\u0438 \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440 \u0441\u0435\u0440\u0432\u0435\u0440\u0430, \u0441\u043F\u0440\u043E\u0431\u0443\u0439\u0442\u0435 \u0437\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0438\u0442\u0438 \u043D\u043E\u0432\u0456\u0448\u0443 \u0432\u0435\u0440\u0441\u0456\u044E Ruffle (\u043F\u043E\u0442\u043E\u0447\u043D\u0430 \u0437\u0431\u0456\u0440\u043A\u0430 \u0437\u0430\u0441\u0442\u0430\u0440\u0456\u043B\u0430: { $buildDate }).
       *[false] \u0426\u044C\u043E\u0433\u043E \u043D\u0435 \u043F\u043E\u0432\u0438\u043D\u043D\u043E \u0432\u0456\u0434\u0431\u0443\u0432\u0430\u0442\u0438\u0441\u044F, \u0442\u043E\u043C\u0443 \u043C\u0438 \u0431\u0443\u0434\u0435\u043C\u043E \u0434\u0443\u0436\u0435 \u0432\u0434\u044F\u0447\u043D\u0456, \u044F\u043A\u0449\u043E \u0432\u0438 \u043F\u043E\u0432\u0456\u0434\u043E\u043C\u0438\u0442\u0435 \u043F\u0440\u043E \u043F\u043E\u043C\u0438\u043B\u043A\u0443!
    }
`,"save-manager.ftl":`save-delete-prompt = \u0412\u0438 \u0432\u043F\u0435\u0432\u043D\u0435\u043D\u0456, \u0449\u043E \u0445\u043E\u0447\u0435\u0442\u0435 \u0432\u0438\u0434\u0430\u043B\u0438\u0442\u0438 \u0446\u0435\u0439 \u0444\u0430\u0439\u043B \u0437\u0431\u0435\u0440\u0435\u0436\u0435\u043D\u043D\u044F?
save-reload-prompt =
    \u0404\u0434\u0438\u043D\u0438\u0439 \u0441\u043F\u043E\u0441\u0456\u0431 { $action ->
        [delete] \u0432\u0438\u0434\u0430\u043B\u0438\u0442\u0438
       *[replace] \u0437\u0430\u043C\u0456\u043D\u0438\u0442\u0438
    } \u0446\u0435\u0439 \u0444\u0430\u0439\u043B \u0437\u0431\u0435\u0440\u0435\u0436\u0435\u043D\u043D\u044F \u0431\u0435\u0437 \u043F\u043E\u0442\u0435\u043D\u0446\u0456\u0439\u043D\u043E\u0433\u043E \u043A\u043E\u043D\u0444\u043B\u0456\u043A\u0442\u0443 \u0454 \u043F\u0435\u0440\u0435\u0437\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0435\u043D\u043D\u044F \u0446\u044C\u043E\u0433\u043E \u043A\u043E\u043D\u0442\u0435\u043D\u0442\u0443. \u0412\u0438 \u0432\u0441\u0435 \u043E\u0434\u043D\u043E \u0431\u0430\u0436\u0430\u0454\u0442\u0435 \u043F\u0440\u043E\u0434\u043E\u0432\u0436\u0438\u0442\u0438?
save-download = \u0417\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0438\u0442\u0438
save-replace = \u0417\u0430\u043C\u0456\u043D\u0438\u0442\u0438
save-delete = \u0412\u0438\u0434\u0430\u043B\u0438\u0442\u0438
save-backup-all = \u0417\u0430\u0432\u0430\u043D\u0442\u0430\u0436\u0438\u0442\u0438 \u0432\u0441\u0456 \u0444\u0430\u0439\u043B\u0438 \u0437\u0431\u0435\u0440\u0435\u0436\u0435\u043D\u043D\u044F
`,"volume-controls.ftl":`volume-controls-mute = \u0412\u0438\u043C\u043A\u043D\u0443\u0442\u0438 \u0437\u0432\u0443\u043A
volume-controls-unmute = \u0423\u0432\u0456\u043C\u043A\u043D\u0443\u0442\u0438 \u0437\u0432\u0443\u043A
`},"vi-VN":{"context_menu.ftl":`context-menu-download-swf = T\u1EA3i v\u1EC1 file SWF
context-menu-copy-debug-info = Sao ch\xE9p th\xF4ng tin g\u1EE1 l\u1ED7i
context-menu-open-save-manager = M\u1EDF tr\xECnh qu\u1EA3n l\xFD l\u01B0u file
context-menu-about-ruffle =
    { $flavor ->
        [extension] Gi\u1EDBi thi\u1EC7u v\u1EC1 ph\u1EA7n m\u1EDF r\u1ED9ng Ruffle ({ $version })
       *[other] Gi\u1EDBi thi\u1EC7u v\u1EC1 Ruffle ({ $version })
    }
context-menu-hide = \u1EA8n menu n\xE0y
context-menu-exit-fullscreen = Tho\xE1t ch\u1EBF \u0111\u1ED9 to\xE0n m\xE0n h\xECnh
context-menu-enter-fullscreen = Chuy\u1EC3n sang ch\u1EBF \u0111\u1ED9 to\xE0n m\xE0n h\xECnh
context-menu-volume-controls = Tu\u1EF3 ch\u1EC9nh \xE2m l\u01B0\u1EE3ng
`,"messages.ftl":`message-cant-embed =
    Ruffle kh\xF4ng th\u1EC3 ch\u1EA1y n\u1ED9i dung Flash \u0111\u01B0\u1EE3c nh\xFAng trong trang n\xE0y.
    B\u1EA1n c\xF3 th\u1EC3 th\u1EED m\u1EDF t\u1EC7p \u1EDF m\u1ED9t tab ri\xEAng bi\u1EC7t \u0111\u1EC3 tr\xE1nh s\u1EF1 c\u1ED1 n\xE0y.
message-restored-from-bfcache = Tr\xECnh duy\u1EC7t \u0111\xE3 kh\xF4i ph\u1EE5c l\u1EA1i n\u1ED9i dung Flash t\u1EEB phi\xEAn g\u1EA7n nh\u1EA5t. T\u1EA3i l\u1EA1i trang n\u1EBFu mu\u1ED1n b\u1EAFt \u0111\u1EA7u l\u1EA1i t\u1EEB \u0111\u1EA7u.
panic-title = C\xF3 l\u1ED7i x\u1EA3y ra :(
more-info = Th\xF4ng tin th\xEAm
run-anyway = V\u1EABn kh\u1EDFi ch\u1EA1y
continue = Ti\u1EBFp t\u1EE5c
report-bug = B\xE1o c\xE1o l\u1ED7i
update-ruffle = C\u1EADp nh\u1EADt Ruffle
ruffle-demo = Trang demo
ruffle-desktop = \u1EE8ng d\u1EE5ng desktop
ruffle-wiki = Truy c\u1EADp Ruffle Wiki
enable-hardware-acceleration = C\xF3 v\u1EBB nh\u01B0 t\u0103ng t\u1ED1c ph\u1EA7n c\u1EE9ng \u0111\xE3 b\u1ECB v\xF4 hi\u1EC7u ho\xE1. M\u1EB7c d\xF9 Ruffle v\u1EABn c\xF3 th\u1EC3 ho\u1EA1t \u0111\u1ED9ng, nh\u01B0ng n\xF3 c\xF3 th\u1EC3 r\u1EA5t ch\u1EADm. B\u1EA1n c\xF3 th\u1EC3 t\xECm c\xE1ch b\u1EADt t\u0103ng t\u1ED1c ph\u1EA7n c\u1EE9ng b\u1EB1ng c\xE1ch l\xE0m theo h\u01B0\u1EDBng d\u1EABn trong \u0111\u01B0\u1EDDng d\u1EABn b\xEAn d\u01B0\u1EDBi:
enable-hardware-acceleration-link = C\xE1c c\xE2u h\u1ECFi th\u01B0\u1EDDng g\u1EB7p - T\u0103ng t\u1ED1c ph\u1EA7n c\u1EE9ng cho Chrome
view-error-details = Xem chi ti\u1EBFt l\u1ED7i
open-in-new-tab = M\u1EDF trong th\u1EBB m\u1EDBi
click-to-unmute = B\u1EA5m \u0111\u1EC3 b\u1EADt ti\u1EBFng
clipboard-message-title = Sao ch\xE9p v\xE0 d\xE1n b\xEAn trong Ruffle
clipboard-message-description =
    { $variant ->
       *[unsupported] Tr\xECnh duy\u1EC7t c\u1EE7a b\u1EA1n kh\xF4ng h\u1ED7 tr\u1EE3 \u0111\u1EA7y \u0111\u1EE7 truy xu\u1EA5t b\u1ED9 nh\u1EDB t\u1EA1m,
        [access-denied] Truy xu\u1EA5t b\u1ED9 nh\u1EDB t\u1EA1m b\u1ECB t\u1EEB ch\u1ED1i,
    } nh\u01B0ng b\u1EA1n lu\xF4n c\xF3 th\u1EC3 s\u1EED d\u1EE5ng ph\xEDm t\u1EAFt \u0111\u1EC3 l\xE0m \u0111i\u1EC1u \u0111\xF3:
clipboard-message-copy = { " " } \u0111\u1EC3 sao ch\xE9p
clipboard-message-cut = { " " } \u0111\u1EC3 c\u1EAFt
clipboard-message-paste = { " " } \u0111\u1EC3 d\xE1n
error-canvas-reload = Tr\xECnh k\u1EBFt xu\u1EA5t \u0111\u1ED3 ho\u1EA1 canvas renderer \u0111ang \u0111\u01B0\u1EE3c s\u1EED d\u1EE5ng n\xEAn kh\xF4ng th\u1EC3 l\xE0m m\u1EDBi.
error-file-protocol =
    C\xF3 v\u1EBB nh\u01B0 b\u1EA1n \u0111ang ch\u1EA1y Ruffle tr\xEAn giao th\u1EE9c "file:".
    \u0110i\u1EC1u n\xE0y kh\xF4ng \u0111\u01B0\u1EE3c ph\xE9p v\xEC tr\xECnh duy\u1EC7t ch\u1EB7n nhi\u1EC1u t\xEDnh n\u0103ng ho\u1EA1t \u0111\u1ED9ng v\xEC l\xFD do b\u1EA3o m\u1EADt.
    Thay v\xE0o \u0111\xF3, ch\xFAng t\xF4i m\u1EDDi b\u1EA1n thi\u1EBFt l\u1EADp m\u1ED9t m\xE1y ch\u1EE7 c\u1EE5c b\u1ED9 ho\u1EB7c s\u1EED d\u1EE5ng trang demo ho\u1EB7c \u1EE9ng d\u1EE5ng desktop.
error-javascript-config =
    Ruffle \u0111\xE3 g\u1EB7p ph\u1EA3i s\u1EF1 c\u1ED1 l\u1EDBn do c\u1EA5u h\xECnh JavaScript kh\xF4ng ch\xEDnh x\xE1c.
    N\u1EBFu b\u1EA1n l\xE0 ng\u01B0\u1EDDi qu\u1EA3n tr\u1ECB m\xE1y ch\u1EE7, ch\xFAng t\xF4i m\u1EDDi b\u1EA1n ki\u1EC3m tra chi ti\u1EBFt l\u1ED7i \u0111\u1EC3 t\xECm ra tham s\u1ED1 n\xE0o kh\xF4ng \u0111\xFAng.
    B\u1EA1n c\u0169ng c\xF3 th\u1EC3 tham kh\u1EA3o th\xF4ng tin tr\u1EE3 gi\xFAp t\u1EEB Ruffle Wiki.
error-wasm-not-found =
    Ruffle kh\xF4ng t\u1EA3i \u0111\u01B0\u1EE3c t\u1EC7p ".wasm" c\u1EA7n thi\u1EBFt.
    N\u1EBFu b\u1EA1n l\xE0 ng\u01B0\u1EDDi qu\u1EA3n tr\u1ECB m\xE1y ch\u1EE7, vui l\xF2ng \u0111\u1EA3m b\u1EA3o t\u1EC7p \u0111\xE3 \u0111\u01B0\u1EE3c t\u1EA3i l\xEAn \u0111\xFAng c\xE1ch.
    N\u1EBFu s\u1EF1 c\u1ED1 v\u1EABn ti\u1EBFp di\u1EC5n, b\u1EA1n c\xF3 th\u1EC3 c\u1EA7n ph\u1EA3i s\u1EED d\u1EE5ng thi\u1EBFt l\u1EADp "publicPath": vui l\xF2ng tham kh\u1EA3o th\xF4ng tin tr\u1EE3 gi\xFAp t\u1EEB Ruffle Wiki.
error-wasm-mime-type =
    Ruffle \u0111\xE3 g\u1EB7p ph\u1EA3i m\u1ED9t v\u1EA5n \u0111\u1EC1 l\u1EDBn khi c\u1ED1 g\u1EAFng kh\u1EDFi t\u1EA1o.
    M\xE1y ch\u1EE7 web kh\xF4ng cung c\u1EA5p t\u1EC7p ".wasm" v\u1EDBi \u0111\xFAng lo\u1EA1i MIME.
    N\u1EBFu b\u1EA1n l\xE0 qu\u1EA3n tr\u1ECB vi\xEAn m\xE1y ch\u1EE7, vui l\xF2ng tham kh\u1EA3o wiki Ruffle \u0111\u1EC3 \u0111\u01B0\u1EE3c tr\u1EE3 gi\xFAp.
error-invalid-swf =
    Ruffle kh\xF4ng th\u1EC3 ph\xE2n t\xEDch t\u1EC7p \u0111\u01B0\u1EE3c y\xEAu c\u1EA7u.
    Kh\u1EA3 n\u0103ng l\u1EDBn nh\u1EA5t l\xE0 do t\u1EC7p \u0111\u01B0\u1EE3c y\xEAu c\u1EA7u kh\xF4ng ph\u1EA3i l\xE0 m\u1ED9t t\u1EC7p SWF h\u1EE3p l\u1EC7.
error-swf-fetch =
    Ruffle kh\xF4ng t\u1EA3i \u0111\u01B0\u1EE3c t\u1EC7p Flash SWF.
    Kh\u1EA3 n\u0103ng l\u1EDBn nh\u1EA5t l\xE0 do t\u1EC7p kh\xF4ng c\xF2n t\u1ED3n t\u1EA1i n\u1EEFa, v\xEC v\u1EADy kh\xF4ng c\xF3 g\xEC \u0111\u1EC3 Ruffle t\u1EA3i.
    H\xE3y th\u1EED li\xEAn h\u1EC7 v\u1EDBi qu\u1EA3n tr\u1ECB vi\xEAn trang web \u0111\u1EC3 \u0111\u01B0\u1EE3c tr\u1EE3 gi\xFAp.
error-swf-cors =
    Ruffle kh\xF4ng t\u1EA3i \u0111\u01B0\u1EE3c t\u1EC7p Flash SWF.
    Quy\u1EC1n truy c\u1EADp \u0111\u1EC3 l\u1EA5y d\u1EEF li\u1EC7u c\xF3 th\u1EC3 \u0111\xE3 b\u1ECB ch\xEDnh s\xE1ch CORS ch\u1EB7n.
    N\u1EBFu b\u1EA1n l\xE0 qu\u1EA3n tr\u1ECB vi\xEAn m\xE1y ch\u1EE7, vui l\xF2ng tham kh\u1EA3o Ruffle Wiki \u0111\u1EC3 \u0111\u01B0\u1EE3c tr\u1EE3 gi\xFAp.
error-wasm-cors =
    Ruffle kh\xF4ng t\u1EA3i \u0111\u01B0\u1EE3c t\u1EC7p ".wasm" c\u1EA7n thi\u1EBFt.
    Quy\u1EC1n truy c\u1EADp \u0111\u1EC3 l\u1EA5y d\u1EEF li\u1EC7u c\xF3 th\u1EC3 \u0111\xE3 b\u1ECB ch\xEDnh s\xE1ch CORS ch\u1EB7n.
    N\u1EBFu b\u1EA1n l\xE0 qu\u1EA3n tr\u1ECB vi\xEAn m\xE1y ch\u1EE7, vui l\xF2ng tham kh\u1EA3o wiki Ruffle \u0111\u1EC3 \u0111\u01B0\u1EE3c tr\u1EE3 gi\xFAp.
error-wasm-invalid =
    Ruffle \u0111\xE3 g\u1EB7p ph\u1EA3i m\u1ED9t v\u1EA5n \u0111\u1EC1 l\u1EDBn khi c\u1ED1 g\u1EAFng kh\u1EDFi t\u1EA1o.
    C\xF3 v\u1EBB nh\u01B0 trang n\xE0y c\xF3 c\xE1c t\u1EC7p b\u1ECB thi\u1EBFu ho\u1EB7c kh\xF4ng h\u1EE3p l\u1EC7 \u0111\u1EC3 ch\u1EA1y Ruffle.
    N\u1EBFu b\u1EA1n l\xE0 qu\u1EA3n tr\u1ECB vi\xEAn m\xE1y ch\u1EE7, vui l\xF2ng tham kh\u1EA3o Ruffle Wiki \u0111\u1EC3 \u0111\u01B0\u1EE3c tr\u1EE3 gi\xFAp.
error-wasm-download =
    Ruffle \u0111\xE3 g\u1EB7p ph\u1EA3i m\u1ED9t v\u1EA5n \u0111\u1EC1 l\u1EDBn khi c\u1ED1 g\u1EAFng kh\u1EDFi t\u1EA1o.
    V\u1EA5n \u0111\u1EC1 n\xE0y th\u01B0\u1EDDng c\xF3 th\u1EC3 t\u1EF1 gi\u1EA3i quy\u1EBFt, v\xEC v\u1EADy b\u1EA1n c\xF3 th\u1EC3 th\u1EED t\u1EA3i l\u1EA1i trang.
    N\u1EBFu kh\xF4ng, vui l\xF2ng li\xEAn h\u1EC7 v\u1EDBi qu\u1EA3n tr\u1ECB vi\xEAn trang web.
error-wasm-disabled-on-edge =
    Ruffle kh\xF4ng t\u1EA3i \u0111\u01B0\u1EE3c th\xE0nh ph\u1EA7n t\u1EC7p ".wasm" c\u1EA7n thi\u1EBFt.
    \u0110\u1EC3 kh\u1EAFc ph\u1EE5c s\u1EF1 c\u1ED1 n\xE0y, h\xE3y th\u1EED m\u1EDF c\xE0i \u0111\u1EB7t c\u1EE7a tr\xECnh duy\u1EC7t, nh\u1EA5p v\xE0o "Quy\u1EC1n ri\xEAng t\u01B0, t\xECm ki\u1EBFm v\xE0 d\u1ECBch v\u1EE5", cu\u1ED9n xu\u1ED1ng v\xE0 t\u1EAFt "N\xE2ng cao b\u1EA3o m\u1EADt tr\xEAn web".
    Thao t\xE1c n\xE0y s\u1EBD cho ph\xE9p tr\xECnh duy\u1EC7t c\u1EE7a b\u1EA1n t\u1EA3i c\xE1c t\u1EC7p ".wasm" c\u1EA7n thi\u1EBFt.
    N\u1EBFu s\u1EF1 c\u1ED1 v\u1EABn ti\u1EBFp di\u1EC5n, b\u1EA1n c\xF3 th\u1EC3 ph\u1EA3i s\u1EED d\u1EE5ng tr\xECnh duy\u1EC7t kh\xE1c.
error-wasm-unsupported-browser =
    Tr\xECnh duy\u1EC7t b\u1EA1n \u0111ang s\u1EED d\u1EE5ng kh\xF4ng h\u1ED7 tr\u1EE3 ti\u1EC7n \xEDch m\u1EDF r\u1ED9ng WebAssembly c\u1EA7n thi\u1EBFt \u0111\u1EC3 ch\u1EA1y Ruffle.
    Vui l\xF2ng chuy\u1EC3n sang tr\xECnh duy\u1EC7t \u0111\u01B0\u1EE3c h\u1ED7 tr\u1EE3.
    B\u1EA1n c\xF3 th\u1EC3 xem danh s\xE1ch c\xE1c tr\xECnh duy\u1EC7t \u0111\u01B0\u1EE3c h\u1ED7 tr\u1EE3 tr\xEAn Ruffle Wiki.
error-javascript-conflict =
    Ruffle g\u1EB7p ph\u1EA3i m\u1ED9t v\u1EA5n \u0111\u1EC1 l\u1EDBn khi c\u1ED1 g\u1EAFng kh\u1EDFi t\u1EA1o.
    C\xF3 v\u1EBB trang n\xE0y s\u1EED d\u1EE5ng m\xE3 JavaScript xung \u0111\u1ED9t v\u1EDBi Ruffle.
    N\u1EBFu b\u1EA1n l\xE0 qu\u1EA3n tr\u1ECB vi\xEAn m\xE1y ch\u1EE7, ch\xFAng t\xF4i m\u1EDDi b\u1EA1n th\u1EED t\u1EA3i t\u1EC7p tr\xEAn m\u1ED9t trang tr\u1EAFng.
error-javascript-conflict-outdated = B\u1EA1n c\u0169ng c\xF3 th\u1EC3 th\u1EED t\u1EA3i l\xEAn phi\xEAn b\u1EA3n Ruffle m\u1EDBi h\u01A1n \u0111\u1EC3 xem s\u1EF1 c\u1ED1 c\xF3 th\u1EC3 \u0111\u01B0\u1EE3c kh\u1EAFc ph\u1EE5c (b\u1EA3n d\u1EF1ng hi\u1EC7n t\u1EA1i \u0111\xE3 c\u0169: { $buildDate }).
error-csp-conflict =
    Ruffle \u0111\xE3 g\u1EB7p ph\u1EA3i m\u1ED9t v\u1EA5n \u0111\u1EC1 l\u1EDBn khi c\u1ED1 g\u1EAFng kh\u1EDFi t\u1EA1o.
    Ch\xEDnh s\xE1ch b\u1EA3o m\u1EADt n\u1ED9i dung c\u1EE7a m\xE1y ch\u1EE7 web n\xE0y kh\xF4ng cho ph\xE9p ch\u1EA1y th\xE0nh ph\u1EA7n t\u1EC7p ".wasm" b\u1EAFt bu\u1ED9c ph\u1EA3i c\xF3 \u0111\u1EC3 ho\u1EA1t \u0111\u1ED9ng.
    N\u1EBFu b\u1EA1n l\xE0 qu\u1EA3n tr\u1ECB vi\xEAn m\xE1y ch\u1EE7, vui l\xF2ng tham kh\u1EA3o Ruffle Wiki \u0111\u1EC3 \u0111\u01B0\u1EE3c tr\u1EE3 gi\xFAp.
error-url-invalid =
    Ruffle kh\xF4ng t\u1EA3i \u0111\u01B0\u1EE3c t\u1EC7p Flash SWF.
    Nhi\u1EC1u kh\u1EA3 n\u0103ng l\xE0 do URL c\u1EE7a t\u1EC7p SWF truy\u1EC1n cho Ruffle kh\xF4ng h\u1EE3p l\u1EC7.
error-unknown =
    Ruffle \u0111\xE3 g\u1EB7p ph\u1EA3i m\u1ED9t v\u1EA5n \u0111\u1EC1 l\u1EDBn khi c\u1ED1 g\u1EAFng hi\u1EC3n th\u1ECB n\u1ED9i dung Flash n\xE0y.
    { $outdated ->
        [true] N\u1EBFu b\u1EA1n l\xE0 qu\u1EA3n tr\u1ECB vi\xEAn m\xE1y ch\u1EE7, vui l\xF2ng th\u1EED t\u1EA3i l\xEAn phi\xEAn b\u1EA3n Ruffle m\u1EDBi h\u01A1n (b\u1EA3n d\u1EF1ng hi\u1EC7n t\u1EA1i \u0111\xE3 c\u0169: { $buildDate }).
       *[false] V\u1EA5n \u0111\u1EC1 n\xE0y \u0111\xE1ng l\u1EBD kh\xF4ng n\xEAn x\u1EA3y ra, v\xEC v\u1EADy ch\xFAng t\xF4i th\u1EF1c s\u1EF1 bi\u1EBFt \u01A1n n\u1EBFu b\u1EA1n c\xF3 th\u1EC3 b\xE1o c\xE1o l\u1ED7i!
    }
`,"save-manager.ftl":`save-delete-prompt = B\u1EA1n c\xF3 ch\u1EAFc ch\u1EAFn mu\u1ED1n xo\xE1 t\u1EC7p \u0111\xE3 l\u01B0u n\xE0y kh\xF4ng?
save-reload-prompt =
    C\xE1ch duy nh\u1EA5t \u0111\u1EC3 { $action ->
        [delete] xo\xE1
       *[replace] thay th\u1EBF
    } t\u1EC7p \u0111\xE3 l\u01B0u n\xE0y m\xE0 kh\xF4ng c\xF3 nguy c\u01A1 xung \u0111\u1ED9t l\xE0 t\u1EA3i l\u1EA1i n\u1ED9i dung n\xE0y. B\u1EA1n c\xF3 mu\u1ED1n ti\u1EBFp t\u1EE5c kh\xF4ng?
save-download = T\u1EA3i v\u1EC1
save-replace = Thay th\u1EBF
save-delete = Xo\xE1
save-backup-all = T\u1EA3i xu\u1ED1ng t\u1EA5t c\u1EA3 t\u1EC7p \u0111\xE3 l\u01B0u
`,"volume-controls.ftl":`volume-controls-mute = T\u1EAFt ti\u1EBFng
volume-controls-unmute = B\u1EADt ti\u1EBFng
`},"zh-CN":{"context_menu.ftl":`context-menu-download-swf = \u4E0B\u8F7D SWF
context-menu-copy-debug-info = \u590D\u5236\u8C03\u8BD5\u4FE1\u606F
context-menu-open-save-manager = \u6253\u5F00\u5B58\u6863\u7BA1\u7406\u5668
context-menu-about-ruffle =
    { $flavor ->
        [extension] \u5173\u4E8E Ruffle \u6269\u5C55 ({ $version })
       *[other] \u5173\u4E8E Ruffle ({ $version })
    }
context-menu-hide = \u9690\u85CF\u6B64\u83DC\u5355
context-menu-exit-fullscreen = \u9000\u51FA\u5168\u5C4F
context-menu-enter-fullscreen = \u8FDB\u5165\u5168\u5C4F
context-menu-volume-controls = \u97F3\u91CF\u63A7\u5236
`,"messages.ftl":`message-cant-embed =
    Ruffle \u65E0\u6CD5\u8FD0\u884C\u5D4C\u5165\u5728\u6B64\u9875\u9762\u4E2D\u7684 Flash\u3002
    \u60A8\u53EF\u4EE5\u5C1D\u8BD5\u5728\u5355\u72EC\u7684\u6807\u7B7E\u9875\u4E2D\u6253\u5F00\u8BE5\u6587\u4EF6\uFF0C\u4EE5\u56DE\u907F\u6B64\u95EE\u9898\u3002
message-restored-from-bfcache =
    \u60A8\u7684\u6D4F\u89C8\u5668\u4ECE\u4E4B\u524D\u7684\u4F1A\u8BDD\u4E2D\u6062\u590D\u4E86\u8FD9\u4E2AFlash\u5185\u5BB9\u3002
    \u82E5\u8981\u4ECE\u5934\u5F00\u59CB\u64AD\u653E\uFF0C\u8BF7\u91CD\u65B0\u52A0\u8F7D\u9875\u9762\u3002
panic-title = \u51FA\u4E86\u4E9B\u95EE\u9898 :(
more-info = \u66F4\u591A\u4FE1\u606F
run-anyway = \u4ECD\u7136\u8FD0\u884C
continue = \u7EE7\u7EED
report-bug = \u53CD\u9988\u95EE\u9898
update-ruffle = \u66F4\u65B0 Ruffle
ruffle-demo = \u7F51\u9875\u6F14\u793A
ruffle-desktop = \u684C\u9762\u5E94\u7528\u7A0B\u5E8F
ruffle-wiki = \u67E5\u770B Ruffle Wiki
enable-hardware-acceleration = \u770B\u8D77\u6765\u786C\u4EF6\u52A0\u901F\u5DF2\u88AB\u7981\u7528\u3002\u867D\u7136Ruffle\u53EF\u80FD\u53EF\u4EE5\u8FD0\u884C\uFF0C\u4F46\u901F\u5EA6\u53EF\u80FD\u4F1A\u975E\u5E38\u6162\u3002\u60A8\u53EF\u4EE5\u901A\u8FC7\u4E0B\u9762\u7684\u94FE\u63A5\u4E86\u89E3\u5982\u4F55\u542F\u7528\u786C\u4EF6\u52A0\u901F\uFF1A
enable-hardware-acceleration-link = \u5E38\u89C1\u95EE\u9898 - Chrome \u786C\u4EF6\u52A0\u901F
view-error-details = \u67E5\u770B\u9519\u8BEF\u8BE6\u60C5
open-in-new-tab = \u5728\u65B0\u6807\u7B7E\u9875\u4E2D\u6253\u5F00
click-to-unmute = \u70B9\u51FB\u53D6\u6D88\u9759\u97F3
clipboard-message-title = \u5728Ruffle\u4E2D\u590D\u5236\u7C98\u8D34
clipboard-message-description =
    { $variant ->
       *[unsupported] \u60A8\u7684\u6D4F\u89C8\u5668\u4E0D\u652F\u6301\u5B8C\u5168\u526A\u8D34\u677F\u8BBF\u95EE,
        [access-denied] \u5BF9\u526A\u8D34\u677F\u7684\u8BBF\u95EE\u5DF2\u88AB\u62D2\u7EDD,
    } \u4F46\u60A8\u4ECD\u7136\u53EF\u4EE5\u4F7F\u7528\u4EE5\u4E0B\u5FEB\u6377\u952E:
clipboard-message-copy = { " " } \u590D\u5236
clipboard-message-cut = { " " } \u526A\u5207
clipboard-message-paste = { " " } \u7C98\u8D34
error-canvas-reload = Canvas \u6E32\u67D3\u5668\u5DF2\u5728\u4F7F\u7528\u4E2D\u65F6\uFF0C\u65E0\u6CD5\u4F7F\u7528 Canvas \u6E32\u67D3\u5668\u91CD\u65B0\u52A0\u8F7D\u3002
error-file-protocol =
    \u770B\u6765\u60A8\u6B63\u5728 "file:" \u534F\u8BAE\u4E0A\u4F7F\u7528 Ruffle\u3002
    \u7531\u4E8E\u6D4F\u89C8\u5668\u4EE5\u5B89\u5168\u539F\u56E0\u963B\u6B62\u8BB8\u591A\u529F\u80FD\uFF0C\u56E0\u6B64\u8FD9\u4E0D\u8D77\u4F5C\u7528\u3002
    \u76F8\u53CD\u6211\u4EEC\u9080\u8BF7\u60A8\u8BBE\u7F6E\u672C\u5730\u670D\u52A1\u5668\u6216\u4F7F\u7528\u7F51\u9875\u6F14\u793A\u6216\u684C\u9762\u5E94\u7528\u7A0B\u5E8F\u3002
error-javascript-config =
    \u7531\u4E8E\u9519\u8BEF\u7684 JavaScript \u914D\u7F6E\uFF0CRuffle \u9047\u5230\u4E86\u4E00\u4E2A\u91CD\u5927\u95EE\u9898\u3002
    \u5982\u679C\u60A8\u662F\u670D\u52A1\u5668\u7BA1\u7406\u5458\uFF0C\u6211\u4EEC\u9080\u8BF7\u60A8\u68C0\u67E5\u9519\u8BEF\u8BE6\u7EC6\u4FE1\u606F\uFF0C\u4EE5\u627E\u51FA\u54EA\u4E2A\u53C2\u6570\u6709\u6545\u969C\u3002
    \u60A8\u4E5F\u53EF\u4EE5\u67E5\u9605 Ruffle \u7684 Wiki \u83B7\u53D6\u5E2E\u52A9\u3002
error-wasm-not-found =
    Ruffle \u65E0\u6CD5\u52A0\u8F7D\u6240\u9700\u7684 \u201C.wasm\u201D \u6587\u4EF6\u7EC4\u4EF6\u3002
    \u5982\u679C\u60A8\u662F\u670D\u52A1\u5668\u7BA1\u7406\u5458\uFF0C\u8BF7\u786E\u4FDD\u6587\u4EF6\u5DF2\u6B63\u786E\u4E0A\u4F20\u3002
    \u5982\u679C\u95EE\u9898\u4ECD\u7136\u5B58\u5728\uFF0C\u60A8\u53EF\u80FD\u9700\u8981\u4F7F\u7528 \u201CpublicPath\u201D \u8BBE\u7F6E\uFF1A\u8BF7\u67E5\u770B Ruffle \u7684 Wiki \u83B7\u53D6\u5E2E\u52A9\u3002
error-wasm-mime-type =
    Ruffle \u5728\u8BD5\u56FE\u521D\u59CB\u5316\u65F6\u9047\u5230\u4E86\u4E00\u4E2A\u91CD\u5927\u95EE\u9898\u3002
    \u8BE5\u7F51\u7AD9\u670D\u52A1\u5668\u6CA1\u6709\u63D0\u4F9B ".asm\u201D \u6587\u4EF6\u6B63\u786E\u7684 MIME \u7C7B\u578B\u3002
    \u5982\u679C\u60A8\u662F\u670D\u52A1\u5668\u7BA1\u7406\u5458\uFF0C\u8BF7\u67E5\u9605 Ruffle Wiki \u83B7\u53D6\u5E2E\u52A9\u3002
error-invalid-swf =
    Ruffle\u65E0\u6CD5\u89E3\u6790\u8BF7\u6C42\u7684\u6587\u4EF6\u3002
    \u6700\u6709\u53EF\u80FD\u7684\u539F\u56E0\u662F\u8BE5\u8BF7\u6C42\u6587\u4EF6\u4E0D\u662F\u4E00\u4E2A\u5408\u6CD5\u7684SWF\u6587\u4EF6\u3002
error-swf-fetch =
    Ruffle \u65E0\u6CD5\u52A0\u8F7D Flash SWF \u6587\u4EF6\u3002
    \u6700\u53EF\u80FD\u7684\u539F\u56E0\u662F\u6587\u4EF6\u4E0D\u518D\u5B58\u5728\u6240\u4EE5 Ruffle \u6CA1\u6709\u8981\u52A0\u8F7D\u7684\u5185\u5BB9\u3002
    \u8BF7\u5C1D\u8BD5\u8054\u7CFB\u7F51\u7AD9\u7BA1\u7406\u5458\u5BFB\u6C42\u5E2E\u52A9\u3002
error-swf-cors =
    Ruffle \u65E0\u6CD5\u52A0\u8F7D Flash SWF \u6587\u4EF6\u3002
    \u83B7\u53D6\u6743\u9650\u53EF\u80FD\u88AB CORS \u7B56\u7565\u963B\u6B62\u3002
    \u5982\u679C\u60A8\u662F\u670D\u52A1\u5668\u7BA1\u7406\u5458\uFF0C\u8BF7\u53C2\u8003 Ruffle Wiki \u83B7\u53D6\u5E2E\u52A9\u3002
error-wasm-cors =
    Ruffle \u65E0\u6CD5\u52A0\u8F7D\u6240\u9700\u7684\u201C.wasm\u201D\u6587\u4EF6\u7EC4\u4EF6\u3002
    \u83B7\u53D6\u6743\u9650\u53EF\u80FD\u88AB CORS \u7B56\u7565\u963B\u6B62\u3002
    \u5982\u679C\u60A8\u662F\u670D\u52A1\u5668\u7BA1\u7406\u5458\uFF0C\u8BF7\u67E5\u9605 Ruffle Wiki \u83B7\u53D6\u5E2E\u52A9\u3002
error-wasm-invalid =
    Ruffle \u5728\u8BD5\u56FE\u521D\u59CB\u5316\u65F6\u9047\u5230\u4E86\u4E00\u4E2A\u91CD\u5927\u95EE\u9898\u3002
    \u8FD9\u4E2A\u9875\u9762\u4F3C\u4E4E\u7F3A\u5C11\u6587\u4EF6\u6765\u8FD0\u884C Curl\u3002
    \u5982\u679C\u60A8\u662F\u670D\u52A1\u5668\u7BA1\u7406\u5458\uFF0C\u8BF7\u67E5\u9605 Ruffle Wiki \u83B7\u53D6\u5E2E\u52A9\u3002
error-wasm-download =
    Ruffle \u5728\u8BD5\u56FE\u521D\u59CB\u5316\u65F6\u9047\u5230\u4E86\u4E00\u4E2A\u91CD\u5927\u95EE\u9898\u3002
    \u8FD9\u901A\u5E38\u53EF\u4EE5\u81EA\u884C\u89E3\u51B3\uFF0C\u56E0\u6B64\u60A8\u53EF\u4EE5\u5C1D\u8BD5\u91CD\u65B0\u52A0\u8F7D\u9875\u9762\u3002
    \u5426\u5219\u8BF7\u8054\u7CFB\u7F51\u7AD9\u7BA1\u7406\u5458\u3002
error-wasm-disabled-on-edge =
    Ruffle \u65E0\u6CD5\u52A0\u8F7D\u6240\u9700\u7684 \u201C.wasm\u201D \u6587\u4EF6\u7EC4\u4EF6\u3002
    \u8981\u89E3\u51B3\u8FD9\u4E2A\u95EE\u9898\uFF0C\u8BF7\u5C1D\u8BD5\u6253\u5F00\u60A8\u7684\u6D4F\u89C8\u5668\u8BBE\u7F6E\uFF0C\u5355\u51FB"\u9690\u79C1\u3001\u641C\u7D22\u548C\u670D\u52A1"\uFF0C\u5411\u4E0B\u6EDA\u52A8\u5E76\u5173\u95ED"\u589E\u5F3A Web \u5B89\u5168\u6027"\u3002
    \u8FD9\u5C06\u5141\u8BB8\u60A8\u7684\u6D4F\u89C8\u5668\u52A0\u8F7D\u6240\u9700\u7684 \u201C.wasm\u201D \u6587\u4EF6\u3002
    \u5982\u679C\u95EE\u9898\u4ECD\u7136\u5B58\u5728\uFF0C\u60A8\u53EF\u80FD\u5FC5\u987B\u4F7F\u7528\u4E0D\u540C\u7684\u6D4F\u89C8\u5668\u3002
error-wasm-unsupported-browser =
    \u60A8\u4F7F\u7528\u7684\u6D4F\u89C8\u5668\u4E0D\u652F\u6301 Ruffle \u8FD0\u884C\u6240\u9700\u7684 WebAssembly \u6269\u5C55\u3002
    \u8BF7\u5207\u6362\u5230\u652F\u6301\u7684\u6D4F\u89C8\u5668\u3002
    \u60A8\u53EF\u4EE5\u5728 Wiki \u4E0A\u627E\u5230\u652F\u6301\u7684\u6D4F\u89C8\u5668\u5217\u8868\u3002
error-javascript-conflict =
    Ruffle \u5728\u8BD5\u56FE\u521D\u59CB\u5316\u65F6\u9047\u5230\u4E86\u4E00\u4E2A\u91CD\u5927\u95EE\u9898\u3002
    \u8FD9\u4E2A\u9875\u9762\u4F3C\u4E4E\u4F7F\u7528\u4E86\u4E0E Ruffle \u51B2\u7A81\u7684 JavaScript \u4EE3\u7801\u3002
    \u5982\u679C\u60A8\u662F\u670D\u52A1\u5668\u7BA1\u7406\u5458\uFF0C\u6211\u4EEC\u5EFA\u8BAE\u60A8\u5C1D\u8BD5\u5728\u7A7A\u767D\u9875\u9762\u4E0A\u52A0\u8F7D\u6587\u4EF6\u3002
error-javascript-conflict-outdated = \u60A8\u4E5F\u53EF\u4EE5\u5C1D\u8BD5\u4E0A\u4F20\u53EF\u80FD\u89C4\u907F\u6B64\u95EE\u9898\u7684\u8F83\u65B0\u7248\u672C\u7684 Ruffle (\u5F53\u524D\u6784\u5EFA\u7248\u672C\u5DF2\u8FC7\u65F6: { $buildDate })\u3002
error-csp-conflict =
    Ruffle \u5728\u8BD5\u56FE\u521D\u59CB\u5316\u65F6\u9047\u5230\u4E86\u4E00\u4E2A\u91CD\u5927\u95EE\u9898\u3002
    \u8BE5\u7F51\u7AD9\u670D\u52A1\u5668\u7684\u5185\u5BB9\u5B89\u5168\u7B56\u7565\u4E0D\u5141\u8BB8\u8FD0\u884C\u6240\u9700\u7684 \u201C.wasm\u201D \u7EC4\u4EF6\u3002
    \u5982\u679C\u60A8\u662F\u670D\u52A1\u5668\u7BA1\u7406\u5458\uFF0C\u8BF7\u67E5\u9605 Ruffle Wiki \u83B7\u53D6\u5E2E\u52A9\u3002
error-url-invalid =
    Ruffle \u65E0\u6CD5\u52A0\u8F7D Flash SWF \u6587\u4EF6\u3002
    \u6700\u6709\u53EF\u80FD\u7684\u539F\u56E0\u662F\u4F20\u9012\u7ED9 Ruffle \u7684 SWF \u6587\u4EF6 URL \u65E0\u6548\u3002
error-unknown =
    Ruffle \u5728\u8BD5\u56FE\u663E\u793A\u6B64 Flash \u5185\u5BB9\u65F6\u9047\u5230\u4E86\u4E00\u4E2A\u91CD\u5927\u95EE\u9898\u3002
    { $outdated ->
        [true] \u5982\u679C\u60A8\u662F\u670D\u52A1\u5668\u7BA1\u7406\u5458\uFF0C\u8BF7\u5C1D\u8BD5\u4E0A\u4F20\u66F4\u65B0\u7684 Ruffle \u7248\u672C (\u5F53\u524D\u7248\u672C\u5DF2\u8FC7\u65F6: { $buildDate }).
       *[false] \u8FD9\u4E0D\u5E94\u8BE5\u53D1\u751F\uFF0C\u56E0\u6B64\u5982\u679C\u60A8\u53EF\u4EE5\u62A5\u544A\u9519\u8BEF\uFF0C\u6211\u4EEC\u5C06\u975E\u5E38\u611F\u8C22\uFF01
    }
`,"save-manager.ftl":`save-delete-prompt = \u786E\u5B9A\u8981\u5220\u9664\u6B64\u5B58\u6863\u5417\uFF1F
save-reload-prompt =
    \u4E3A\u4E86\u907F\u514D\u6F5C\u5728\u7684\u51B2\u7A81\uFF0C{ $action ->
        [delete] \u5220\u9664
       *[replace] \u66FF\u6362
    } \u6B64\u5B58\u6863\u6587\u4EF6\u9700\u8981\u91CD\u65B0\u52A0\u8F7D\u5F53\u524D\u5185\u5BB9\u3002\u662F\u5426\u4ECD\u7136\u7EE7\u7EED\uFF1F
save-download = \u4E0B\u8F7D
save-replace = \u66FF\u6362
save-delete = \u5220\u9664
save-backup-all = \u4E0B\u8F7D\u6240\u6709\u5B58\u6863\u6587\u4EF6
`,"volume-controls.ftl":`volume-controls-mute = \u9759\u97F3
volume-controls-unmute = \u53D6\u6D88\u9759\u97F3
`},"zh-TW":{"context_menu.ftl":`context-menu-download-swf = \u4E0B\u8F09SWF\u6A94\u6848
context-menu-copy-debug-info = \u8907\u88FD\u9664\u932F\u8CC7\u8A0A
context-menu-open-save-manager = \u958B\u555F\u5B58\u6A94\u7BA1\u7406\u5668
context-menu-about-ruffle =
    { $flavor ->
        [extension] \u95DC\u65BCRuffle\u64F4\u5145\u529F\u80FD ({ $version })
       *[other] \u95DC\u65BCRuffle ({ $version })
    }
context-menu-hide = \u96B1\u85CF\u83DC\u55AE
context-menu-exit-fullscreen = \u9000\u51FA\u5168\u87A2\u5E55
context-menu-enter-fullscreen = \u9032\u5165\u5168\u87A2\u5E55
context-menu-volume-controls = \u97F3\u91CF\u63A7\u5236
`,"messages.ftl":`message-cant-embed =
    Ruffle \u7121\u6CD5\u57F7\u884C\u672C\u9801\u9762\u5167\u5D4C\u7684 Flash\u3002
    \u60A8\u53EF\u4EE5\u5617\u8A66\u5728\u55AE\u7368\u7684\u6A19\u7C64\u9801\u4E2D\u958B\u555F\u6A94\u6848\uFF0C\u4EE5\u907F\u514D\u6B64\u554F\u984C\u3002
message-restored-from-bfcache =
    \u60A8\u7684\u700F\u89BD\u5668\u5F9E\u4E4B\u524D\u7684\u6703\u8A71\u4E2D\u9084\u539F\u4E86\u6B64 Flash \u5167\u5BB9\u3002
    \u82E5\u8981\u91CD\u65B0\u958B\u59CB\uFF0C\u8ACB\u91CD\u65B0\u8F09\u5165\u9801\u9762\u3002
panic-title = \u767C\u751F\u4E86\u67D0\u4E9B\u932F\u8AA4 :(
more-info = \u66F4\u591A\u8CC7\u8A0A
run-anyway = \u76F4\u63A5\u57F7\u884C
continue = \u7E7C\u7E8C
report-bug = \u56DE\u5831BUG
update-ruffle = \u66F4\u65B0Ruffle
ruffle-demo = \u7DB2\u9801\u5C55\u793A
ruffle-desktop = \u684C\u9762\u61C9\u7528\u7A0B\u5F0F
ruffle-wiki = \u67E5\u770BRuffle Wiki
enable-hardware-acceleration = \u770B\u8D77\u4F86\u786C\u9AD4\u52A0\u901F\u5DF2\u505C\u7528\u3002\u96D6\u7136 Ruffle \u53EF\u4EE5\u904B\u4F5C\uFF0C\u4F46\u901F\u5EA6\u53EF\u80FD\u5F88\u6162\u3002\u60A8\u53EF\u4EE5\u900F\u904E\u4EE5\u4E0B\u9023\u7D50\u77AD\u89E3\u5982\u4F55\u555F\u7528\u786C\u9AD4\u52A0\u901F\uFF1A
enable-hardware-acceleration-link = FAQ - Chrome\u786C\u9AD4\u52A0\u901F
view-error-details = \u6AA2\u8996\u932F\u8AA4\u8A73\u7D30\u8CC7\u6599
open-in-new-tab = \u958B\u555F\u65B0\u589E\u5206\u9801
click-to-unmute = \u9EDE\u64CA\u4EE5\u53D6\u6D88\u975C\u97F3
clipboard-message-title = \u5728 Ruffle \u4E2D\u8907\u88FD\u548C\u8CBC\u4E0A
clipboard-message-description =
    { $variant ->
       *[unsupported] \u60A8\u7684\u700F\u89BD\u5668\u4E0D\u652F\u63F4\u5B8C\u6574\u7684\u526A\u8CBC\u677F\u5B58\u53D6\u3001
        [access-denied] \u5DF2\u62D2\u7D55\u5B58\u53D6\u526A\u8CBC\u7C3F\u3001
    } \u4F46\u60A8\u53EF\u4EE5\u4F7F\u7528\u9019\u4E9B\u6377\u5F91\u4F86\u4EE3\u66FF\uFF1A
clipboard-message-copy = { " " } \u8907\u88FD
clipboard-message-cut = { " " } \u526A\u4E0B
clipboard-message-paste = { " " } \u8CBC\u4E0A
error-canvas-reload = \u7576\u756B\u5E03\u6E32\u67D3\u5668\u5DF2\u5728\u4F7F\u7528\u4E2D\u6642\uFF0C\u7121\u6CD5\u4F7F\u7528\u756B\u5E03\u6E32\u67D3\u5668\u91CD\u65B0\u8F09\u5165\u3002
error-file-protocol =
    \u60A8\u4F3C\u4E4E\u662F\u5728 \u300Cfile: \u300D\u5354\u5B9A\u4E0A\u57F7\u884C Ruffle\u3002
    \u9019\u4E26\u4E0D\u53EF\u884C\uFF0C\u56E0\u70BA\u700F\u89BD\u5668\u57FA\u65BC\u5B89\u5168\u7406\u7531\u6703\u963B\u64CB\u8A31\u591A\u529F\u80FD\u7684\u904B\u4F5C\u3002
    \u76F8\u53CD\uFF0C\u6211\u5011\u9080\u8ACB\u60A8\u8A2D\u5B9A\u672C\u6A5F\u4F3A\u670D\u5668\uFF0C\u6216\u4F7F\u7528\u7DB2\u9801\u793A\u7BC4\u6216\u684C\u9762\u61C9\u7528\u7A0B\u5F0F\u3002
error-javascript-config =
    \u7531\u65BC JavaScript \u8A2D\u5B9A\u4E0D\u6B63\u78BA\uFF0CRuffle \u9047\u5230\u4E86\u91CD\u5927\u554F\u984C\u3002
    \u5982\u679C\u60A8\u662F\u4F3A\u670D\u5668\u7BA1\u7406\u54E1\uFF0C\u6211\u5011\u9080\u8ACB\u60A8\u6AA2\u67E5\u932F\u8AA4\u7D30\u7BC0\uFF0C\u627E\u51FA\u662F\u54EA\u500B\u53C3\u6578\u51FA\u4E86\u554F\u984C\u3002
    \u60A8\u4E5F\u53EF\u4EE5\u53C3\u8003 Ruffle wiki \u4EE5\u7372\u5F97\u5354\u52A9\u3002
error-wasm-not-found =
    Ruffle \u672A\u80FD\u8F09\u5165\u6240\u9700\u7684 \u300C.wasm\u300D \u6A94\u6848\u5143\u4EF6\u3002
    \u5982\u679C\u60A8\u662F\u4F3A\u670D\u5668\u7BA1\u7406\u54E1\uFF0C\u8ACB\u78BA\u8A8D\u6A94\u6848\u5DF2\u6B63\u78BA\u4E0A\u50B3\u3002
    \u5982\u679C\u554F\u984C\u4ECD\u7136\u5B58\u5728\uFF0C\u60A8\u53EF\u80FD\u9700\u8981\u4F7F\u7528\u300CpublicPath\u300D\u8A2D\u5B9A\uFF1A\u8ACB\u53C3\u95B1 Ruffle wiki \u4EE5\u7372\u5F97\u5354\u52A9\u3002
error-wasm-mime-type =
    Ruffle \u5728\u5617\u8A66\u521D\u59CB\u5316\u6642\u9047\u5230\u91CD\u5927\u554F\u984C\u3002
    \u6B64 Web \u4F3A\u670D\u5668\u7121\u6CD5\u63D0\u4F9B MIME \u985E\u578B\u6B63\u78BA\u7684 \u300C.wasm \u300D\u6A94\u6848\u3002
    \u5982\u679C\u60A8\u662F\u4F3A\u670D\u5668\u7BA1\u7406\u54E1\uFF0C\u8ACB\u53C3\u95B1 Ruffle wiki \u4EE5\u7372\u5F97\u5354\u52A9\u3002
error-invalid-swf =
    Ruffle \u7121\u6CD5\u89E3\u6790\u8ACB\u6C42\u7684\u6A94\u6848\u3002
    \u6700\u53EF\u80FD\u7684\u539F\u56E0\u662F\u8ACB\u6C42\u7684\u6A94\u6848\u4E0D\u662F\u6709\u6548\u7684 SWF\u3002
error-swf-fetch =
    Ruffle \u672A\u80FD\u8F09\u5165 Flash SWF \u6A94\u6848\u3002
    \u6700\u53EF\u80FD\u7684\u539F\u56E0\u662F\u8A72\u6A94\u6848\u5DF2\u4E0D\u5B58\u5728\uFF0C\u56E0\u6B64 Ruffle \u7121\u6CD5\u8F09\u5165\u4EFB\u4F55\u5167\u5BB9\u3002
    \u8ACB\u5617\u8A66\u806F\u7D61\u7DB2\u7AD9\u7BA1\u7406\u54E1\u5C0B\u6C42\u5354\u52A9\u3002
error-swf-cors =
    Ruffle \u672A\u80FD\u8F09\u5165 Flash SWF \u6A94\u6848\u3002
    \u8A2A\u554F fetch \u53EF\u80FD\u5DF2\u88AB CORS \u7B56\u7565\u5C01\u9396\u3002
    \u5982\u679C\u60A8\u662F\u4F3A\u670D\u5668\u7BA1\u7406\u54E1\uFF0C\u8ACB\u53C3\u95B1 Ruffle wiki \u4EE5\u7372\u5F97\u5354\u52A9\u3002
error-wasm-cors =
    Ruffle \u672A\u80FD\u8F09\u5165\u6240\u9700\u7684 \u300C.wasm\u300D \u6A94\u6848\u5143\u4EF6\u3002
    \u8A2A\u554F fetch \u53EF\u80FD\u5DF2\u88AB CORS \u7B56\u7565\u5C01\u9396\u3002
    \u5982\u679C\u60A8\u662F\u4F3A\u670D\u5668\u7BA1\u7406\u54E1\uFF0C\u8ACB\u53C3\u95B1 Ruffle wiki \u4EE5\u7372\u5F97\u5354\u52A9\u3002
error-wasm-invalid =
    Ruffle \u5728\u5617\u8A66\u521D\u59CB\u5316\u6642\u9047\u5230\u91CD\u5927\u554F\u984C\u3002
    \u6B64\u9801\u9762\u4F3C\u4E4E\u6709\u907A\u5931\u6216\u7121\u6548\u7684\u6A94\u6848\uFF0C\u7121\u6CD5\u57F7\u884C Ruffle\u3002
    \u5982\u679C\u60A8\u662F\u4F3A\u670D\u5668\u7BA1\u7406\u54E1\uFF0C\u8ACB\u53C3\u95B1 Ruffle wiki \u4EE5\u7372\u5F97\u5354\u52A9\u3002
error-wasm-download =
    Ruffle \u5728\u5617\u8A66\u521D\u59CB\u5316\u6642\u9047\u5230\u91CD\u5927\u554F\u984C\u3002
    \u9019\u901A\u5E38\u53EF\u4EE5\u81EA\u884C\u89E3\u6C7A\uFF0C\u56E0\u6B64\u60A8\u53EF\u4EE5\u5617\u8A66\u91CD\u65B0\u8F09\u5165\u9801\u9762\u3002
    \u5426\u5247\uFF0C\u8ACB\u806F\u7D61\u7DB2\u7AD9\u7BA1\u7406\u54E1\u3002
error-wasm-disabled-on-edge =
    Ruffle \u672A\u80FD\u8F09\u5165\u6240\u9700\u7684\u300C.wasm \u300D\u6A94\u6848\u5143\u4EF6\u3002
    \u8981\u89E3\u6C7A\u9019\u500B\u554F\u984C\uFF0C\u8ACB\u5617\u8A66\u6253\u958B\u700F\u89BD\u5668\u7684\u8A2D\u5B9A\uFF0C\u6309\u4E00\u4E0B\u300C\u96B1\u79C1\u3001\u641C\u5C0B\u548C\u670D\u52D9\u300D\uFF0C\u5411\u4E0B\u6372\u52D5\uFF0C\u7136\u5F8C\u95DC\u9589\u300C\u52A0\u5F37\u60A8\u5728\u7DB2\u8DEF\u4E0A\u7684\u5B89\u5168\u6027\u300D\u3002
    \u9019\u5C07\u5141\u8A31\u60A8\u7684\u700F\u89BD\u5668\u8F09\u5165\u6240\u9700\u7684\u300C.wasm \u300D\u6A94\u6848\u3002
    \u5982\u679C\u554F\u984C\u4ECD\u7136\u5B58\u5728\uFF0C\u60A8\u53EF\u80FD\u5FC5\u9808\u4F7F\u7528\u5176\u4ED6\u700F\u89BD\u5668\u3002
error-wasm-unsupported-browser =
    \u60A8\u4F7F\u7528\u7684\u700F\u89BD\u5668\u4E0D\u652F\u63F4 Ruffle \u57F7\u884C\u6240\u9700\u7684 WebAssembly \u64F4\u5145\u5957\u4EF6\u3002
    \u8ACB\u5207\u63DB\u5230\u652F\u63F4\u7684\u700F\u89BD\u5668\u3002
    \u60A8\u53EF\u4EE5\u5728 Wiki \u4E0A\u627E\u5230\u652F\u63F4\u7684\u700F\u89BD\u5668\u6E05\u55AE\u3002
error-javascript-conflict =
    Ruffle \u5728\u5617\u8A66\u521D\u59CB\u5316\u6642\u9047\u5230\u91CD\u5927\u554F\u984C\u3002
    \u9019\u500B\u9801\u9762\u4F3C\u4E4E\u4F7F\u7528\u4E86\u8207 Ruffle \u76F8\u885D\u7A81\u7684 JavaScript \u7A0B\u5F0F\u78BC\u3002
    \u5982\u679C\u60A8\u662F\u4F3A\u670D\u5668\u7BA1\u7406\u54E1\uFF0C\u8ACB\u5617\u8A66\u5728\u7A7A\u767D\u9801\u9762\u4E0A\u8F09\u5165\u6A94\u6848\u3002
error-javascript-conflict-outdated = \u60A8\u4E5F\u53EF\u4EE5\u5617\u8A66\u4E0A\u50B3\u8F03\u65B0\u7248\u672C\u7684 Ruffle\uFF0C\u53EF\u80FD\u6703\u907F\u514D\u6B64\u554F\u984C (\u76EE\u524D\u7684\u7248\u672C\u5DF2\u904E\u6642\uFF1A{ $buildDate })\u3002
error-csp-conflict =
    Ruffle \u5728\u5617\u8A66\u521D\u59CB\u5316\u6642\u9047\u5230\u91CD\u5927\u554F\u984C\u3002
    \u6B64\u7DB2\u9801\u4F3A\u670D\u5668\u7684\u5167\u5BB9\u5B89\u5168\u653F\u7B56\u4E0D\u5141\u8A31\u57F7\u884C\u6240\u9700\u7684 \u300C.wasm \u300D\u5143\u4EF6\u3002
    \u5982\u679C\u60A8\u662F\u4F3A\u670D\u5668\u7BA1\u7406\u54E1\uFF0C\u8ACB\u53C3\u95B1 Ruffle wiki \u4EE5\u53D6\u5F97\u5354\u52A9\u3002
error-url-invalid =
    Ruffle \u7121\u6CD5\u8F09\u5165 Flash SWF \u6A94\u6848\u3002
    \u6700\u53EF\u80FD\u7684\u539F\u56E0\u662F\u50B3\u905E\u7D66 Ruffle \u7684 SWF \u6A94\u6848\u7DB2\u5740\u7121\u6548\u3002
error-unknown =
    Ruffle \u5728\u5617\u8A66\u986F\u793A\u6B64 Flash \u5167\u5BB9\u6642\u9047\u5230\u4E86\u91CD\u5927\u554F\u984C\u3002
    { $outdated ->
        [true]  \u5982\u679C\u60A8\u662F\u4F3A\u670D\u5668\u7BA1\u7406\u54E1\uFF0C\u8ACB\u5617\u8A66\u4E0A\u50B3\u8F03\u65B0\u7248\u672C\u7684 Ruffle (\u76EE\u524D\u7684\u7248\u672C\u5DF2\u7D93\u904E\u6642 { $buildDate })\u3002
       *[false] \u9019\u4E0D\u61C9\u8A72\u767C\u751F\uFF0C\u6240\u4EE5\u5982\u679C\u60A8\u80FD\u63D0\u51FA\u932F\u8AA4\uFF0C\u6211\u5011\u6703\u975E\u5E38\u611F\u6FC0\uFF01
    }
`,"save-manager.ftl":`save-delete-prompt = \u4F60\u78BA\u5B9A\u8981\u522A\u9664\u9019\u500B\u5B58\u6A94\u55CE\uFF1F
save-reload-prompt =
    \u552F\u4E00\u65B9\u6CD5\u53EA\u6709 { $action ->
        [delete] \u522A\u9664
       *[replace] \u53D6\u4EE3
    } \u9019\u500B\u5B58\u6A94\u4E0D\u6703\u5B8C\u5168\u53D6\u4EE3\u76F4\u5230\u91CD\u65B0\u555F\u52D5\u3002 \u4F60\u9700\u8981\u7E7C\u7E8C\u55CE?
save-download = \u4E0B\u8F09
save-replace = \u53D6\u4EE3
save-delete = \u522A\u9664
save-backup-all = \u4E0B\u8F09\u6240\u6709\u5B58\u6A94\u6A94\u6848\u3002
`,"volume-controls.ftl":`volume-controls-mute = \u975C\u97F3
volume-controls-unmute = \u53D6\u6D88\u975C\u97F3
`}},xj={};for(let[r,A]of Object.entries(V6)){let e=new re(r);if(A){for(let[j,a]of Object.entries(A))if(a)for(let t of e.addResource(new te(a)))console.error(`Error in text for ${r} ${j}: ${t}`)}xj[r]=e}function X6(r,A,e){let j=xj[r];if(j!==void 0){let a=j.getMessage(A);if(a!==void 0&&a.value)return j.formatPattern(a.value,e)}return null}function F(r,A){let e=wj(navigator.languages,Object.keys(xj),{defaultLocale:"en-US"});for(let j in e){let a=X6(e[j],r,A);if(a)return a}return console.error(`Unknown text key '${r}'`),r}function C(r,A){let e=document.createElement("div");return F(r,A).split(`
`).forEach(j=>{let a=document.createElement("p");a.innerText=j,e.appendChild(a)}),e}function Ya(r,A=F){for(let e of r.querySelectorAll("[data-i18n-key]"))e.textContent=A(e.dataset.i18nKey);for(let e of r.querySelectorAll("[data-i18n-title-key]"))e.setAttribute("title",A(e.dataset.i18nTitleKey))}l();l();var Ve="application/x-shockwave-flash",Xe="application/futuresplash",ze="application/x-shockwave-flash2-preview",We="application/vnd.adobe.flash.movie",Za="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000";function z6(r){let A="";try{A=new URL(r,"https://example.com").pathname}catch{}if(A&&A.length>=4){let e=A.slice(-4).toLowerCase();if(e===".swf"||e===".spl")return!0}return!1}function W6(r,A){switch(r=r.toLowerCase(),r){case Ve.toLowerCase():case Xe.toLowerCase():case ze.toLowerCase():case We.toLowerCase():return!0;default:if(A)switch(r){case"application/octet-stream":case"binary/octet-stream":return!0}}return!1}function Ne(r,A){let e=z6(r);return A?W6(A,e):e}function Ar(r){let A=r.pathname;return A.substring(A.lastIndexOf("/")+1)}l();var Je=null,AA=!1;try{if(document.currentScript instanceof HTMLScriptElement&&document.currentScript.src!==""){let r=document.currentScript.src;!r.endsWith(".js")&&!r.endsWith("/")&&(r+="/"),Je=new URL(".",r),AA=Je.protocol.includes("extension")}}catch(r){console.warn("Unable to get currentScript URL",r)}l();var kA="https://ruffle.rs";l();var LA=class extends Error{constructor(A,e){super(`Failed to fetch ${A}`),this.swfUrl=A,this.statusNotOk=e,this.swfUrl=A,this.statusNotOk=e}},UA=class extends Error{constructor(A){super(`Not a valid swf: ${A}`)}},FA=class extends Error{constructor(A){super("Failed to load Ruffle WASM"),this.cause=A}},VA=class extends Error{constructor(A){super(`Failed to begin SWF load: ${A}`)}},XA=class extends Error{constructor(A){super(`Invalid options: ${A}`)}};l();var S=X(M(),1);var Oj=X(or(),1);function At({action:r,showDetails:A,errorArray:e,errorText:j,swfUrl:a}){if(r.type==="show_details")return(0,S.jsx)("li",{children:(0,S.jsx)("a",{href:"#",id:"panic-view-details",onClick:o=>{o.preventDefault(),A()},children:F("view-error-details")})});if(r.type==="open_link")return(0,S.jsx)("li",{children:(0,S.jsx)("a",{href:r.url,target:"_top",children:r.label})});{let t;document.location.protocol.includes("extension")&&a?t=a.href:t=document.location.href,t=t.split(/[?#]/,1)[0];let o=`Error on ${t}`,f=`https://github.com/ruffle-rs/ruffle/issues/new?title=${encodeURIComponent(o)}&template=error_report.md&labels=error-report&body=`,s=encodeURIComponent(j);return e.stackIndex>-1&&String(f+s).length>8195&&(e[e.stackIndex]=null,e.avmStackIndex>-1&&(e[e.avmStackIndex]=null),s=encodeURIComponent(e.join(""))),f+=s,(0,S.jsx)("li",{children:(0,S.jsx)("a",{href:f,target:"_top",children:F("report-bug")})})}}function Cj(){let r=new Date(U.buildDate),A=new Date;return A.setMonth(A.getMonth()-6),A>r}var v={OpenDemo:{type:"open_link",url:kA+"/demo",label:F("ruffle-demo")},DownloadDesktop:{type:"open_link",url:kA+"/downloads#desktop-app",label:F("ruffle-desktop")},UpdateRuffle:{type:"open_link",url:kA+"/downloads",label:F("update-ruffle")},CreateReport:{type:"create_report"},ShowDetails:{type:"show_details"},createReportOrUpdate(){return Cj()?this.UpdateRuffle:this.CreateReport},openWiki(r,A){return{type:"open_link",url:`https://github.com/ruffle-rs/ruffle/wiki/${r}`,label:A??F("ruffle-wiki")}}};function et(r){if(r instanceof LA)return r.swfUrl&&!r.swfUrl.protocol.includes("http")?{body:C("error-file-protocol"),actions:[v.OpenDemo,v.DownloadDesktop]}:window.location.origin===r.swfUrl?.origin||r.statusNotOk||window.location.protocol.includes("extension")?{body:C("error-swf-fetch"),actions:[v.ShowDetails]}:{body:C("error-swf-cors"),actions:[v.openWiki("Using-Ruffle#configure-cors-header"),v.ShowDetails]};if(r instanceof UA)return{body:C("error-invalid-swf"),actions:[v.ShowDetails]};if(r instanceof FA){if(window.location.protocol==="file:")return{body:C("error-file-protocol"),actions:[v.OpenDemo,v.DownloadDesktop]};let A=String(r.cause.message).toLowerCase();if(A.includes("mime"))return{body:C("error-wasm-mime-type"),actions:[v.openWiki("Using-Ruffle#configure-webassembly-mime-type"),v.ShowDetails]};if(A.includes("networkerror")||A.includes("failed to fetch")||A.includes("load failed"))return{body:C("error-wasm-cors"),actions:[v.openWiki("Using-Ruffle#configure-cors-header"),v.ShowDetails]};if(A.includes("disallowed by embedder"))return{body:C("error-csp-conflict"),actions:[v.openWiki("Using-Ruffle#configure-wasm-csp"),v.ShowDetails]};if(r.cause.name==="CompileError"&&A.includes("bad type"))return{body:C("error-wasm-unsupported-browser"),actions:[v.openWiki("#web"),v.ShowDetails]};if(r.cause.name==="CompileError"||A.includes("failed to execute 'compile' on 'webassembly'"))return{body:C("error-wasm-invalid"),actions:[v.openWiki("Using-Ruffle#addressing-a-compileerror"),v.ShowDetails]};if((A.includes("could not download wasm module")||A.includes("webassembly compilation aborted"))&&r.cause.name==="TypeError")return{body:C("error-wasm-download"),actions:[v.ShowDetails]};if(r.cause.name==="TypeError"){let e=C("error-javascript-conflict");return Cj()&&e.appendChild(C("error-javascript-conflict-outdated",{buildDate:U.buildDate})),{body:e,actions:[v.createReportOrUpdate(),v.ShowDetails]}}return navigator.userAgent.includes("Edg")&&A.includes("webassembly is not defined")?{body:C("error-wasm-disabled-on-edge"),actions:[v.openWiki("Frequently-Asked-Questions-For-Users#edge-webassembly-error",F("more-info")),v.ShowDetails]}:{body:C("error-wasm-not-found"),actions:[v.openWiki("Using-Ruffle#configuration-options"),v.ShowDetails]}}if(r instanceof XA)return{body:C("error-javascript-config"),actions:[v.openWiki("Using-Ruffle#javascript-api"),v.ShowDetails]};if(r instanceof VA){let A=String(r.message).toLowerCase();if(A.includes("is not a valid url")||A.includes("invalid url")||A.includes("invalid base url")){let e;try{new URL(document.baseURI),e=!0}catch{e=!1}return e?{body:C("error-url-invalid"),actions:[v.ShowDetails]}:{body:C("error-javascript-conflict"),actions:[v.ShowDetails]}}}return{body:C("error-unknown",{buildDate:U.buildDate,outdated:String(Cj)}),actions:[v.createReportOrUpdate(),v.ShowDetails]}}function cr(r,A,e,j){let a=e.join(""),{body:t,actions:o}=et(A),f=(0,Oj.createRef)(),s=(0,Oj.createRef)(),i=()=>{f.current.classList.remove("hidden")};r.textContent="",r.appendChild((0,S.jsxs)("div",{id:"panic",children:[(0,S.jsx)("div",{id:"panic-title",children:F("panic-title")}),(0,S.jsx)("div",{id:"panic-body",children:t}),(0,S.jsx)("div",{id:"panic-footer",children:(0,S.jsx)("ul",{children:o.map(b=>At({action:b,showDetails:i,errorText:a,errorArray:e,swfUrl:j}))})}),(0,S.jsx)("div",{id:"panic-details-modal",class:"hidden",ref:f,children:(0,S.jsxs)("div",{id:"panic-details-content",children:[(0,S.jsx)("span",{class:"panic-copy-button",title:"Copy to clipboard",ref:s,onClick:()=>{s.current&&(navigator.clipboard?.writeText(a),s.current.classList.add("copied"),setTimeout(()=>{s.current?.classList.remove("copied")},2e3))}}),(0,S.jsx)("span",{class:"close-modal",onClick:()=>f.current.classList.add("hidden")}),(0,S.jsx)("textarea",{readOnly:!0,children:a})]})})]}))}l();var Pj="./ruffle_web_bg.UNPWTSW4.wasm";l();var fr=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,4,1,96,0,0,3,2,1,0,5,3,1,0,1,10,14,1,12,0,65,0,65,0,65,0,252,10,0,0,11]));var nr=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,4,1,96,0,0,3,2,1,0,10,7,1,5,0,208,112,26,11]));var sr=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,4,1,96,0,0,3,2,1,0,10,12,1,10,0,67,0,0,0,0,252,0,26,11])),lr=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,4,1,96,0,0,3,2,1,0,10,8,1,6,0,65,0,192,26,11])),dr=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,5,1,96,0,1,123,3,2,1,0,10,10,1,8,0,65,0,253,15,253,98,11]));bj();l();function Ye(r){let A=Je?.href??"";return!AA&&"publicPath"in r&&r.publicPath!==null&&r.publicPath!==void 0&&(A=r.publicPath),A!==""&&!A.endsWith("/")&&(A+="/"),A}async function Eo(r){fa();let A=(await Promise.all([fr(),dr(),sr(),lr(),nr()])).every(Boolean);IA.options.onFirstLoad?.(),IA.options.onFirstLoad=()=>{};let{default:e,RuffleInstanceBuilder:j,ZipWriter:a}=await(A?Promise.resolve().then(()=>(Nj(),Wj)):Promise.resolve().then(()=>(Nj(),Wj))),t,o;{let i=new URL(Ye(window.RufflePlayer?.config??{}),document.baseURI);o=A?new URL(Pj,i):new URL(Pj,i)}let f=await fetch(o);if(r&&typeof ReadableStreamDefaultController=="function"){let i=f?.headers?.get("content-length")||"",b=0,E=parseInt(i);t=new Response(new ReadableStream({async start(h){let T=f.body?.getReader();if(!T)throw"Response had no body";for(r(b,E);;){let{done:Q,value:fA}=await T.read();if(Q)break;fA?.byteLength&&(b+=fA?.byteLength),h.enqueue(fA),r(b,E)}h.close()}}),f)}else t=f;return await e({module_or_path:t}),[j,a]}var Jj=null;async function Fr(r){Jj===null&&(Jj=Eo(r));let A=await Jj;return[new A[0],()=>new A[1]]}l();l();function mo(r,A,e){let j=[],a=0,t=0;for(;a<r.length&&t<A.length;){let o=r[a],f=A[t];e(o,f)<=0?(j.push(o),a++):(j.push(f),t++)}for(;a<r.length;)j.push(r[a++]);for(;t<A.length;)j.push(A[t++]);return j}function go(r,A){if(r===A)return 0;let e=r.compareDocumentPosition(A);return e&Node.DOCUMENT_POSITION_FOLLOWING?-1:e&Node.DOCUMENT_POSITION_PRECEDING?1:0}function Bo(r){let A=["ruffle-embed"];for(let e=1;e<=r;e++)A.push(`ruffle-embed-${e}`);return A.join(", ")}function $r(r){let A=Object.getOwnPropertyDescriptor(Document.prototype,"embeds");if(!A?.get)return;let e=Symbol("ruffle_embeds_cache");Object.defineProperty(Document.prototype,"embeds",{get(){let j=this,a=j[e];if(a)return a;let t=null,o=()=>{let b=A.get.call(this),E=Bo(r),h=Array.from(this.querySelectorAll(E));return mo(Array.from(b),h,go)},f=()=>(t!==null||(t=o(),queueMicrotask(()=>{t=null})),t),s=Object.create(HTMLCollection.prototype);Object.defineProperty(s,"length",{enumerable:!0,configurable:!0,get(){return f().length}}),s.item=function(b){return f()[b]??null},s.namedItem=function(b){let E=f();for(let h of E){let T=h;if(b&&(T.getAttribute("name")===b||T.id===b))return T}return null},s[Symbol.iterator]=function*(){for(let b of f())yield b};let i=new Proxy(s,{get(b,E,h){if(typeof E=="string"){let T=Number(E);if(!Number.isNaN(T)&&T>=0)return f()[T];if(Reflect.has(b,E))return Reflect.get(b,E,h);let Q=b.namedItem(E);if(Q)return Q}return Reflect.get(b,E,h)},has(b,E){if(typeof E=="string"){let h=Number(E);return!Number.isNaN(h)&&h>=0?h<f().length:Reflect.has(b,E)?!0:b.namedItem(E)!==null}return Reflect.has(b,E)},ownKeys(){let b=f().length,E=[];for(let h=0;h<b;h++)E.push(String(h));return E},getOwnPropertyDescriptor(b,E){if(typeof E=="string"){let h=Number(E);if(!Number.isNaN(h)&&h>=0&&h<f().length)return{enumerable:!0,configurable:!0,writable:!1,value:f()[h]}}return Reflect.getOwnPropertyDescriptor(b,E)}});return i[e]=!0,j[e]=i,i},configurable:!0,enumerable:!0})}var ho=999,Qj={};function yr(r){let A=Qj[r];return A!==void 0?{internalName:r,name:A.name,class:A.class}:null}function JA(r,A){let e=Qj[r];if(e!==void 0){if(e.class!==A)throw new Error("Internal naming conflict on "+r);return e.name}let j=0;if(window.customElements!==void 0)for(;j<ho;){let a=r;if(j>0&&(a=a+"-"+j),window.customElements.get(a)!==void 0){j+=1;continue}else window.customElements.define(a,A),r==="ruffle-embed"&&$r(j);return Qj[r]={class:A,name:a,internalName:r},a}throw new Error("Failed to assign custom element "+r)}l();function y(r){return r!=null}function wr(r,A){if(y(A.allowScriptAccess)&&r.setAllowScriptAccess(A.allowScriptAccess),y(A.backgroundColor)&&r.setBackgroundColor(qo(A.backgroundColor)),y(A.upgradeToHttps)&&r.setUpgradeToHttps(A.upgradeToHttps),y(A.compatibilityRules)&&r.setCompatibilityRules(A.compatibilityRules),y(A.letterbox)&&r.setLetterbox(A.letterbox.toLowerCase()),y(A.base)&&r.setBaseUrl(A.base),y(A.menu)&&r.setShowMenu(A.menu),y(A.allowFullscreen)&&r.setAllowFullscreen(A.allowFullscreen),y(A.salign)&&r.setStageAlign(A.salign.toLowerCase()),y(A.forceAlign)&&r.setForceAlign(A.forceAlign),y(A.quality)?r.setQuality(A.quality.toLowerCase()):vo()&&(console.log("Running on a mobile device; defaulting to low quality"),r.setQuality("low")),y(A.scale)&&r.setScale(A.scale.toLowerCase()),y(A.forceScale)&&r.setForceScale(A.forceScale),y(A.frameRate)&&r.setFrameRate(A.frameRate),y(A.wmode)&&r.setWmode(A.wmode),y(A.logLevel)&&r.setLogLevel(A.logLevel),y(A.maxExecutionDuration)&&r.setMaxExecutionDuration(Go(A.maxExecutionDuration)),y(A.playerVersion)&&r.setPlayerVersion(A.playerVersion),y(A.preferredRenderer)&&r.setPreferredRenderer(A.preferredRenderer),y(A.openUrlMode)&&r.setOpenUrlMode(A.openUrlMode.toLowerCase()),y(A.allowNetworking)&&r.setAllowNetworking(A.allowNetworking.toLowerCase()),y(A.credentialAllowList)&&r.setCredentialAllowList(A.credentialAllowList),y(A.playerRuntime)&&r.setPlayerRuntime(A.playerRuntime),y(A.socketProxy))for(let e of A.socketProxy)r.addSocketProxy(e.host,e.port,e.proxyUrl);if(y(A.gamepadButtonMapping))for(let[e,j]of Object.entries(A.gamepadButtonMapping))r.addGamepadButtonMapping(e,j);if(y(A.urlRewriteRules))for(let[e,j]of A.urlRewriteRules)if(e instanceof RegExp)r.addUrlRewriteRule(e,j);else{let a=e.replace(/[.*+?^${}()|[\]\\]/g,"\\$&"),t=new RegExp(`^${a}$`),o=j.replace(/\$/g,"$$$$");r.addUrlRewriteRule(t,o)}y(A.scrollingBehavior)&&r.setScrollingBehavior(A.scrollingBehavior),y(A.deviceFontRenderer)&&r.setDeviceFontRenderer(A.deviceFontRenderer)}function qo(r){if(r.startsWith("#")&&(r=r.substring(1)),r.length<6)return;let A=0;for(let e=0;e<6;e++){let j=parseInt(r[e],16);isNaN(j)?A=A<<4:A=A<<4|j}return A}function Go(r){return typeof r=="number"?r:r.secs}function vo(){return typeof window.orientation<"u"}var ko=/^\s*(\d+(\.\d+)?(%)?)/,ej=!1;function Yj(r){if(r==null)return{};r instanceof URLSearchParams||(r=new URLSearchParams(r));let A={};for(let[e,j]of r)A[e]=j.toString();return A}var aj=class{constructor(A,e){this.x=A,this.y=e}distanceTo(A){let e=A.x-this.x,j=A.y-this.y;return Math.sqrt(e*e+j*j)}},QA=class r{constructor(A,e,j){this.contextMenuForceDisabled=!1,this.isTouch=!1,this.contextMenuSupported=!1,this._suppressContextMenu=!1,this.panicked=!1,this.rendererDebugInfo="",this.longPressTimer=null,this.pointerDownPosition=null,this.pointerMoveMaxDistance=0,this.onFSCommand=[],this.config={},this.SaveRow=({rowKey:o,solName:f,solData:s})=>(0,D.jsxs)("tr",{children:[(0,D.jsx)("td",{title:o,children:f}),(0,D.jsx)("td",{children:(0,D.jsx)("span",{class:"save-option",id:"download-save",title:F("save-download"),onClick:()=>Zj(Fo(s,"application/octet-stream"),f+".sol")})}),(0,D.jsxs)("td",{children:[(0,D.jsx)("input",{type:"file",accept:".sol",class:"replace-save",id:"replace-save-"+o,onChange:i=>this.replaceSOL(i,o)}),(0,D.jsx)("label",{for:"replace-save-"+o,class:"save-option",id:"replace-save",title:F("save-replace")})]}),(0,D.jsx)("td",{children:(0,D.jsx)("span",{class:"save-option",id:"delete-save",title:F("save-delete"),onClick:()=>this.deleteSave(o)})})]}),this.element=A,this.debugPlayerInfo=e,this.onCallbackAvailable=j,this.shadow=this.element.attachShadow({mode:"open",delegatesFocus:!0}),this.shadow.appendChild(J.content.cloneNode(!0)),this.dynamicStyles=this.shadow.getElementById("dynamic-styles"),this.container=this.shadow.getElementById("container"),this.playButton=this.shadow.getElementById("play-button"),this.playButton.addEventListener("click",()=>this.play()),this.unmuteOverlay=this.shadow.getElementById("unmute-overlay"),this.splashScreen=this.shadow.getElementById("splash-screen"),this.virtualKeyboard=this.shadow.getElementById("virtual-keyboard"),this.virtualKeyboard.addEventListener("input",this.virtualKeyboardInput.bind(this)),this.saveManager=this.shadow.getElementById("save-manager"),this.videoModal=this.shadow.getElementById("video-modal"),this.hardwareAccelerationModal=this.shadow.getElementById("hardware-acceleration-modal"),this.volumeControls=this.shadow.getElementById("volume-controls-modal"),this.clipboardModal=this.shadow.getElementById("clipboard-modal"),this.addModalJavaScript(this.saveManager),this.addModalJavaScript(this.volumeControls),this.addModalJavaScript(this.videoModal),this.addModalJavaScript(this.hardwareAccelerationModal),this.addModalJavaScript(this.clipboardModal),this.volumeSettings=new Aa(!1,100),this.addVolumeControlsJavaScript(this.volumeControls);let a=this.saveManager.querySelector(".modal-button");a&&a.addEventListener("click",this.backupSaves.bind(this)),this.contextMenuOverlay=this.shadow.getElementById("context-menu-overlay"),this.contextMenuElement=this.shadow.getElementById("context-menu");let t=o=>{o.preventDefault(),o.stopPropagation()};this.contextMenuElement.addEventListener("contextmenu",t),this.contextMenuElement.addEventListener("click",t),this.localize(),window.addEventListener("languagechange",()=>this.localize()),document.documentElement.addEventListener("pointerdown",this.checkIfTouch.bind(this)),this.element.addEventListener("contextmenu",this.showContextMenu.bind(this)),this.container.addEventListener("pointerdown",this.pointerDown.bind(this)),this.container.addEventListener("pointermove",this.checkLongPressMovement.bind(this)),this.container.addEventListener("pointerup",this.checkLongPress.bind(this)),this.container.addEventListener("pointercancel",this.clearLongPressTimer.bind(this)),this.element.addEventListener("fullscreenchange",this.fullScreenChange.bind(this)),this.element.addEventListener("webkitfullscreenchange",this.fullScreenChange.bind(this)),this.instance=null,this.newZipWriter=null,this._readyState=lA.HaveNothing,this.metadata=null,this.lastActivePlayingState=!1,this.backgroundWorker=null,this.setupTabVisibilityHandling()}addFSCommandHandler(A){this.onFSCommand.push(A)}callFSCommand(A,e){if(this.onFSCommand.length===0)return!1;for(let j of this.onFSCommand)j(A,e);return!0}addModalJavaScript(A){let e=A.querySelector("#video-holder"),j=()=>{A.classList.add("hidden"),e&&(e.textContent="")};A.parentNode.addEventListener("click",j);let a=A.querySelector(".modal-area");a&&a.addEventListener("click",o=>o.stopPropagation());let t=A.querySelector(".close-modal");t&&t.addEventListener("click",j)}addVolumeControlsJavaScript(A){let e=A.querySelector("#mute-checkbox"),j=A.querySelector("#volume-mute"),a=[A.querySelector("#volume-min"),A.querySelector("#volume-mid"),A.querySelector("#volume-max")],t=A.querySelector("#volume-slider"),o=A.querySelector("#volume-slider-text"),f=()=>{if(this.volumeSettings.isMuted)j.style.display="inline",a.forEach(s=>{s.style.display="none"});else{j.style.display="none";let s=Math.round(this.volumeSettings.volume/50);a.forEach((i,b)=>{i.style.display=b===s?"inline":"none"})}};e.checked=this.volumeSettings.isMuted,t.disabled=e.checked,t.valueAsNumber=this.volumeSettings.volume,o.textContent=t.value+"%",f(),e.addEventListener("change",()=>{t.disabled=e.checked,this.volumeSettings.isMuted=e.checked,this.instance?.set_volume(this.volumeSettings.get_volume()),f()}),t.addEventListener("input",()=>{o.textContent=t.value+"%",this.volumeSettings.volume=t.valueAsNumber,this.instance?.set_volume(this.volumeSettings.get_volume()),f()})}localize(){Ya(this.shadow),this.contextMenuElement.dir=yo()}setupTabVisibilityHandling(){document.addEventListener("visibilitychange",()=>{if(!this.instance)return;let A=this.loadedConfig?.backgroundExecutionMode??iA.None;document.hidden?(this.lastActivePlayingState=this.instance.is_playing(),A===iA.MainThread?(this.instance.enable_background_tick_mode(),this.lastActivePlayingState&&this.startBackgroundTick()):this.instance.pause()):(A===iA.MainThread&&(this.stopBackgroundTick(),this.instance.restart_animation_loop()),this.lastActivePlayingState&&this.instance.play(),this.instance.audio_context()?.resume())})}startBackgroundTick(){let e=`
            const intervalMs = ${1e3/(this.metadata?.frameRate||24)};
            self.onmessage = () => {
                setTimeout(() => self.postMessage("tick"), intervalMs);
            };
            setTimeout(() => self.postMessage("tick"), intervalMs);
        `;try{let j=new Blob([e],{type:"application/javascript"}),a=URL.createObjectURL(j),t=new Worker(a);URL.revokeObjectURL(a),this.backgroundWorker=t,t.onmessage=()=>{this.backgroundWorker===t&&(this.instance?.tick_for_background(performance.now()),t.postMessage("ack"))}}catch(j){console.warn("Unable to create background Worker:",j),this.instance?.pause()}}stopBackgroundTick(){this.backgroundWorker?.terminate(),this.backgroundWorker=null}updateStyles(){if(this.dynamicStyles.sheet){if(this.dynamicStyles.sheet.cssRules)for(let a=this.dynamicStyles.sheet.cssRules.length-1;a>=0;a--)this.dynamicStyles.sheet.deleteRule(a);let A=this.element.attributes.getNamedItem("align");if(A!=null){let a=A.value.toLowerCase(),t=(()=>{switch(a){case"right":return"vertical-align: top; float: right;";case"left":return"vertical-align: top; float: left;";case"bottom":return"vertical-align: baseline;";case"top":return"vertical-align: top;";case"center":return"vertical-align: middle; vertical-align: -moz-middle-with-baseline;";case"middle":return"vertical-align: middle; vertical-align: -webkit-baseline-middle; vertical-align: -moz-middle-with-baseline;";case"absbottom":return"vertical-align: bottom;";case"absmiddle":case"abscenter":return"vertical-align: middle;";case"texttop":return"vertical-align: text-top;";default:return""}})();t&&this.dynamicStyles.sheet.insertRule(`:host { ${t} }`)}let e=this.element.attributes.getNamedItem("width");if(e!=null){let a=r.htmlDimensionToCssDimension(e.value);a!==null&&this.dynamicStyles.sheet.insertRule(`:host { width: ${a}; }`)}let j=this.element.attributes.getNamedItem("height");if(j!=null){let a=r.htmlDimensionToCssDimension(j.value);a!==null&&this.dynamicStyles.sheet.insertRule(`:host { height: ${a}; }`)}}}isUnusedFallbackObject(){let A=yr("ruffle-object");if(A!==null){let e=this.element.parentNode;for(;e!==document&&e!==null;){if(e.nodeName===A.name)return!0;e=e.parentNode}}return!1}async ensureFreshInstance(){this.destroy(),this.loadedConfig&&this.loadedConfig.splashScreen!==!1&&this.loadedConfig.preloader!==!1&&this.showSplashScreen(),this.loadedConfig&&this.loadedConfig.preloader===!1&&console.warn("The configuration option preloader has been replaced with splashScreen. If you own this website, please update the configuration."),this.loadedConfig&&this.loadedConfig.maxExecutionDuration&&typeof this.loadedConfig.maxExecutionDuration!="number"&&console.warn("Configuration: An obsolete format for duration for 'maxExecutionDuration' was used, please use a single number indicating seconds instead. For instance '15' instead of '{secs: 15, nanos: 0}'."),this.loadedConfig&&typeof this.loadedConfig.contextMenu=="boolean"&&console.warn('The configuration option contextMenu no longer takes a boolean. Use "on", "off", or "rightClickOnly".');let[A,e]=await Fr(this.onRuffleDownloadProgress.bind(this)).catch(t=>{console.error(`Serious error loading Ruffle: ${t}`);let o=new FA(t);throw this.panic(o),o});if(this.newZipWriter=e,wr(A,this.loadedConfig||{}),A.setVolume(this.volumeSettings.get_volume()),this.loadedConfig?.fontSources)for(let t of this.loadedConfig.fontSources)try{let o=await fetch(t);A.addFont(t,new Uint8Array(await o.arrayBuffer()))}catch(o){console.warn(`Couldn't download font source from ${t}`,o)}for(let t in this.loadedConfig?.defaultFonts){let o=this.loadedConfig.defaultFonts[t];o&&A.setDefaultFont(t,o)}this.instance=await A.build(this.container,this).catch(t=>{throw console.error(`Serious error loading Ruffle: ${t}`),this.panic(t),t}),this.rendererDebugInfo=this.instance.renderer_debug_info(),this.rendererDebugInfo.includes("Adapter Device Type: Cpu")&&this.container.addEventListener("mouseover",this.openHardwareAccelerationModal.bind(this),{once:!0});let j=this.instance.renderer_name(),a=this.instance.constructor;if(console.log("%cNew Ruffle instance created (Version: "+U.versionName+" | WebAssembly extensions: "+(a.is_wasm_simd_used()?"ON":"OFF")+" | Used renderer: "+(j??"")+")","background: #37528C; color: #FFAD33"),this.audioState()!=="running"&&(this.container.style.visibility="hidden",await new Promise(t=>{window.setTimeout(()=>{t()},200)}),this.container.style.visibility=""),this.unmuteAudioContext(),!this.loadedConfig||this.loadedConfig.autoplay===GA.On||this.loadedConfig.autoplay!==GA.Off&&this.audioState()==="running"){if(this.play(),this.audioState()!=="running"){(!this.loadedConfig||this.loadedConfig.unmuteOverlay!==TA.Hidden)&&(this.unmuteOverlay.style.display="block"),this.container.addEventListener("click",this.unmuteOverlayClicked.bind(this),{once:!0});let t=this.instance?.audio_context();t&&(t.onstatechange=()=>{t.state==="running"&&this.unmuteOverlayClicked(),t.onstatechange=null})}}else this.playButton.style.display="block"}onRuffleDownloadProgress(A,e){let j=this.splashScreen.querySelector(".loadbar-inner"),a=this.splashScreen.querySelector(".loadbar");Number.isNaN(e)?a&&(a.style.display="none"):j.style.width=`${100*(A/e)}%`}destroy(){this.instance&&(this.stopBackgroundTick(),this.instance.destroy(),this.instance=null,this.metadata=null,this._readyState=lA.HaveNothing,console.log("Ruffle instance destroyed."))}checkOptions(A){if(typeof A=="string")return{url:A};let e=(j,a)=>{if(!j){let t=new XA(a);throw this.panic(t),t}};return e(A!==null&&typeof A=="object","Argument 0 must be a string or object"),e("url"in A||"data"in A,"Argument 0 must contain a `url` or `data` key"),e(!("url"in A)||typeof A.url=="string","`url` must be a string"),A}async reload(){if(this.loadedConfig)await this.load(this.loadedConfig);else throw new Error("Cannot reload if load wasn't first called")}async reloadWithCanvasRenderer(){if(this.loadedConfig&&this.loadedConfig.preferredRenderer!==je.Canvas){let A={...this.loadedConfig,preferredRenderer:je.Canvas};await this.load(A)}else if(this.loadedConfig)this.panic(new Error(F("error-canvas-reload")));else throw new Error("Cannot reload if load wasn't first called")}async load(A,e=!1){if(A=this.checkOptions(A),!this.element.isConnected||this.isUnusedFallbackObject()){console.warn("Ignoring attempt to play a disconnected or suspended Ruffle element");return}if(!Ge(this.element))try{this.loadedConfig={...va,...e&&"url"in A?{allowScriptAccess:Dr("samedomain",A.url)}:{},...window.RufflePlayer?.config??{},...this.config,...A},this.loadedConfig.backgroundColor&&this.loadedConfig.wmode!==SA.Transparent&&(this.container.style.backgroundColor=this.loadedConfig.backgroundColor),await this.ensureFreshInstance(),"url"in A?(console.log(`Loading SWF file ${A.url}`),this.swfUrl=new URL(A.url,document.baseURI),this.instance.stream_from(this.swfUrl.href,Yj(A.parameters))):"data"in A&&(console.log("Loading SWF data"),delete this.swfUrl,this.instance.load_data(new Uint8Array(A.data),Yj(A.parameters),A.swfFileName||"movie.swf"))}catch(j){console.error(`Serious error occurred loading SWF file: ${j}`);let a=new VA(j);throw this.panic(a),a}}play(){this.instance&&(this.instance.play(),this.playButton.style.display="none")}get isPlaying(){return this.instance?this.instance.is_playing():!1}get volume(){return this.instance?this.instance.volume():1}set volume(A){this.instance&&this.instance.set_volume(A)}get fullscreenEnabled(){return!!(document.fullscreenEnabled||document.webkitFullscreenEnabled)}get isFullscreen(){return(document.fullscreenElement||document.webkitFullscreenElement)===this.element}setFullscreen(A){this.fullscreenEnabled&&A!==this.isFullscreen&&(A?this.enterFullscreen():this.exitFullscreen())}enterFullscreen(){let A={navigationUI:"hide"};this.element.requestFullscreen?this.element.requestFullscreen(A):this.element.webkitRequestFullscreen?this.element.webkitRequestFullscreen(A):this.element.webkitRequestFullScreen&&this.element.webkitRequestFullScreen(A)}exitFullscreen(){document.exitFullscreen?document.exitFullscreen():document.webkitExitFullscreen?document.webkitExitFullscreen():document.webkitCancelFullScreen&&document.webkitCancelFullScreen()}fullScreenChange(){if(this.isFullscreen&&screen.orientation&&typeof screen.orientation.lock=="function"){let A=this.loadedConfig?.fullScreenAspectRatio?.toLowerCase()??"";["portrait","landscape","any"].includes(A)&&screen.orientation.lock(A).catch(()=>{})}else try{screen.orientation.unlock()}catch{}this.instance?.set_fullscreen(this.isFullscreen)}checkIfTouch(A){this.isTouch=A.pointerType==="touch"||A.pointerType==="pen"}confirmReloadSave(A,e,j){if(jj(e)&&localStorage[A]){if(!j&&!confirm(F("save-delete-prompt")))return;let a=this.swfUrl?this.swfUrl.pathname:"",t=this.swfUrl?this.swfUrl.hostname:document.location.hostname,o=A.split("/").slice(1,-1).join("/");if(a.includes(o)&&A.startsWith(t)){confirm(F("save-reload-prompt",{action:j?"replace":"delete"}))&&this.loadedConfig&&(this.destroy(),j?localStorage.setItem(A,e):localStorage.removeItem(A),this.reload(),this.populateSaves(),this.saveManager.classList.add("hidden"));return}j?localStorage.setItem(A,e):localStorage.removeItem(A),this.populateSaves(),this.saveManager.classList.add("hidden")}}replaceSOL(A,e){let j=A.target,a=new FileReader;a.addEventListener("load",()=>{if(a.result&&typeof a.result=="string"){let t=new RegExp("data:.*;base64,"),o=a.result.replace(t,"");this.confirmReloadSave(e,o,!0)}}),j&&j.files&&j.files.length>0&&j.files[0]&&a.readAsDataURL(j.files[0])}checkSaves(){if(!this.saveManager.querySelector("#local-saves"))return!1;try{if(localStorage===null)return!1}catch{return!1}return Object.keys(localStorage).some(A=>{let e=A.split("/").pop(),j=localStorage.getItem(A);return e&&j&&jj(j)})}deleteSave(A){let e=localStorage.getItem(A);e&&this.confirmReloadSave(A,e,!1)}populateSaves(){if(!this.checkSaves())return;let A=this.saveManager.querySelector("#local-saves");A.textContent="",Object.keys(localStorage).forEach(e=>{let j=e.split("/").pop(),a=localStorage.getItem(e);j&&a&&jj(a)&&A.appendChild((0,D.jsx)(this.SaveRow,{rowKey:e,solName:j,solData:a}))})}async backupSaves(){let A=this.newZipWriter(),e=[];Object.keys(localStorage).forEach(a=>{let t=String(a.split("/").pop()),o=localStorage.getItem(a);if(o&&jj(o)){let f=Ir(o),s=e.filter(i=>i===t).length;e.push(t),s>0&&(t+=` (${s+1})`),A.addFile(t+".sol",f)}});let j=new Blob([A.save()],{type:"application/zip"});Zj(j,"saves.zip")}openHardwareAccelerationModal(){this.hardwareAccelerationModal.classList.remove("hidden")}async openSaveManager(){this.populateSaves(),this.saveManager.classList.remove("hidden")}openVolumeControls(){this.volumeControls.classList.remove("hidden")}async downloadSwf(){try{if(this.swfUrl){console.log("Downloading SWF: "+this.swfUrl);let A=await fetch(this.swfUrl.href);if(!A.ok){console.error("SWF download failed");return}let e=await A.blob();Zj(e,Ar(this.swfUrl))}else console.error("SWF download failed")}catch{console.error("SWF download failed")}}virtualKeyboardInput(){let A=this.virtualKeyboard,e=A.value;for(let j of e)for(let a of["keydown","keyup"])this.element.dispatchEvent(new KeyboardEvent(a,{key:j,bubbles:!0}));A.value=""}openVirtualKeyboard(){this.instance?.has_focus()?this.virtualKeyboard.focus({preventScroll:!0}):setTimeout(()=>{this.virtualKeyboard.focus({preventScroll:!0})},0)}closeVirtualKeyboard(){this.isVirtualKeyboardFocused()&&this.container.focus({preventScroll:!0})}isVirtualKeyboardFocused(){return this.shadow.activeElement===this.virtualKeyboard}contextMenuItems(){let A=[],e=()=>{A.length>0&&A[A.length-1]!==null&&A.push(null)};return this.instance&&this.isPlaying&&(this.instance.prepare_context_menu().forEach((a,t)=>{a.separatorBefore&&e(),A.push({text:a.caption,onClick:async()=>this.instance?.run_context_menu_callback(t),enabled:a.enabled,checked:a.checked})}),e()),this.fullscreenEnabled&&(this.isFullscreen?A.push({text:F("context-menu-exit-fullscreen"),onClick:async()=>this.setFullscreen(!1)}):A.push({text:F("context-menu-enter-fullscreen"),onClick:async()=>this.setFullscreen(!0)})),A.push({text:F("context-menu-volume-controls"),onClick:async()=>{this.openVolumeControls()}}),this.instance&&this.swfUrl&&this.loadedConfig&&this.loadedConfig.showSwfDownload===!0&&(e(),A.push({text:F("context-menu-download-swf"),onClick:this.downloadSwf.bind(this)})),navigator.clipboard&&window.isSecureContext&&A.push({text:F("context-menu-copy-debug-info"),onClick:()=>navigator.clipboard.writeText(this.getPanicData())}),this.checkSaves()&&A.push({text:F("context-menu-open-save-manager"),onClick:this.openSaveManager.bind(this)}),e(),A.push({text:F("context-menu-about-ruffle",{flavor:AA?"extension":"",version:U.versionName}),async onClick(){window.open(kA,"_blank")}}),this.isTouch&&(e(),A.push({text:F("context-menu-hide"),onClick:async()=>{this.contextMenuForceDisabled=!0}})),A}pointerDown(A){this.pointerDownPosition=new aj(A.pageX,A.pageY),this.pointerMoveMaxDistance=0,this.startLongPressTimer()}clearLongPressTimer(){this.longPressTimer&&(clearTimeout(this.longPressTimer),this.longPressTimer=null)}startLongPressTimer(){this.clearLongPressTimer(),this.longPressTimer=setTimeout(()=>this.clearLongPressTimer(),800)}checkLongPressMovement(A){if(this.pointerDownPosition!==null){let e=new aj(A.pageX,A.pageY),j=this.pointerDownPosition.distanceTo(e);j>this.pointerMoveMaxDistance&&(this.pointerMoveMaxDistance=j)}}checkLongPress(A){this.longPressTimer?this.clearLongPressTimer():!this.contextMenuSupported&&A.pointerType!=="mouse"&&this.pointerMoveMaxDistance<15&&this.showContextMenu(A)}suppressContextMenu(){this._suppressContextMenu=!0}showContextMenu(A){if(this.panicked)return;if(A.type==="contextmenu"&&A.shiftKey){this.hideContextMenu();return}if(A.preventDefault(),this._suppressContextMenu){this._suppressContextMenu=!1;return}if(this.shadow.querySelectorAll(".modal:not(.hidden)").length!==0||(A.type==="contextmenu"?(this.contextMenuSupported=!0,document.documentElement.addEventListener("click",this.hideContextMenu.bind(this),{once:!0})):(document.documentElement.addEventListener("pointerup",this.hideContextMenu.bind(this),{once:!0}),A.stopPropagation()),[!1,dA.Off].includes(this.loadedConfig?.contextMenu??dA.On)||this.isTouch&&this.loadedConfig?.contextMenu===dA.RightClickOnly||this.contextMenuForceDisabled))return;for(;this.contextMenuElement.firstChild;)this.contextMenuElement.removeChild(this.contextMenuElement.firstChild);let e=this.contextMenuItems(),j=e.some(BA=>BA!==null&&BA.checked!==void 0);this.contextMenuElement.classList.toggle("has-checkmarks",j);for(let BA of e)if(BA===null)this.contextMenuElement.appendChild((0,D.jsx)("li",{class:"menu-separator",children:(0,D.jsx)("hr",{})}));else{let{text:xe,onClick:Ie,enabled:YA,checked:dj}=BA,OA=(0,D.jsx)("li",{class:{"menu-item":!0,disabled:YA===!1,checked:dj===!0},"data-text":xe,children:xe});if(this.contextMenuElement.appendChild(OA),YA!==!1){let CA=async hA=>{hA.preventDefault(),hA.stopPropagation(),await Ie(hA),this.hideContextMenu()};this.contextMenuSupported?(OA.addEventListener("click",CA),OA.addEventListener("contextmenu",CA)):OA.addEventListener("pointerup",CA)}}this.contextMenuOverlay.classList.remove("hidden");let a=this.element.getBoundingClientRect(),t=this.contextMenuElement.getBoundingClientRect(),o=document.scrollingElement||document.body,f=t.width,s=t.height,i=o.clientWidth,b=o.clientHeight,E=A.clientX;E+f>i&&(E=A.clientX-f>=0?A.clientX-f:i-f);let h=A.clientY;h+s>b&&(h=A.clientY-s>=0?A.clientY-s:b-s);let T=E-a.x,Q=h-a.y,fA=getComputedStyle(this.contextMenuElement).direction==="rtl";this.contextMenuElement.style.top=`${Q}px`,fA?(this.contextMenuElement.style.right=`${a.width-T}px`,this.contextMenuElement.style.left=""):(this.contextMenuElement.style.right="",this.contextMenuElement.style.left=`${T}px`)}hideContextMenu(){this.instance?.clear_custom_menu_items(),this.contextMenuOverlay.classList.add("hidden")}pause(){this.instance&&(this.instance.pause(),this.playButton.style.display="block")}audioState(){if(this.instance){let A=this.instance.audio_context();return A&&A.state||"running"}return"suspended"}unmuteOverlayClicked(){if(this.instance){if(this.audioState()!=="running"){let A=this.instance.audio_context();A&&A.resume()}this.unmuteOverlay.style.display="none"}}unmuteAudioContext(){if(!ej){if(navigator.maxTouchPoints<1){ej=!0;return}"audioSession"in navigator?navigator.audioSession.type="playback":this.container.addEventListener("click",()=>{if(ej)return;let A=this.instance?.audio_context();if(!A)return;let e=new Audio;e.src=(()=>{let j=new ArrayBuffer(10),a=new DataView(j),t=A.sampleRate;return a.setUint32(0,t,!0),a.setUint32(4,t,!0),a.setUint16(8,1,!0),`data:audio/wav;base64,UklGRisAAABXQVZFZm10IBAAAAABAAEA${window.btoa(String.fromCharCode(...new Uint8Array(j))).slice(0,13)}AgAZGF0YQcAAACAgICAgICAAAA=`})(),e.load(),e.play().then(()=>{ej=!0}).catch(j=>{console.warn(`Failed to play dummy sound: ${j}`)})},{once:!0})}}static htmlDimensionToCssDimension(A){if(A){let e=A.match(ko);if(e){let j=e[1];return e[3]||(j+="px"),j}}return null}callExternalInterface(A,e){return this.instance?.call_exposed_callback(A,e)}getObjectId(){return this.element.getAttribute("name")}set traceObserver(A){this.instance?.set_trace_observer(A)}getPanicData(){let A=`
# Player Info
`;if(A+=`Allows script access: ${this.loadedConfig?this.loadedConfig.allowScriptAccess:!1}
`,A+=`${this.rendererDebugInfo}
`,A+=this.debugPlayerInfo(),A+=`
# Page Info
`,A+=`Page URL: ${document.location.href}
`,this.swfUrl&&(A+=`SWF URL: ${this.swfUrl}
`),A+=`
# Browser Info
`,A+=`User Agent: ${window.navigator.userAgent}
`,A+=`Platform: ${window.navigator.platform}
`,A+=`Has touch support: ${window.navigator.maxTouchPoints>0}
`,A+=`
# Ruffle Info
`,A+=`Version: ${U.versionNumber}
`,A+=`Name: ${U.versionName}
`,A+=`Channel: ${U.versionChannel}
`,A+=`Built: ${U.buildDate}
`,A+=`Commit: ${U.commitHash}
`,A+=`Is extension: ${AA}
`,A+=`
# Metadata
`,this.metadata)for(let[e,j]of Object.entries(this.metadata))A+=`${e}: ${j}
`;return A}panic(A){if(this.panicked)return;this.panicked=!0,this.hideSplashScreen();let e=A;if(A instanceof Error&&(A.name==="AbortError"||A.message.includes("AbortError")))return;if(A instanceof FA){let a=this.loadedConfig?.openInNewTab,t=this.loadedConfig&&"url"in this.loadedConfig?new URL(this.loadedConfig.url,document.baseURI):void 0;if(a&&t){this.addOpenInNewTabMessage(a,t);return}A=A.cause}let j=Object.assign([],{stackIndex:-1,avmStackIndex:-1});if(j.push(`# Error Info
`),A instanceof Error){if(j.push(`Error name: ${A.name}
`),j.push(`Error message: ${A.message}
`),A.stack){let a=j.push(`Error stack:
\`\`\`
${A.stack}
\`\`\`
`)-1;A.avmStack&&(j.avmStackIndex=j.push(`AVM2 stack:
\`\`\`
    ${A.avmStack.trim().replace(/\t/g,"    ")}
\`\`\`
`)-1),j.stackIndex=a}}else j.push(`Error: ${A}
`);j.push(this.getPanicData()),cr(this.container,e,j,this.swfUrl),this.destroy()}addOpenInNewTabMessage(A,e){let j=new URL(e);if(this.loadedConfig?.parameters){let t=Yj(this.loadedConfig?.parameters);Object.entries(t).forEach(([o,f])=>{j.searchParams.set(o,f)})}this.hideSplashScreen();let a=(0,D.jsxs)("div",{children:[C("message-cant-embed"),(0,D.jsx)("div",{children:(0,D.jsx)("a",{href:"#",onClick:()=>A(j),children:F("open-in-new-tab")})})]});this.displayMessageOrElement(a,!0)}displayRootMovieDownloadFailedMessage(A,e){let j=this.loadedConfig?.openInNewTab;if(j&&this.swfUrl&&window.location.origin!==this.swfUrl.origin)this.addOpenInNewTabMessage(j,this.swfUrl);else{let a=e.includes("HTTP Status is not OK:"),t=A?new UA(this.swfUrl):new LA(this.swfUrl,a);this.panic(t)}}displayMessageOrElement(A,e){let j=A instanceof HTMLDivElement?A:(0,D.jsx)("p",{children:A}),a=e?null:(0,D.jsx)("div",{children:(0,D.jsx)("button",{id:"continue-btn",children:F("continue")})}),t=(0,D.jsx)("div",{id:"message-overlay",children:(0,D.jsxs)("div",{class:"message",children:[j,a]})});if(this.container.prepend(t),!e){let o=this.container.querySelector("#continue-btn");o.onclick=()=>{t.parentNode.removeChild(t)}}}displayMessage(A){this.displayMessageOrElement(A)}displayRestoredFromBfcacheMessage(){if(this.container.querySelector("#message-overlay")!==null)return;let A=C("message-restored-from-bfcache");this.displayMessageOrElement(A);let e=this.container.querySelector("#message-overlay");(e.scrollWidth>e.offsetWidth||e.scrollHeight>e.offsetHeight)&&e.parentNode.removeChild(e)}displayUnsupportedVideo(A){let e=this.videoModal.querySelector("#video-holder");if(e){let j=(0,D.jsx)("video",{src:A,autoplay:!0,controls:!0,onContextMenu:a=>a.stopPropagation()});e.textContent="",e.appendChild(j),this.videoModal.classList.remove("hidden")}}displayClipboardModal(A){let e=this.clipboardModal.querySelector("#clipboard-modal-description");e&&(e.textContent=F("clipboard-message-description",{variant:A?"access-denied":"unsupported"}),this.clipboardModal.classList.remove("hidden"))}hideSplashScreen(){this.splashScreen.classList.add("hidden"),this.container.classList.remove("hidden")}showSplashScreen(){this.splashScreen.classList.remove("hidden"),this.container.classList.add("hidden")}setMetadata(A){this.metadata=A,this._readyState=lA.Loaded,this.hideSplashScreen(),this.element.dispatchEvent(new CustomEvent(r.LOADED_METADATA)),this.element.dispatchEvent(new CustomEvent(r.LOADED_DATA))}};QA.LOADED_METADATA="loadedmetadata";QA.LOADED_DATA="loadeddata";var Aa=class{constructor(A,e){this.isMuted=A,this.volume=e}get_volume(){return this.isMuted?0:this.volume/100}};function Be(r,A){let e={url:r},j=A("allowNetworking");j!==null&&(e.allowNetworking=j);let a=Dr(A("allowScriptAccess"),r);a!==null&&(e.allowScriptAccess=a);let t=A("bgcolor");t!==null&&(e.backgroundColor=t);let o=A("base");if(o!==null)if(o==="."){let fA=new URL(r,document.baseURI);e.base=new URL(o,fA).href}else e.base=o;let f=xr(A("menu"));f!==null&&(e.menu=f);let s=xr(A("allowFullScreen"));s!==null&&(e.allowFullscreen=s);let i=A("flashvars");i!==null&&(e.parameters=i);let b=A("quality");b!==null&&(e.quality=b);let E=A("salign");E!==null&&(e.salign=E);let h=A("scale");h!==null&&(e.scale=h);let T=A("wmode");T!==null&&(e.wmode=T);let Q=A("fullScreenAspectRatio");return Q!==null&&(e.fullScreenAspectRatio=Q),e}function he(r){if(r){let A="",e="";try{let j=new URL(r,kA);A=j.pathname,e=j.hostname}catch{}if(A.startsWith("/v/")&&/^(?:www\.|m\.)?youtube(?:-nocookie)?\.com|youtu\.be$/i.test(e))return!0}return!1}function qe(r,A){let e=r.getAttribute(A),j=window.RufflePlayer?.config??{};if(e)try{let a=new URL(e);a.protocol==="http:"&&window.location.protocol==="https:"&&(!("upgradeToHttps"in j)||j.upgradeToHttps!==!1)&&(a.protocol="https:",r.setAttribute(A,a.toString()))}catch{}}function Ge(r){let A=r.parentElement;for(;A!==null;){switch(A.tagName){case"AUDIO":case"VIDEO":return!0}A=A.parentElement}return!1}function Zj(r,A){let e=URL.createObjectURL(r),j=document.createElement("a");j.href=e,j.download=A,j.click(),URL.revokeObjectURL(e)}function Ir(r){let A=atob(r);return Uint8Array.from(A,e=>e.charCodeAt(0))}function Fo(r,A){let e=Ir(r);return new Blob([e],{type:A})}function jj(r){try{let A=atob(r);return $o(A)}catch{return!1}}function $o(r){return r.charCodeAt(0)===0&&r.charCodeAt(1)===191&&r.slice(6,10)==="TCSO"&&[0,4,0,0,0,0].every((A,e)=>r.charCodeAt(10+e)===A)}function xr(r){switch(r?.toLowerCase()){case"true":return!0;case"false":return!1;default:return null}}function Dr(r,A){switch(r?.toLowerCase()){case"always":return!0;case"never":return!1;case"samedomain":try{return new URL(window.location.href).origin===new URL(A,window.location.href).origin}catch{return!1}default:return null}}function yo(){let r=new Intl.Locale(navigator.language),A;if("getTextInfo"in r&&typeof r.getTextInfo=="function")A=r.getTextInfo();else if("textInfo"in r&&typeof r.textInfo=="object")A=r.textInfo;else return"ltr";return typeof A=="object"&&"direction"in A&&typeof A.direction=="string"&&A.direction||"ltr"}l();var wo=function(r,A,e,j,a){if(j==="m")throw new TypeError("Private method is not writable");if(j==="a"&&!a)throw new TypeError("Private accessor was defined without a setter");if(typeof A=="function"?r!==A||!a:!A.has(r))throw new TypeError("Cannot write private member to an object whose class did not declare it");return j==="a"?a.call(r,e):a?a.value=e:A.set(r,e),e},P=function(r,A,e,j){if(e==="a"&&!j)throw new TypeError("Private accessor was defined without a getter");if(typeof A=="function"?r!==A||!j:!A.has(r))throw new TypeError("Cannot read private member from an object whose class did not declare it");return e==="m"?j:e==="a"?j.call(r):j?j.value:A.get(r)},x,rj=class{constructor(A){x.set(this,void 0),wo(this,x,A,"f")}addFSCommandHandler(A){P(this,x,"f").addFSCommandHandler(A)}get readyState(){return P(this,x,"f")._readyState}get metadata(){return P(this,x,"f").metadata}get loadedConfig(){return P(this,x,"f").loadedConfig??null}async reload(){await P(this,x,"f").reload()}async load(A,e=!1){await P(this,x,"f").load(A,e)}resume(){P(this,x,"f").play()}get isPlaying(){return P(this,x,"f").isPlaying}get volume(){return P(this,x,"f").volume}set volume(A){P(this,x,"f").volume=A}get fullscreenEnabled(){return P(this,x,"f").fullscreenEnabled}get isFullscreen(){return P(this,x,"f").isFullscreen}setFullscreen(A){P(this,x,"f").setFullscreen(A)}requestFullscreen(){P(this,x,"f").enterFullscreen()}exitFullscreen(){P(this,x,"f").exitFullscreen()}async downloadSwf(){await P(this,x,"f").downloadSwf()}displayMessage(A){P(this,x,"f").displayMessage(A)}suspend(){P(this,x,"f").pause()}get suspended(){return!P(this,x,"f").isPlaying}set traceObserver(A){P(this,x,"f").traceObserver=A}get config(){return P(this,x,"f").config}set config(A){P(this,x,"f").config=A}callExternalInterface(A,...e){return P(this,x,"f").callExternalInterface(A,e)}};x=new WeakMap;var $=function(r,A,e,j){if(e==="a"&&!j)throw new TypeError("Private accessor was defined without a getter");if(typeof A=="function"?r!==A||!j:!A.has(r))throw new TypeError("Cannot read private member from an object whose class did not declare it");return e==="m"?j:e==="a"?j.call(r):j?j.value:A.get(r)},Or=function(r,A,e,j,a){if(j==="m")throw new TypeError("Private method is not writable");if(j==="a"&&!a)throw new TypeError("Private accessor was defined without a setter");if(typeof A=="function"?r!==A||!a:!A.has(r))throw new TypeError("Cannot write private member to an object whose class did not declare it");return j==="a"?a.call(r,e):a?a.value=e:A.set(r,e),e},k,ve,nA=class r extends HTMLElement{get onFSCommand(){return $(this,ve,"f")}set onFSCommand(A){Or(this,ve,A,"f")}get readyState(){return $(this,k,"f")._readyState}get metadata(){return $(this,k,"f").metadata}constructor(){super(),k.set(this,void 0),ve.set(this,null),Or(this,k,new QA(this,()=>this.debugPlayerInfo(),A=>{try{Object.defineProperty(this,A,{value:(...e)=>$(this,k,"f").callExternalInterface(A,e),configurable:!0})}catch(e){console.warn(`Error setting ExternalInterface legacy callback for ${A}`,e)}}),"f"),$(this,k,"f").addFSCommandHandler((A,e)=>{$(this,ve,"f")?.call(this,A,e)})}ruffle(A){if((A??1)===1)return new rj($(this,k,"f"));throw new Error(`Version ${A} not supported.`)}get loadedConfig(){return $(this,k,"f").loadedConfig??null}connectedCallback(){$(this,k,"f").updateStyles()}static get observedAttributes(){return["width","height","align"]}attributeChangedCallback(A,e,j){r.observedAttributes.includes(A)&&$(this,k,"f").updateStyles()}disconnectedCallback(){$(this,k,"f").destroy()}async reload(){await $(this,k,"f").reload()}async load(A,e=!1){await $(this,k,"f").load(A,e)}play(){$(this,k,"f").play()}get isPlaying(){return $(this,k,"f").isPlaying}get volume(){return $(this,k,"f").volume}set volume(A){$(this,k,"f").volume=A}get fullscreenEnabled(){return $(this,k,"f").fullscreenEnabled}get isFullscreen(){return $(this,k,"f").isFullscreen}setFullscreen(A){$(this,k,"f").setFullscreen(A)}enterFullscreen(){$(this,k,"f").enterFullscreen()}exitFullscreen(){$(this,k,"f").exitFullscreen()}async downloadSwf(){await $(this,k,"f").downloadSwf()}pause(){$(this,k,"f").pause()}set traceObserver(A){$(this,k,"f").traceObserver=A}debugPlayerInfo(){return""}PercentLoaded(){return $(this,k,"f")._readyState===lA.Loaded?100:0}get config(){return $(this,k,"f").config}set config(A){$(this,k,"f").config=A}displayMessage(A){$(this,k,"f").displayMessage(A)}};k=new WeakMap,ve=new WeakMap;function tj(r,A){if(r){for(let e of r.attributes)if(e.specified){if(e.name==="title"&&e.value==="Adobe Flash Player")continue;try{A.setAttribute(e.name,e.value)}catch{console.warn(`Unable to set attribute ${e.name} on Ruffle instance`)}}for(let e of Array.from(r.children))A.appendChild(e)}}l();var pA=class r extends nA{connectedCallback(){super.connectedCallback();let A=this.attributes.getNamedItem("src");if(A){let e=a=>this.attributes.getNamedItem(a)?.value??null,j=Be(A.value,e);this.load(j,!0)}}get nodeName(){return"EMBED"}get src(){return this.attributes.getNamedItem("src")?.value}set src(A){if(A){let e=document.createAttribute("src");e.value=A,this.attributes.setNamedItem(e)}else this.attributes.removeNamedItem("src")}static get observedAttributes(){return[...nA.observedAttributes,"src"]}attributeChangedCallback(A,e,j){if(super.attributeChangedCallback(A,e,j),this.isConnected&&A==="src"){let a=this.attributes.getNamedItem("src");if(a){let t=f=>this.attributes.getNamedItem(f)?.value??null,o=Be(a.value,t);this.load(o,!0)}}}static isInterdictable(A){let e=A.getAttribute("src"),j=A.getAttribute("type");return!e||Ge(A)?!1:he(e)?(qe(A,"src"),!1):Ne(e,j)}static fromNativeEmbedElement(A){let e=JA("ruffle-embed",r),j=document.createElement(e);return tj(A,j),j}get height(){return this.getAttribute("height")||""}set height(A){this.setAttribute("height",A)}get width(){return this.getAttribute("width")||""}set width(A){this.setAttribute("width",A)}get type(){return this.getAttribute("type")||""}set type(A){this.setAttribute("type",A)}};function xo(r,A,e){A=A.toLowerCase();for(let[j,a]of Object.entries(r))if(j.toLowerCase()===A)return a;return e}function Cr(r){let A={};for(let e of r.children)if(e instanceof HTMLParamElement){let j=e.attributes.getNamedItem("name")?.value,a=e.attributes.getNamedItem("value")?.value;j&&a&&(A[j]=a)}return A}var ke=class r extends nA{constructor(){super(...arguments),this.params={}}connectedCallback(){super.connectedCallback(),this.params=Cr(this);let A=null;if(this.attributes.getNamedItem("data")?A=this.attributes.getNamedItem("data")?.value:this.params.movie&&(A=this.params.movie),A){let e=["allowNetworking","base","bgcolor","flashvars"],a=Be(A,t=>xo(this.params,t,e.includes(t)?this.getAttribute(t):null));this.load(a,!0)}}debugPlayerInfo(){let A=`Player type: Object
`,e=null;return this.attributes.getNamedItem("data")?e=this.attributes.getNamedItem("data")?.value:this.params.movie&&(e=this.params.movie),A+=`SWF URL: ${e}
`,Object.keys(this.params).forEach(j=>{A+=`Param ${j}: ${this.params[j]}
`}),Object.keys(this.attributes).forEach(j=>{A+=`Attribute ${j}: ${this.attributes.getNamedItem(j)?.value}
`}),A}get nodeName(){return"OBJECT"}get data(){return this.getAttribute("data")}set data(A){if(A){let e=document.createAttribute("data");e.value=A,this.attributes.setNamedItem(e)}else this.attributes.removeNamedItem("data")}static isInterdictable(A){if(Ge(A)||A.getElementsByTagName("ruffle-object").length>0||A.getElementsByTagName("ruffle-embed").length>0)return!1;let e=A.attributes.getNamedItem("data")?.value.toLowerCase(),j=A.attributes.getNamedItem("type")?.value??null,a=Cr(A),t;if(e){if(he(e))return qe(A,"data"),!1;t=e}else if(a&&a.movie){if(he(a.movie)){let f=A.querySelector("param[name='movie']");if(f){qe(f,"value");let s=f.getAttribute("value");s&&A.setAttribute("data",s)}return!1}t=a.movie}else return!1;let o=A.attributes.getNamedItem("classid")?.value.toLowerCase();return o===Za.toLowerCase()?!Array.from(A.getElementsByTagName("object")).some(r.isInterdictable)&&!Array.from(A.getElementsByTagName("embed")).some(pA.isInterdictable):o?!1:Ne(t,j)}static fromNativeObjectElement(A){let e=JA("ruffle-object",r),j=document.createElement(e);for(let a of Array.from(A.getElementsByTagName("embed")))pA.isInterdictable(a)&&a.remove();for(let a of Array.from(A.getElementsByTagName("object")))r.isInterdictable(a)&&a.remove();return tj(A,j),j}get height(){return this.getAttribute("height")||""}set height(A){this.setAttribute("height",A)}get width(){return this.getAttribute("width")||""}set width(A){this.setAttribute("width",A)}get type(){return this.getAttribute("type")||""}set type(A){this.setAttribute("type",A)}};l();var gA=function(r,A,e,j,a){if(j==="m")throw new TypeError("Private method is not writable");if(j==="a"&&!a)throw new TypeError("Private accessor was defined without a setter");if(typeof A=="function"?r!==A||!a:!A.has(r))throw new TypeError("Cannot write private member to an object whose class did not declare it");return j==="a"?a.call(r,e):a?a.value=e:A.set(r,e),e},H=function(r,A,e,j){if(e==="a"&&!j)throw new TypeError("Private accessor was defined without a getter");if(typeof A=="function"?r!==A||!j:!A.has(r))throw new TypeError("Cannot read private member from an object whose class did not declare it");return e==="m"?j:e==="a"?j.call(r):j?j.value:A.get(r)},EA,Fe,DA,oj,cj,fj,mA,$e,ye=class{constructor(A){if(EA.set(this,void 0),Fe.set(this,void 0),gA(this,EA,[],"f"),gA(this,Fe,{},"f"),A)for(let e=0;e<A.length;e++)this.install(A[e])}install(A){let e=new nj(A),j=H(this,EA,"f").length;H(this,EA,"f").push(e),H(this,Fe,"f")[A.type]=e,Object.defineProperty(this,e.type,{configurable:!0,enumerable:!1,value:e}),this[j]=e}item(A){return H(this,EA,"f")[A>>>0]}namedItem(A){return H(this,Fe,"f")[A]}get length(){return H(this,EA,"f").length}[(EA=new WeakMap,Fe=new WeakMap,Symbol.iterator)](){return H(this,EA,"f")[Symbol.iterator]()}get[Symbol.toStringTag](){return"MimeTypeArray"}},nj=class{constructor(A){DA.set(this,void 0),gA(this,DA,A,"f")}get type(){return H(this,DA,"f").type}get description(){return H(this,DA,"f").description}get suffixes(){return H(this,DA,"f").suffixes}get enabledPlugin(){return H(this,DA,"f").enabledPlugin}get[(DA=new WeakMap,Symbol.toStringTag)](){return"MimeType"}},ea=class extends ye{constructor(A,e,j){super(),oj.set(this,void 0),cj.set(this,void 0),fj.set(this,void 0),gA(this,oj,A,"f"),gA(this,cj,e,"f"),gA(this,fj,j,"f")}get name(){return H(this,oj,"f")}get description(){return H(this,cj,"f")}get filename(){return H(this,fj,"f")}get[(oj=new WeakMap,cj=new WeakMap,fj=new WeakMap,Symbol.toStringTag)](){return"Plugin"}},sj=class{constructor(A){mA.set(this,void 0),$e.set(this,void 0),gA(this,mA,[],"f"),gA(this,$e,{},"f");for(let e=0;e<A.length;e++)this.install(A[e])}install(A){let e=H(this,mA,"f").length;H(this,mA,"f").push(A),H(this,$e,"f")[A.name]=A,Object.defineProperty(this,A.name,{configurable:!0,enumerable:!1,value:A}),this[e]=A}item(A){return H(this,mA,"f")[A>>>0]}namedItem(A){return H(this,$e,"f")[A]}refresh(){}[(mA=new WeakMap,$e=new WeakMap,Symbol.iterator)](){return H(this,mA,"f")[Symbol.iterator]()}get[Symbol.toStringTag](){return"PluginArray"}get length(){return H(this,mA,"f").length}},cA=new ea("Shockwave Flash","Shockwave Flash 32.0 r0","ruffle.js");cA.install({type:Xe,description:"Shockwave Flash",suffixes:"spl",enabledPlugin:cA});cA.install({type:Ve,description:"Shockwave Flash",suffixes:"swf",enabledPlugin:cA});cA.install({type:ze,description:"Shockwave Flash",suffixes:"swf",enabledPlugin:cA});cA.install({type:We,description:"Shockwave Flash",suffixes:"swf",enabledPlugin:cA});function Pr(r){if(navigator.plugins.namedItem("Shockwave Flash"))return;(!("install"in navigator.plugins)||!navigator.plugins.install)&&(Object.defineProperty(window,"PluginArray",{value:sj,configurable:!0}),Object.defineProperty(navigator,"plugins",{value:new sj(navigator.plugins),writable:!1,configurable:!0})),navigator.plugins.install(r),r.length>0&&(!("install"in navigator.mimeTypes)||!navigator.mimeTypes.install)&&(Object.defineProperty(window,"MimeTypeArray",{value:ye,configurable:!0}),Object.defineProperty(window,"MimeType",{value:nj,configurable:!0}),Object.defineProperty(navigator,"mimeTypes",{value:new ye(navigator.mimeTypes),writable:!1,configurable:!0}));let e=navigator.mimeTypes;for(let j=0;j<r.length;j+=1)e.install(r[j])}var we=window.RufflePlayer?.config??{},Io=Ye(we)+"ruffle.js",ja,aa;function Do(){return"favorFlash"in we&&we.favorFlash===!1?!1:(navigator.plugins.namedItem("Shockwave Flash")?.filename??"ruffle.js")!=="ruffle.js"}function Hr(){try{ja=ja??document.getElementsByTagName("object"),aa=aa??document.getElementsByTagName("embed");for(let r of Array.from(ja))if(ke.isInterdictable(r)){let A=ke.fromNativeObjectElement(r);r.replaceWith(A)}for(let r of Array.from(aa))if(pA.isInterdictable(r)){let A=pA.fromNativeEmbedElement(r);r.replaceWith(A)}}catch(r){console.error(`Serious error encountered when polyfilling native Flash elements: ${r}`)}}var ra,ta;function Tr(){ra=ra??document.getElementsByTagName("iframe"),ta=ta??document.getElementsByTagName("frame"),[ra,ta].forEach(r=>{for(let A of r){if(A.dataset.rufflePolyfilled!==void 0)continue;A.dataset.rufflePolyfilled="";let e=A.contentWindow,j=`Couldn't load Ruffle into ${A.tagName}[${A.src}]: `;try{e.document.readyState==="complete"&&Rr(e,j)}catch(a){AA||console.warn(j+a)}A.addEventListener("load",()=>{Rr(e,j)},!1)}})}async function Rr(r,A){await new Promise(j=>{window.setTimeout(()=>{j()},100)});let e;try{if(e=r.document,!e)return}catch(j){AA||console.warn(A+j);return}if(!(!AA&&e.documentElement.dataset.ruffleOptout!==void 0)){if(AA)r.RufflePlayer||(r.RufflePlayer={}),r.RufflePlayer.config={...we,...r.RufflePlayer.config??{}};else if(!r.RufflePlayer){let j=e.createElement("script");j.setAttribute("src",Io),j.onload=()=>{r.RufflePlayer={},r.RufflePlayer.config=we},e.head.appendChild(j)}}}function Oo(){new MutationObserver(function(A){A.some(j=>Array.from(j.addedNodes).some(a=>["EMBED","OBJECT"].includes(a.nodeName)||a instanceof Element&&a.querySelector("embed, object")!==null))&&(Hr(),Tr())}).observe(document,{childList:!0,subtree:!0})}function Sr(){Pr(cA)}function Kr(){Do()||(Hr(),Tr(),Oo())}var IA={version:U.versionNumber+"+"+U.buildDate.substring(0,10),polyfill(){Kr()},pluginPolyfill(){Sr()},createPlayer(){let r=JA("ruffle-player",nA);return document.createElement(r)},options:{}};function Co(r,A={}){let e;window.RufflePlayer instanceof RA?e=window.RufflePlayer:(e=new RA(window.RufflePlayer),window.RufflePlayer=e),e.sources[r]=IA,IA.options=A,("polyfills"in e.config?e.config.polyfills:!0)!==!1&&IA.pluginPolyfill()}lj.installRuffle("local");})();
//# sourceMappingURL=ruffle.js.map
