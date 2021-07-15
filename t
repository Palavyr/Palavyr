[1mdiff --git a/frontend/package-lock.json b/frontend/package-lock.json[m
[1mindex 66a5ebd9..07474cda 100644[m
[1m--- a/frontend/package-lock.json[m
[1m+++ b/frontend/package-lock.json[m
[36m@@ -42,6 +42,7 @@[m
         "highlight.js": "^10.1.2",[m
         "jquery": "^3.5.1",[m
         "js-cookie": "^2.2.1",[m
[32m+[m[32m        "jsonwebtoken": "^8.5.1",[m
         "less": "^3.12.2",[m
         "lodash": "^4.17.19",[m
         "material-ui-dropzone": "^3.3.1",[m
[36m@@ -63,6 +64,7 @@[m
         "react-scroll": "^1.8.0",[m
         "react-spring": "^8.0.27",[m
         "react-stripe-checkout": "^2.6.3",[m
[32m+[m[32m        "react-tracking": "^8.1.0",[m
         "sass": "^1.26.10",[m
         "uuid": "^8.3.2"[m
       },[m
[36m@@ -82,6 +84,7 @@[m
         "@testing-library/user-event": "^7.1.2",[m
         "@types/aos": "^3.0.3",[m
         "@types/jest": "^25.2.3",[m
[32m+[m[32m        "@types/jsonwebtoken": "^8.5.4",[m
         "@types/react-color": "^3.0.4",[m
         "@types/stripe": "^7.13.25",[m
         "@typescript-eslint/eslint-plugin": "^2.10.0",[m
[36m@@ -13556,6 +13559,15 @@[m
       "integrity": "sha1-7ihweulOEdK4J7y+UnC86n8+ce4=",[m
       "dev": true[m
     },[m
[32m+[m[32m    "node_modules/@types/jsonwebtoken": {[m
[32m+[m[32m      "version": "8.5.4",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/@types/jsonwebtoken/-/jsonwebtoken-8.5.4.tgz",[m
[32m+[m[32m      "integrity": "sha512-4L8msWK31oXwdtC81RmRBAULd0ShnAHjBuKT9MRQpjP0piNrZdXyTRcKY9/UIfhGeKIT4PvF5amOOUbbT/9Wpg==",[m
[32m+[m[32m      "dev": true,[m
[32m+[m[32m      "dependencies": {[m
[32m+[m[32m        "@types/node": "*"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "node_modules/@types/lodash": {[m
       "version": "4.14.161",[m
       "resolved": "https://registry.npmjs.org/@types/lodash/-/lodash-4.14.161.tgz",[m
[36m@@ -18599,6 +18611,11 @@[m
         "isarray": "^1.0.0"[m
       }[m
     },[m
[32m+[m[32m    "node_modules/buffer-equal-constant-time": {[m
[32m+[m[32m      "version": "1.0.1",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/buffer-equal-constant-time/-/buffer-equal-constant-time-1.0.1.tgz",[m
[32m+[m[32m      "integrity": "sha1-+OcRMvf/5uAaXJaXpMbz5I1cyBk="[m
[32m+[m[32m    },[m
     "node_modules/buffer-from": {[m
       "version": "1.1.1",[m
       "resolved": "https://registry.npmjs.org/buffer-from/-/buffer-from-1.1.1.tgz",[m
[36m@@ -20999,7 +21016,6 @@[m
       "version": "4.2.2",[m
       "resolved": "https://registry.npmjs.org/deepmerge/-/deepmerge-4.2.2.tgz",[m
       "integrity": "sha512-FJ3UgI4gIl+PHZm53knsuSFpE+nESMr7M4v9QcgB7S63Kj/6WqMiFQJpBBYz1Pt+66bZpP3Q7Lye0Oo9MPKEdg==",[m
[31m-      "dev": true,[m
       "engines": {[m
         "node": ">=0.10.0"[m
       }[m
[36m@@ -21612,6 +21628,14 @@[m
         "safer-buffer": "^2.1.0"[m
       }[m
     },[m
[32m+[m[32m    "node_modules/ecdsa-sig-formatter": {[m
[32m+[m[32m      "version": "1.0.11",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/ecdsa-sig-formatter/-/ecdsa-sig-formatter-1.0.11.tgz",[m
[32m+[m[32m      "integrity": "sha512-nagl3RYrbNv6kQkeJIpt6NJZy8twLB/2vtz6yN9Z4vRKHN4/QZJIEbqohALSgwKdnksuY3k5Addp5lg8sVoVcQ==",[m
[32m+[m[32m      "dependencies": {[m
[32m+[m[32m        "safe-buffer": "^5.0.1"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "node_modules/editions": {[m
       "version": "2.3.1",[m
       "resolved": "https://registry.npmjs.org/editions/-/editions-2.3.1.tgz",[m
[36m@@ -28801,6 +28825,40 @@[m
       "integrity": "sha1-LHS27kHZPKUbe1qu6PUDYx0lKnM=",[m
       "dev": true[m
     },[m
[32m+[m[32m    "node_modules/jsonwebtoken": {[m
[32m+[m[32m      "version": "8.5.1",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/jsonwebtoken/-/jsonwebtoken-8.5.1.tgz",[m
[32m+[m[32m      "integrity": "sha512-XjwVfRS6jTMsqYs0EsuJ4LGxXV14zQybNd4L2r0UvbVnSF9Af8x7p5MzbJ90Ioz/9TI41/hTCvznF/loiSzn8w==",[m
[32m+[m[32m      "dependencies": {[m
[32m+[m[32m        "jws": "^3.2.2",[m
[32m+[m[32m        "lodash.includes": "^4.3.0",[m
[32m+[m[32m        "lodash.isboolean": "^3.0.3",[m
[32m+[m[32m        "lodash.isinteger": "^4.0.4",[m
[32m+[m[32m        "lodash.isnumber": "^3.0.3",[m
[32m+[m[32m        "lodash.isplainobject": "^4.0.6",[m
[32m+[m[32m        "lodash.isstring": "^4.0.1",[m
[32m+[m[32m        "lodash.once": "^4.0.0",[m
[32m+[m[32m        "ms": "^2.1.1",[m
[32m+[m[32m        "semver": "^5.6.0"[m
[32m+[m[32m      },[m
[32m+[m[32m      "engines": {[m
[32m+[m[32m        "node": ">=4",[m
[32m+[m[32m        "npm": ">=1.4.28"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
[32m+[m[32m    "node_modules/jsonwebtoken/node_modules/ms": {[m
[32m+[m[32m      "version": "2.1.3",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/ms/-/ms-2.1.3.tgz",[m
[32m+[m[32m      "integrity": "sha512-6FlzubTLZG3J2a/NVCAleEhjzq5oxgHyaCU9yYXvcLsvoVaHJq/s5xXI6/XXP6tz7R9xAOtHnSO/tXtF3WRTlA=="[m
[32m+[m[32m    },[m
[32m+[m[32m    "node_modules/jsonwebtoken/node_modules/semver": {[m
[32m+[m[32m      "version": "5.7.1",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/semver/-/semver-5.7.1.tgz",[m
[32m+[m[32m      "integrity": "sha512-sauaDf/PZdVgrLTNYHRtpXa1iRiKcaebiKQ1BJdpQlWH2lCvexQdX55snPFyK7QzpudqbCI0qXFfOasHdyNDGQ==",[m
[32m+[m[32m      "bin": {[m
[32m+[m[32m        "semver": "bin/semver"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "node_modules/jsprim": {[m
       "version": "1.4.1",[m
       "resolved": "https://registry.npmjs.org/jsprim/-/jsprim-1.4.1.tgz",[m
[36m@@ -28956,6 +29014,25 @@[m
         "node": ">=8"[m
       }[m
     },[m
[32m+[m[32m    "node_modules/jwa": {[m
[32m+[m[32m      "version": "1.4.1",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/jwa/-/jwa-1.4.1.tgz",[m
[32m+[m[32m      "integrity": "sha512-qiLX/xhEEFKUAJ6FiBMbes3w9ATzyk5W7Hvzpa/SLYdxNtng+gcurvrI7TbACjIXlsJyr05/S1oUhZrc63evQA==",[m
[32m+[m[32m      "dependencies": {[m
[32m+[m[32m        "buffer-equal-constant-time": "1.0.1",[m
[32m+[m[32m        "ecdsa-sig-formatter": "1.0.11",[m
[32m+[m[32m        "safe-buffer": "^5.0.1"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
[32m+[m[32m    "node_modules/jws": {[m
[32m+[m[32m      "version": "3.2.2",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/jws/-/jws-3.2.2.tgz",[m
[32m+[m[32m      "integrity": "sha512-YHlZCB6lMTllWDtSPHz/ZXTsi8S00usEV6v1tjq8tOUZzw7DpSDWVXjXDre6ed1w/pd495ODpHZYSdkRTsa0HA==",[m
[32m+[m[32m      "dependencies": {[m
[32m+[m[32m        "jwa": "^1.4.1",[m
[32m+[m[32m        "safe-buffer": "^5.0.1"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "node_modules/keyv": {[m
       "version": "3.1.0",[m
       "resolved": "https://registry.npmjs.org/keyv/-/keyv-3.1.0.tgz",[m
[36m@@ -29340,11 +29417,35 @@[m
       "resolved": "https://registry.npmjs.org/lodash.debounce/-/lodash.debounce-4.0.8.tgz",[m
       "integrity": "sha1-gteb/zCmfEAF/9XiUVMArZyk168="[m
     },[m
[32m+[m[32m    "node_modules/lodash.includes": {[m
[32m+[m[32m      "version": "4.3.0",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/lodash.includes/-/lodash.includes-4.3.0.tgz",[m
[32m+[m[32m      "integrity": "sha1-YLuYqHy5I8aMoeUTJUgzFISfVT8="[m
[32m+[m[32m    },[m
[32m+[m[32m    "node_modules/lodash.isboolean": {[m
[32m+[m[32m      "version": "3.0.3",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/lodash.isboolean/-/lodash.isboolean-3.0.3.tgz",[m
[32m+[m[32m      "integrity": "sha1-bC4XHbKiV82WgC/UOwGyDV9YcPY="[m
[32m+[m[32m    },[m
[32m+[m[32m    "node_modules/lodash.isinteger": {[m
[32m+[m[32m      "version": "4.0.4",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/lodash.isinteger/-/lodash.isinteger-4.0.4.tgz",[m
[32m+[m[32m      "integrity": "sha1-YZwK89A/iwTDH1iChAt3sRzWg0M="[m
[32m+[m[32m    },[m
[32m+[m[32m    "node_modules/lodash.isnumber": {[m
[32m+[m[32m      "version": "3.0.3",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/lodash.isnumber/-/lodash.isnumber-3.0.3.tgz",[m
[32m+[m[32m      "integrity": "sha1-POdoEMWSjQM1IwGsKHMX8RwLH/w="[m
[32m+[m[32m    },[m
     "node_modules/lodash.isplainobject": {[m
       "version": "4.0.6",[m
       "resolved": "https://registry.npmjs.org/lodash.isplainobject/-/lodash.isplainobject-4.0.6.tgz",[m
[31m-      "integrity": "sha1-fFJqUtibRcRcxpC4gWO+BJf1UMs=",[m
[31m-      "dev": true[m
[32m+[m[32m      "integrity": "sha1-fFJqUtibRcRcxpC4gWO+BJf1UMs="[m
[32m+[m[32m    },[m
[32m+[m[32m    "node_modules/lodash.isstring": {[m
[32m+[m[32m      "version": "4.0.1",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/lodash.isstring/-/lodash.isstring-4.0.1.tgz",[m
[32m+[m[32m      "integrity": "sha1-1SfftUVuynzJu5XV2ur4i6VKVFE="[m
     },[m
     "node_modules/lodash.memoize": {[m
       "version": "4.1.2",[m
[36m@@ -29352,6 +29453,11 @@[m
       "integrity": "sha1-vMbEmkKihA7Zl/Mj6tpezRguC/4=",[m
       "dev": true[m
     },[m
[32m+[m[32m    "node_modules/lodash.once": {[m
[32m+[m[32m      "version": "4.1.1",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/lodash.once/-/lodash.once-4.1.1.tgz",[m
[32m+[m[32m      "integrity": "sha1-DdOXEhPHxW34gJd9UEyI+0cal6w="[m
[32m+[m[32m    },[m
     "node_modules/lodash.sortby": {[m
       "version": "4.7.0",[m
       "resolved": "https://registry.npmjs.org/lodash.sortby/-/lodash.sortby-4.7.0.tgz",[m
[36m@@ -34971,6 +35077,20 @@[m
         "node": ">=10"[m
       }[m
     },[m
[32m+[m[32m    "node_modules/react-tracking": {[m
[32m+[m[32m      "version": "8.1.0",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/react-tracking/-/react-tracking-8.1.0.tgz",[m
[32m+[m[32m      "integrity": "sha512-iQGkQM6OP2Vxxpn8ckR6WVvxdR501fygXff5DQhOVKp7hnVtg0jaUUIBWEVUVhXlMeOylZhDoLehR5hsZZy+4w==",[m
[32m+[m[32m      "dependencies": {[m
[32m+[m[32m        "core-js": "^3.3.2",[m
[32m+[m[32m        "deepmerge": "^4.1.1",[m
[32m+[m[32m        "hoist-non-react-statics": "^3.3.0"[m
[32m+[m[32m      },[m
[32m+[m[32m      "peerDependencies": {[m
[32m+[m[32m        "prop-types": "^15.x",[m
[32m+[m[32m        "react": "^16.8"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "node_modules/react-transition-group": {[m
       "version": "4.4.1",[m
       "resolved": "https://registry.npmjs.org/react-transition-group/-/react-transition-group-4.4.1.tgz",[m
[36m@@ -54383,6 +54503,15 @@[m
       "integrity": "sha1-7ihweulOEdK4J7y+UnC86n8+ce4=",[m
       "dev": true[m
     },[m
[32m+[m[32m    "@types/jsonwebtoken": {[m
[32m+[m[32m      "version": "8.5.4",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/@types/jsonwebtoken/-/jsonwebtoken-8.5.4.tgz",[m
[32m+[m[32m      "integrity": "sha512-4L8msWK31oXwdtC81RmRBAULd0ShnAHjBuKT9MRQpjP0piNrZdXyTRcKY9/UIfhGeKIT4PvF5amOOUbbT/9Wpg==",[m
[32m+[m[32m      "dev": true,[m
[32m+[m[32m      "requires": {[m
[32m+[m[32m        "@types/node": "*"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "@types/lodash": {[m
       "version": "4.14.161",[m
       "resolved": "https://registry.npmjs.org/@types/lodash/-/lodash-4.14.161.tgz",[m
[36m@@ -58912,6 +59041,11 @@[m
         "isarray": "^1.0.0"[m
       }[m
     },[m
[32m+[m[32m    "buffer-equal-constant-time": {[m
[32m+[m[32m      "version": "1.0.1",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/buffer-equal-constant-time/-/buffer-equal-constant-time-1.0.1.tgz",[m
[32m+[m[32m      "integrity": "sha1-+OcRMvf/5uAaXJaXpMbz5I1cyBk="[m
[32m+[m[32m    },[m
     "buffer-from": {[m
       "version": "1.1.1",[m
       "resolved": "https://registry.npmjs.org/buffer-from/-/buffer-from-1.1.1.tgz",[m
[36m@@ -60879,8 +61013,7 @@[m
     "deepmerge": {[m
       "version": "4.2.2",[m
       "resolved": "https://registry.npmjs.org/deepmerge/-/deepmerge-4.2.2.tgz",[m
[31m-      "integrity": "sha512-FJ3UgI4gIl+PHZm53knsuSFpE+nESMr7M4v9QcgB7S63Kj/6WqMiFQJpBBYz1Pt+66bZpP3Q7Lye0Oo9MPKEdg==",[m
[31m-      "dev": true[m
[32m+[m[32m      "integrity": "sha512-FJ3UgI4gIl+PHZm53knsuSFpE+nESMr7M4v9QcgB7S63Kj/6WqMiFQJpBBYz1Pt+66bZpP3Q7Lye0Oo9MPKEdg=="[m
     },[m
     "default-gateway": {[m
       "version": "4.2.0",[m
[36m@@ -61431,6 +61564,14 @@[m
         "safer-buffer": "^2.1.0"[m
       }[m
     },[m
[32m+[m[32m    "ecdsa-sig-formatter": {[m
[32m+[m[32m      "version": "1.0.11",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/ecdsa-sig-formatter/-/ecdsa-sig-formatter-1.0.11.tgz",[m
[32m+[m[32m      "integrity": "sha512-nagl3RYrbNv6kQkeJIpt6NJZy8twLB/2vtz6yN9Z4vRKHN4/QZJIEbqohALSgwKdnksuY3k5Addp5lg8sVoVcQ==",[m
[32m+[m[32m      "requires": {[m
[32m+[m[32m        "safe-buffer": "^5.0.1"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "editions": {[m
       "version": "2.3.1",[m
       "resolved": "https://registry.npmjs.org/editions/-/editions-2.3.1.tgz",[m
[36m@@ -67416,6 +67557,35 @@[m
       "integrity": "sha1-LHS27kHZPKUbe1qu6PUDYx0lKnM=",[m
       "dev": true[m
     },[m
[32m+[m[32m    "jsonwebtoken": {[m
[32m+[m[32m      "version": "8.5.1",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/jsonwebtoken/-/jsonwebtoken-8.5.1.tgz",[m
[32m+[m[32m      "integrity": "sha512-XjwVfRS6jTMsqYs0EsuJ4LGxXV14zQybNd4L2r0UvbVnSF9Af8x7p5MzbJ90Ioz/9TI41/hTCvznF/loiSzn8w==",[m
[32m+[m[32m      "requires": {[m
[32m+[m[32m        "jws": "^3.2.2",[m
[32m+[m[32m        "lodash.includes": "^4.3.0",[m
[32m+[m[32m        "lodash.isboolean": "^3.0.3",[m
[32m+[m[32m        "lodash.isinteger": "^4.0.4",[m
[32m+[m[32m        "lodash.isnumber": "^3.0.3",[m
[32m+[m[32m        "lodash.isplainobject": "^4.0.6",[m
[32m+[m[32m        "lodash.isstring": "^4.0.1",[m
[32m+[m[32m        "lodash.once": "^4.0.0",[m
[32m+[m[32m        "ms": "^2.1.1",[m
[32m+[m[32m        "semver": "^5.6.0"[m
[32m+[m[32m      },[m
[32m+[m[32m      "dependencies": {[m
[32m+[m[32m        "ms": {[m
[32m+[m[32m          "version": "2.1.3",[m
[32m+[m[32m          "resolved": "https://registry.npmjs.org/ms/-/ms-2.1.3.tgz",[m
[32m+[m[32m          "integrity": "sha512-6FlzubTLZG3J2a/NVCAleEhjzq5oxgHyaCU9yYXvcLsvoVaHJq/s5xXI6/XXP6tz7R9xAOtHnSO/tXtF3WRTlA=="[m
[32m+[m[32m        },[m
[32m+[m[32m        "semver": {[m
[32m+[m[32m          "version": "5.7.1",[m
[32m+[m[32m          "resolved": "https://registry.npmjs.org/semver/-/semver-5.7.1.tgz",[m
[32m+[m[32m          "integrity": "sha512-sauaDf/PZdVgrLTNYHRtpXa1iRiKcaebiKQ1BJdpQlWH2lCvexQdX55snPFyK7QzpudqbCI0qXFfOasHdyNDGQ=="[m
[32m+[m[32m        }[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "jsprim": {[m
       "version": "1.4.1",[m
       "resolved": "https://registry.npmjs.org/jsprim/-/jsprim-1.4.1.tgz",[m
[36m@@ -67560,6 +67730,25 @@[m
       "integrity": "sha512-pBxcB3LFc8QVgdggvZWyeys+hnrNWg4OcZIU/1X59k5jQdLBlCsYGRQaz234SqoRLTCgMH00fY0xRJH+F9METQ==",[m
       "dev": true[m
     },[m
[32m+[m[32m    "jwa": {[m
[32m+[m[32m      "version": "1.4.1",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/jwa/-/jwa-1.4.1.tgz",[m
[32m+[m[32m      "integrity": "sha512-qiLX/xhEEFKUAJ6FiBMbes3w9ATzyk5W7Hvzpa/SLYdxNtng+gcurvrI7TbACjIXlsJyr05/S1oUhZrc63evQA==",[m
[32m+[m[32m      "requires": {[m
[32m+[m[32m        "buffer-equal-constant-time": "1.0.1",[m
[32m+[m[32m        "ecdsa-sig-formatter": "1.0.11",[m
[32m+[m[32m        "safe-buffer": "^5.0.1"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
[32m+[m[32m    "jws": {[m
[32m+[m[32m      "version": "3.2.2",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/jws/-/jws-3.2.2.tgz",[m
[32m+[m[32m      "integrity": "sha512-YHlZCB6lMTllWDtSPHz/ZXTsi8S00usEV6v1tjq8tOUZzw7DpSDWVXjXDre6ed1w/pd495ODpHZYSdkRTsa0HA==",[m
[32m+[m[32m      "requires": {[m
[32m+[m[32m        "jwa": "^1.4.1",[m
[32m+[m[32m        "safe-buffer": "^5.0.1"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "keyv": {[m
       "version": "3.1.0",[m
       "resolved": "https://registry.npmjs.org/keyv/-/keyv-3.1.0.tgz",[m
[36m@@ -67868,11 +68057,35 @@[m
       "resolved": "https://registry.npmjs.org/lodash.debounce/-/lodash.debounce-4.0.8.tgz",[m
       "integrity": "sha1-gteb/zCmfEAF/9XiUVMArZyk168="[m
     },[m
[32m+[m[32m    "lodash.includes": {[m
[32m+[m[32m      "version": "4.3.0",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/lodash.includes/-/lodash.includes-4.3.0.tgz",[m
[32m+[m[32m      "integrity": "sha1-YLuYqHy5I8aMoeUTJUgzFISfVT8="[m
[32m+[m[32m    },[m
[32m+[m[32m    "lodash.isboolean": {[m
[32m+[m[32m      "version": "3.0.3",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/lodash.isboolean/-/lodash.isboolean-3.0.3.tgz",[m
[32m+[m[32m      "integrity": "sha1-bC4XHbKiV82WgC/UOwGyDV9YcPY="[m
[32m+[m[32m    },[m
[32m+[m[32m    "lodash.isinteger": {[m
[32m+[m[32m      "version": "4.0.4",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/lodash.isinteger/-/lodash.isinteger-4.0.4.tgz",[m
[32m+[m[32m      "integrity": "sha1-YZwK89A/iwTDH1iChAt3sRzWg0M="[m
[32m+[m[32m    },[m
[32m+[m[32m    "lodash.isnumber": {[m
[32m+[m[32m      "version": "3.0.3",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/lodash.isnumber/-/lodash.isnumber-3.0.3.tgz",[m
[32m+[m[32m      "integrity": "sha1-POdoEMWSjQM1IwGsKHMX8RwLH/w="[m
[32m+[m[32m    },[m
     "lodash.isplainobject": {[m
       "version": "4.0.6",[m
       "resolved": "https://registry.npmjs.org/lodash.isplainobject/-/lodash.isplainobject-4.0.6.tgz",[m
[31m-      "integrity": "sha1-fFJqUtibRcRcxpC4gWO+BJf1UMs=",[m
[31m-      "dev": true[m
[32m+[m[32m      "integrity": "sha1-fFJqUtibRcRcxpC4gWO+BJf1UMs="[m
[32m+[m[32m    },[m
[32m+[m[32m    "lodash.isstring": {[m
[32m+[m[32m      "version": "4.0.1",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/lodash.isstring/-/lodash.isstring-4.0.1.tgz",[m
[32m+[m[32m      "integrity": "sha1-1SfftUVuynzJu5XV2ur4i6VKVFE="[m
     },[m
     "lodash.memoize": {[m
       "version": "4.1.2",[m
[36m@@ -67880,6 +68093,11 @@[m
       "integrity": "sha1-vMbEmkKihA7Zl/Mj6tpezRguC/4=",[m
       "dev": true[m
     },[m
[32m+[m[32m    "lodash.once": {[m
[32m+[m[32m      "version": "4.1.1",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/lodash.once/-/lodash.once-4.1.1.tgz",[m
[32m+[m[32m      "integrity": "sha1-DdOXEhPHxW34gJd9UEyI+0cal6w="[m
[32m+[m[32m    },[m
     "lodash.sortby": {[m
       "version": "4.7.0",[m
       "resolved": "https://registry.npmjs.org/lodash.sortby/-/lodash.sortby-4.7.0.tgz",[m
[36m@@ -72628,6 +72846,16 @@[m
         "use-latest": "^1.0.0"[m
       }[m
     },[m
[32m+[m[32m    "react-tracking": {[m
[32m+[m[32m      "version": "8.1.0",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/react-tracking/-/react-tracking-8.1.0.tgz",[m
[32m+[m[32m      "integrity": "sha512-iQGkQM6OP2Vxxpn8ckR6WVvxdR501fygXff5DQhOVKp7hnVtg0jaUUIBWEVUVhXlMeOylZhDoLehR5hsZZy+4w==",[m
[32m+[m[32m      "requires": {[m
[32m+[m[32m        "core-js": "^3.3.2",[m
[32m+[m[32m        "deepmerge": "^4.1.1",[m
[32m+[m[32m        "hoist-non-react-statics": "^3.3.0"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "react-transition-group": {[m
       "version": "4.4.1",[m
       "resolved": "https://registry.npmjs.org/react-transition-group/-/react-transition-group-4.4.1.tgz",[m
[1mdiff --git a/frontend/package.json b/frontend/package.json[m
[1mindex 57a3a26d..2f54fea4 100644[m
[1m--- a/frontend/package.json[m
[1m+++ b/frontend/package.json[m
[36m@@ -40,6 +40,7 @@[m
     "highlight.js": "^10.1.2",[m
     "jquery": "^3.5.1",[m
     "js-cookie": "^2.2.1",[m
[32m+[m[32m    "jsonwebtoken": "^8.5.1",[m
     "less": "^3.12.2",[m
     "lodash": "^4.17.19",[m
     "material-ui-dropzone": "^3.3.1",[m
[36m@@ -61,6 +62,7 @@[m
     "react-scroll": "^1.8.0",[m
     "react-spring": "^8.0.27",[m
     "react-stripe-checkout": "^2.6.3",[m
[32m+[m[32m    "react-tracking": "^8.1.0",[m
     "sass": "^1.26.10",[m
     "uuid": "^8.3.2"[m
   },[m
[36m@@ -99,6 +101,7 @@[m
     "@testing-library/user-event": "^7.1.2",[m
     "@types/aos": "^3.0.3",[m
     "@types/jest": "^25.2.3",[m
[32m+[m[32m    "@types/jsonwebtoken": "^8.5.4",[m
     "@types/react-color": "^3.0.4",[m
     "@types/stripe": "^7.13.25",[m
     "@typescript-eslint/eslint-plugin": "^2.10.0",[m
[1mdiff --git a/frontend/src/auth/Auth.ts b/frontend/src/auth/Auth.ts[m
[1mindex c033260e..dbe94756 100644[m
[1m--- a/frontend/src/auth/Auth.ts[m
[1m+++ b/frontend/src/auth/Auth.ts[m
[36m@@ -37,7 +37,7 @@[m [mclass Auth {[m
         return this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);[m
     }[m
 [m
[31m-    private async processAuthenticationResponse(authenticationResponse: Credentials, callback: () => any, errorCallback: (response: Credentials) => any) {[m
[32m+[m[32m    private async processAuthenticationResponse(authenticationResponse: Credentials, successRedirectToDashboard: () => any, errorCallback: (response: Credentials) => any) {[m
         if (authenticationResponse.authenticated) {[m
             this.authenticated = true;[m
             SessionStorage.setAuthorization(authenticationResponse.sessionId, authenticationResponse.jwtToken);[m
[36m@@ -49,7 +49,7 @@[m [mclass Auth {[m
             this.isActive = accountIsActive;[m
             SessionStorage.setIsActive(accountIsActive);[m
 [m
[31m-            await callback();[m
[32m+[m[32m            await successRedirectToDashboard();[m
             return true;[m
         } else {[m
             this.authenticated = false;[m
[36m@@ -70,10 +70,10 @@[m [mclass Auth {[m
         }[m
     }[m
 [m
[31m-    async loginWithGoogle(oneTimeCode: string, tokenId: string, callback: () => void, errorCallback: (response: Credentials) => void) {[m
[32m+[m[32m    async loginWithGoogle(oneTimeCode: string, tokenId: string, successRedirectToDashboard: () => void, errorCallback: (response: Credentials) => void) {[m
         try {[m
             const authenticationResponse = await this.loginClient.Login.RequestLoginWithGoogleToken(oneTimeCode, tokenId);[m
[31m-            return this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);[m
[32m+[m[32m            return this.processAuthenticationResponse(authenticationResponse, successRedirectToDashboard, errorCallback);[m
         } catch {[m
             console.log("Error attempting to reach the server.");[m
             return null;[m
[1mdiff --git a/frontend/src/auth/googlebutton/GoogleLogin.tsx b/frontend/src/auth/googlebutton/GoogleLogin.tsx[m
[1mindex 0b123539..3cf9df31 100644[m
[1m--- a/frontend/src/auth/googlebutton/GoogleLogin.tsx[m
[1m+++ b/frontend/src/auth/googlebutton/GoogleLogin.tsx[m
[36m@@ -1,15 +1,14 @@[m
[31m-import { googleOAuthClientId } from '@api-client/clientUtils'[m
[31m-import React from 'react'[m
[31m-import { makeStyles } from '@material-ui/core'[m
[31m-import { AnyFunction } from '@Palavyr-Types'[m
[31m-import { useEffect } from 'react'[m
[32m+[m[32mimport { googleOAuthClientId } from "@api-client/clientUtils";[m
[32m+[m[32mimport React from "react";[m
[32m+[m[32mimport { makeStyles } from "@material-ui/core";[m
[32m+[m[32mimport { AnyFunction } from "@Palavyr-Types";[m
[32m+[m[32mimport { useEffect } from "react";[m
 [m
[31m-[m
[31m-const useStyles = makeStyles(theme => ({[m
[32m+[m[32mconst useStyles = makeStyles((theme) => ({[m
     loginButton: {[m
         // fontSize: "18pt"[m
[31m-    }[m
[31m-}))[m
[32m+[m[32m    },[m
[32m+[m[32m}));[m
 [m
 interface IGoogleLogin {[m
     onSuccess: AnyFunction;[m
[36m@@ -22,50 +21,50 @@[m [minterface Gapi {[m
     signin2: any;[m
 }[m
 [m
[31m-type CurrentWindow = Window & typeof globalThis & {[m
[31m-    gapi: Gapi;[m
[31m-}[m
[32m+[m[32mtype CurrentWindow = Window &[m
[32m+[m[32m    typeof globalThis & {[m
[32m+[m[32m        gapi: Gapi;[m
[32m+[m[32m    };[m
 [m
 export const GoogleLogin = ({ onSuccess, onFailure }: IGoogleLogin) => {[m
     const classes = useStyles();[m
 [m
     const initializeGoogleSignin = () => {[m
[31m-        window.gapi.load('auth2', () => {[m
[31m-[m
[31m-            window.gapi.auth2.init({ client_id: googleOAuthClientId, fetch_basic_profile: true });[m
[31m-            window.gapi.load('signin2', () => {[m
[31m-[m
[31m-                const params = {[m
[31m-                    onsuccess: onSuccess,[m
[31m-                    onfailure: onFailure,[m
[31m-                    // scope: 'email profile openid',[m
[31m-                    // access_type: "online",[m
[31m-                    'width': 240,[m
[31m-                    'height': 50,[m
[31m-                    'longtitle': true,[m
[31m-                    'theme': 'dark',[m
[31m-                };[m
[31m-                window.gapi.signin2.render('googleLoginButton', params);[m
[31m-            })[m
[31m-        })[m
[31m-    }[m
[32m+[m[32m        window.gapi.load("auth2", () => {[m
[32m+[m[32m            window.gapi.auth2.init({ client_id: googleOAuthClientId, fetch_basic_profile: true }).then(() => {[m
[32m+[m[32m                window.gapi.load("signin2", () => {[m
[32m+[m[32m                    const params = {[m
[32m+[m[32m                        onsuccess: onSuccess,[m
[32m+[m[32m                        onfailure: onFailure,[m
[32m+[m[32m                        // scope: 'email profile openid',[m
[32m+[m[32m                        // access_type: "online",[m
[32m+[m[32m                        width: 240,[m
[32m+[m[32m                        height: 50,[m
[32m+[m[32m                        longtitle: true,[m
[32m+[m[32m                        theme: "dark",[m
[32m+[m[32m                    };[m
[32m+[m[32m                    window.gapi.signin2.render("googleLoginButton", params);[m
[32m+[m[32m                });[m
[32m+[m[32m            });[m
[32m+[m[32m        });[m
[32m+[m[32m    };[m
 [m
     const insertGapiScript = () => {[m
[31m-        const script = document.createElement('script');[m
[32m+[m[32m        const script = document.createElement("script");[m
         script.src = "https://apis.google.com/js/platform.js";[m
         script.onload = () => {[m
[31m-            initializeGoogleSignin()[m
[31m-        }[m
[32m+[m[32m            initializeGoogleSignin();[m
[32m+[m[32m        };[m
         document.body.appendChild(script);[m
[31m-    }[m
[32m+[m[32m    };[m
 [m
     useEffect(() => {[m
[31m-        insertGapiScript()[m
[31m-    }, [])[m
[32m+[m[32m        insertGapiScript();[m
[32m+[m[32m    }, []);[m
 [m
     return ([m
         <div style={{ display: "flex", justifyContent: "center", width: "100%" }}>[m
             <div id="googleLoginButton" className={classes.loginButton}></div>[m
         </div>[m
[31m-    )[m
[31m-}[m
\ No newline at end of file[m
[32m+[m[32m    );[m
[32m+[m[32m};[m
[1mdiff --git a/frontend/src/client/AxiosClient.tsx b/frontend/src/client/AxiosClient.tsx[m
[1mindex 4b81fc45..136a548a 100644[m
[1m--- a/frontend/src/client/AxiosClient.tsx[m
[1m+++ b/frontend/src/client/AxiosClient.tsx[m
[36m@@ -1,11 +1,57 @@[m
[31m-import axios, { AxiosInstance, AxiosRequestConfig } from "axios";[m
[32m+[m[32mimport axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from "axios";[m
[32m+[m[32mimport { SessionStorage } from "localStorage/sessionStorage";[m
 import { serverUrl, SPECIAL_HEADERS } from "./clientUtils";[m
 [m
 interface IAxiosClient {[m
[31m-    get<T>(url: string, config: AxiosRequestConfig): Promise<T>;[m
[31m-    post<T>(url: string, payload: T, config: AxiosRequestConfig): Promise<T>;[m
[31m-    put<T>(url: string, payload: T, config: AxiosRequestConfig): Promise<T>;[m
[31m-    delete<T>(url: string): Promise<T>;[m
[32m+[m[32m    get<T>(url: string, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T>;[m
[32m+[m[32m    post<T>(url: string, payload: T, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T>;[m
[32m+[m[32m    put<T>(url: string, payload: T, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T>;[m
[32m+[m[32m    delete<T>(url: string, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T>;[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mexport enum CacheIds {[m
[32m+[m[32m    Areas = "Areas",[m
[32m+[m[32m    CurrentPlanMeta = "CurrentPlanMeta",[m
[32m+[m[32m    Enquiries = "Enquiries",[m
[32m+[m[32m    Conversation = "Conversation",[m
[32m+[m[32m    WidgetPrefs = "WidgetPrefs",[m
[32m+[m[32m    CustomerId = "CustomerId",[m
[32m+[m[32m    Attachments = "Attachments",[m
[32m+[m[32m    PalavyrConfiguration = "PalavyrConfiguration",[m
[32m+[m[32m    CompanyName = "CompanyName",[m
[32m+[m[32m    Email = "Email",[m
[32m+[m[32m    PhoneNumber = "PhoneNumber",[m
[32m+[m[32m    Locale = "Locale",[m
[32m+[m[32m    Logo = "Logo",[m
[32m+[m[32m    ShowSeenQueries = "ShowSeenQueries",[m
[32m+[m[32m    NeedsPassword = "NeedsPassword"[m
[32m+[m
[32m+[m[32m}[m
[32m+[m
[32m+[m
[32m+[m[32mexport async function DoRequest<T>(resolve: (value: T | PromiseLike<T>) => void, reject: (reason?: any) => void, request: Promise<AxiosResponse<T>>, cacheId?: CacheIds) {[m
[32m+[m[32m    if (cacheId) {[m
[32m+[m[32m        try {[m
[32m+[m[32m            let result: T;[m
[32m+[m[32m            const cachedValue = SessionStorage.getCacheValue(cacheId);[m
[32m+[m[32m            if (cachedValue) {[m
[32m+[m[32m                result = cachedValue as T;[m
[32m+[m[32m            } else {[m
[32m+[m[32m                const response = (await request) as AxiosResponse<T>;[m
[32m+[m[32m                SessionStorage.setCacheValue(cacheId, response.data);[m
[32m+[m[32m                resolve(response.data as T);[m
[32m+[m[32m            }[m
[32m+[m[32m        } catch (response) {[m
[32m+[m[32m            reject(response);[m
[32m+[m[32m        }[m
[32m+[m[32m    } else {[m
[32m+[m[32m        try {[m
[32m+[m[32m            const response = (await request) as AxiosResponse<T>;[m
[32m+[m[32m            resolve(response.data as T);[m
[32m+[m[32m        } catch (response) {[m
[32m+[m[32m            reject(response);[m
[32m+[m[32m        }[m
[32m+[m[32m    }[m
 }[m
 [m
 export class AxiosClient implements IAxiosClient {[m
[36m@@ -27,46 +73,97 @@[m [mexport class AxiosClient implements IAxiosClient {[m
         this.client.defaults.baseURL = serverUrl + "/api/";[m
     }[m
 [m
[31m-    get<T>(url: string, config?: AxiosRequestConfig): Promise<T> {[m
[31m-        return new Promise<T>(async (resolve, reject) => {[m
[31m-            try {[m
[31m-                const response = await this.client.get(url, config);[m
[31m-                resolve(response.data as T);[m
[31m-            } catch (response) {[m
[31m-                reject(response);[m
[32m+[m[32m    get<T>(url: string, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T> {[m
[32m+[m[32m        return new Promise<T>(async (resolve: (value: T | PromiseLike<T>) => void, reject: (reason?: any) => void) => {[m
[32m+[m[32m            if (cacheId) {[m
[32m+[m[32m                try {[m
[32m+[m[32m                    let result: T;[m
[32m+[m[32m                    const cachedValue = SessionStorage.getCacheValue(cacheId);[m
[32m+[m[32m                    if (cachedValue) {[m
[32m+[m[32m                        resolve(cachedValue as T);[m
[32m+[m[32m                    } else {[m
[32m+[m[32m                        const response = (await this.client.get(url, config)) as AxiosResponse<T>;[m
[32m+[m[32m                        SessionStorage.setCacheValue(cacheId, response.data);[m
[32m+[m[32m                        resolve(response.data as T);[m
[32m+[m[32m                    }[m
[32m+[m[32m                } catch (response) {[m
[32m+[m[32m                    reject(response);[m
[32m+[m[32m                }[m
[32m+[m[32m            } else {[m
[32m+[m[32m                try {[m
[32m+[m[32m                    const response = (await this.client.get(url, config)) as AxiosResponse<T>;[m
[32m+[m[32m                    resolve(response.data as T);[m
[32m+[m[32m                } catch (response) {[m
[32m+[m[32m                    reject(response);[m
[32m+[m[32m                }[m
             }[m
         });[m
     }[m
 [m
[31m-    post<T, S>(url: string, payload?: S, config?: AxiosRequestConfig): Promise<T> {[m
[31m-        return new Promise<T>(async (resolve, reject) => {[m
[31m-            try {[m
[31m-                const response = await this.client.post(url, payload, config);[m
[31m-                resolve(response.data as T);[m
[31m-            } catch (response) {[m
[31m-                reject(response);[m
[32m+[m[32m    post<T, S>(url: string, payload?: S, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T> {[m
[32m+[m[32m        return new Promise<T>(async (resolve: (value: T | PromiseLike<T>) => void, reject: (reason?: any) => void) => {[m
[32m+[m[32m            if (cacheId) {[m
[32m+[m[32m                try {[m
[32m+[m[32m                    const response = (await this.client.post(url, payload, config)) as AxiosResponse<T>;[m
[32m+[m[32m                    SessionStorage.setCacheValue(cacheId, response.data);[m
[32m+[m[32m                    resolve(response.data as T);[m
[32m+[m[32m                } catch (response) {[m
[32m+[m[32m                    reject(response);[m
[32m+[m[32m                }[m
[32m+[m[32m            } else {[m
[32m+[m[32m                try {[m
[32m+[m[32m                    const response = (await this.client.post(url, payload, config)) as AxiosResponse<T>;[m
[32m+[m[32m                    resolve(response.data as T);[m
[32m+[m[32m                } catch (response) {[m
[32m+[m[32m                    reject(response);[m
[32m+[m[32m                }[m
             }[m
         });[m
     }[m
 [m
[31m-    put<T, S>(url: string, payload?: S, config?: AxiosRequestConfig): Promise<T> {[m
[31m-        return new Promise<T>(async (resolve, reject) => {[m
[31m-            try {[m
[31m-                const response = await this.client.put(url, payload, config);[m
[31m-                resolve(response.data as T);[m
[31m-            } catch (response) {[m
[31m-                reject(response);[m
[32m+[m[32m    put<T, S>(url: string, payload?: S, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T> {[m
[32m+[m[32m        return new Promise<T>(async (resolve: (value: T | PromiseLike<T>) => void, reject: (reason?: any) => void) => {[m
[32m+[m[32m            if (cacheId) {[m
[32m+[m[32m                try {[m
[32m+[m[32m                    const response = (await this.client.put(url, payload, config)) as AxiosResponse<T>;[m
[32m+[m[32m                    SessionStorage.setCacheValue(cacheId, response.data);[m
[32m+[m[32m                    resolve(response.data as T);[m
[32m+[m[32m                } catch (response) {[m
[32m+[m[32m                    reject(response);[m
[32m+[m[32m                }[m
[32m+[m[32m            } else {[m
[32m+[m[32m                try {[m
[32m+[m[32m                    const response = (await this.client.put(url, payload, config)) as AxiosResponse<T>;[m
[32m+[m[32m                    resolve(response.data as T);[m
[32m+[m[32m                } catch (response) {[m
[32m+[m[32m                    reject(response);[m
[32m+[m[32m                }[m
             }[m
         });[m
     }[m
 [m
[31m-    delete<T>(url: string, config?: AxiosRequestConfig): Promise<T> {[m
[31m-        return new Promise<T>(async (resolve, reject) => {[m
[31m-            try {[m
[31m-                const response = await this.client.delete(url, config);[m
[31m-                resolve(response.data as T);[m
[31m-            } catch (response) {[m
[31m-                reject(response);[m
[32m+[m[32m    delete<T>(url: string, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T> {[m
[32m+[m[32m        return new Promise<T>(async (resolve: (value: T | PromiseLike<T>) => void, reject: (reason?: any) => void) => {[m
[32m+[m[32m            if (cacheId) {[m
[32m+[m[32m                try {[m
[32m+[m[32m                    const response = await this.client.delete(url, config);[m
[32m+[m[32m                    const data = response.data as T;[m
[32m+[m[32m                    if (data) {[m
[32m+[m[32m                        SessionStorage.setCacheValue(cacheId, data);[m
[32m+[m[32m                    } else {[m
[32m+[m[32m                        SessionStorage.clearCacheValue(cacheId);[m
[32m+[m[32m                    }[m
[32m+[m[32m                    resolve(response.data as T);[m
[32m+[m[32m                } catch (response) {[m
[32m+[m[32m                    reject(response);[m
[32m+[m[32m                }[m
[32m+[m[32m            } else {[m
[32m+[m[32m                try {[m
[32m+[m[32m                    const response = await this.client.delete(url, config);[m
[32m+[m[32m                    resolve(response.data as T);[m
[32m+[m[32m                } catch (response) {[m
[32m+[m[32m                    reject(response);[m
[32m+[m[32m                }[m
             }[m
         });[m
     }[m
[1mdiff --git a/frontend/src/client/PalavyrRepository.ts b/frontend/src/client/PalavyrRepository.ts[m
[1mindex 78b8ccf6..7e3092b4 100644[m
[1m--- a/frontend/src/client/PalavyrRepository.ts[m
[1m+++ b/frontend/src/client/PalavyrRepository.ts[m
[36m@@ -30,7 +30,7 @@[m [mimport {[m
 } from "@Palavyr-Types";[m
 import { filterNodeTypeOptionsOnSubscription } from "dashboard/subscriptionFilters/filterConvoNodeTypes";[m
 import { SessionStorage } from "localStorage/sessionStorage";[m
[31m-import { AxiosClient } from "./AxiosClient";[m
[32m+[m[32mimport { AxiosClient, CacheIds } from "./AxiosClient";[m
 import { getJwtTokenFromLocalStorage, getSessionIdFromLocalStorage } from "./clientUtils";[m
 [m
 export class PalavyrRepository {[m
[36m@@ -45,10 +45,26 @@[m [mexport class PalavyrRepository {[m
         this.client = new AxiosClient("tubmcgubs", getSessionIdFromLocalStorage, getJwtTokenFromLocalStorage);[m
     }[m
 [m
[32m+[m[32m    public AuthenticationCheck = {[m
[32m+[m[32m        check: async () => {[m
[32m+[m[32m            let result: boolean;[m
[32m+[m[32m            try {[m
[32m+[m[32m                await this.client.get<boolean>("");[m
[32m+[m[32m                result = true;[m
[32m+[m[32m            } catch {[m
[32m+[m[32m                result = false;[m
[32m+[m[32m            }[m
[32m+[m[32m            return result;[m
[32m+[m[32m        },[m
[32m+[m[32m    };[m
[32m+[m
     public Purchase = {[m
         Customer: {[m
[31m-            GetCustomerId: async () => this.client.get<string>(`payments/customer-id`),[m
[31m-            GetCustomerPortal: async (customerId: string, returnUrl: string) => this.client.post<string, {}>(`payments/customer-portal`, { CustomerId: customerId, ReturnUrl: returnUrl }),[m
[32m+[m[32m            GetCustomerId: async () => this.client.get<string>(`payments/customer-id`, CacheIds.CustomerId),[m
[32m+[m[32m            GetCustomerPortal: async (customerId: string, returnUrl: string) => {[m
[32m+[m[32m                SessionStorage.clearCacheValue(CacheIds.CurrentPlanMeta);[m
[32m+[m[32m                return this.client.post<string, {}>(`payments/customer-portal`, { CustomerId: customerId, ReturnUrl: returnUrl });[m
[32m+[m[32m            },[m
         },[m
         Subscription: {[m
             CancelSubscription: async () => this.client.post<string, {}>(`products/cancel-subscription`),[m
[36m@@ -74,12 +90,21 @@[m [mexport class PalavyrRepository {[m
         UpdateIsEnabled: async (areaToggleStateUpdate: boolean, areaIdentifier: string) => this.client.put<boolean, {}>(`areas/${areaIdentifier}/area-toggle`, { IsEnabled: areaToggleStateUpdate }),[m
         UpdateUseAreaFallbackEmail: async (useAreaFallbackEmailUpdate: boolean, areaIdentifier: string) =>[m
             this.client.put<boolean, {}>(`areas/${areaIdentifier}/use-fallback-email-toggle`, { UseFallback: useAreaFallbackEmailUpdate }),[m
[31m-        GetAreas: async () => this.client.get<Areas>("areas"),[m
[31m-        GetArea: async (areaIdentifier: string) => this.client.get<AreaTable>(`areas/${areaIdentifier}`),[m
[31m-        createArea: (areaName: string) => this.client.post<AreaTable, {}>(`areas/create/`, { AreaName: areaName }), // get creates and gets new area[m
[31m-        updateAreaName: (areaIdentifier: string, areaName: string) => this.client.put<string, {}>(`areas/update/name/${areaIdentifier}`, { AreaName: areaName }),[m
[31m-        updateDisplayTitle: (areaIdentifier: string, displayTitle: string) => this.client.put<string, {}>(`areas/update/display-title/${areaIdentifier}`, { AreaDisplayTitle: displayTitle }),[m
[31m-        deleteArea: (areaIdentifier: string) => this.client.delete<void>(`areas/delete/${areaIdentifier}`),[m
[32m+[m[32m        GetAreas: async () => this.client.get<Areas>("areas", CacheIds.Areas),[m
[32m+[m[32m        createArea: (areaName: string) => this.client.post<AreaTable, {}>(`areas/create/`, { AreaName: areaName }, CacheIds.Areas), // get creates and gets new area[m
[32m+[m
[32m+[m[32m        updateAreaName: (areaIdentifier: string, areaName: string) => {[m
[32m+[m[32m            const result = this.client.put<string, {}>(`areas/update/name/${areaIdentifier}`, { AreaName: areaName });[m
[32m+[m[32m            SessionStorage.clearCacheValue(CacheIds.Areas);[m
[32m+[m[32m            return result;[m
[32m+[m[32m        },[m
[32m+[m[32m        updateDisplayTitle: (areaIdentifier: string, displayTitle: string) => {[m
[32m+[m[32m            const result = this.client.put<string, {}>(`areas/update/display-title/${areaIdentifier}`, { AreaDisplayTitle: displayTitle });[m
[32m+[m[32m            SessionStorage.clearCacheValue(CacheIds.Areas);[m
[32m+[m[32m            return result;[m
[32m+[m[32m        },[m
[32m+[m
[32m+[m[32m        deleteArea: (areaIdentifier: string) => this.client.delete<void>(`areas/delete/${areaIdentifier}`, CacheIds.Areas),[m
         toggleSendPdfResponse: (areaIdentifier: string) => this.client.post<boolean, {}>(`area/send-pdf/${areaIdentifier}`),[m
     };[m
 [m
[36m@@ -94,7 +119,7 @@[m [mexport class PalavyrRepository {[m
         },[m
         Tables: {[m
             Dynamic: {[m
[31m-                getDynamicTableMetas: async (areaIdentifier: string) => this.client.get<DynamicTableMetas>(`tables/dynamic/type/${areaIdentifier}`),[m
[32m+[m[32m                getDynamicTableMetas: async (areaIdentifier: string) => this.client.get<DynamicTableMetas>(`tables/dynamic/type/${areaIdentifier}`), // todo - cache[m
                 getDynamicTableTypes: async () => this.client.get<TableNameMap>(`tables/dynamic/table-name-map`),[m
 [m
                 modifyDynamicTableMeta: async (dynamicTableMeta: DynamicTableMeta) => this.client.put<DynamicTableMeta, {}>(`tables/dynamic/modify`, dynamicTableMeta),[m
[36m@@ -141,18 +166,18 @@[m [mexport class PalavyrRepository {[m
         },[m
 [m
         Attachments: {[m
[31m-            fetchAttachmentLinks: async (areaIdentifier: string) => this.client.get<FileLink[]>(`attachments/${areaIdentifier}`),[m
[31m-            removeAttachment: async (areaIdentifier: string, fileId: string) => this.client.delete<FileLink[]>(`attachments/${areaIdentifier}/file-link`, { data: { fileId: fileId } }),[m
[32m+[m[32m            fetchAttachmentLinks: async (areaIdentifier: string) => this.client.get<FileLink[]>(`attachments/${areaIdentifier}`, CacheIds.Attachments),[m
[32m+[m[32m            removeAttachment: async (areaIdentifier: string, fileId: string) => this.client.delete<FileLink[]>(`attachments/${areaIdentifier}/file-link`, CacheIds.Attachments, { data: { fileId: fileId } }),[m
 [m
             saveSingleAttachment: async (areaIdentifier: string, formData: FormData) =>[m
[31m-                this.client.post<FileLink[], {}>(`attachments/${areaIdentifier}/save-one`, formData, {[m
[32m+[m[32m                this.client.post<FileLink[], {}>(`attachments/${areaIdentifier}/save-one`, formData, CacheIds.Attachments, {[m
                     headers: {[m
                         Accept: "application/json",[m
                         "Content-Type": "multipart/form-data",[m
                     },[m
                 }),[m
             saveManyAttachments: async (areaIdentifier: string, formData: FormData) =>[m
[31m-                this.client.post<FileLink[], {}>(`attachments/${areaIdentifier}/save-many`, formData, {[m
[32m+[m[32m                this.client.post<FileLink[], {}>(`attachments/${areaIdentifier}/save-many`, formData, CacheIds.Attachments, {[m
                     headers: {[m
                         Accept: "application/json",[m
                         "Content-Type": "multipart/form-data",[m
[36m@@ -162,31 +187,28 @@[m [mexport class PalavyrRepository {[m
 [m
         Images: {[m
             // Node Editor Flow[m
[31m-            saveSingleImage: async (formData: FormData) => this.client.post<FileLink[], {}>(`images/save-one`, formData, { headers: this.formDataHeaders }),[m
[32m+[m[32m            saveSingleImage: async (formData: FormData) => this.client.post<FileLink[], {}>(`images/save-one`, formData, undefined, { headers: this.formDataHeaders }),[m
             saveImageUrl: async (url: string, nodeId: string) => this.client.post<FileLink[], {}>(`images/use-link/${nodeId}`, { Url: url }),[m
             getImages: async (imageIds?: string[]) => this.client.get<FileLink[]>(`images${imageIds !== undefined ? `?imageIds=${imageIds.join(",")}` : ""}`), // takes a querystring comma delimieted of imageIds[m
             savePreExistingImage: async (imageId: string, nodeId: string) => this.client.post<ConvoNode, {}>(`images/pre-existing/${imageId}/${nodeId}`),[m
             // DO NOT USE WITH NODE[m
[31m-            saveMultipleImages: async (formData: FormData) => this.client.post<FileLink[], {}>(`images/save-many`, formData, { headers: this.formDataHeaders }),[m
[32m+[m[32m            saveMultipleImages: async (formData: FormData) => this.client.post<FileLink[], {}>(`images/save-many`, formData, undefined, { headers: this.formDataHeaders }),[m
             deleteImage: async (imageIds: string[]) => this.client.delete<FileLink[]>(`images?imageIds=${imageIds.join(",")}`), // takes a querystring command delimited of imageIds[m
             getSignedUrl: async (s3Key: string) => this.client.post<string, {}>(`images/link`, { s3Key: s3Key }),[m
         },[m
     };[m
 [m
     public Conversations = {[m
[31m-        GetConversation: async (areaIdentifier: string) => this.client.get<ConvoNode[]>(`configure-conversations/${areaIdentifier}`),[m
[32m+[m[32m        GetConversation: async (areaIdentifier: string) => this.client.get<ConvoNode[]>(`configure-conversations/${areaIdentifier}`, CacheIds.PalavyrConfiguration),[m
         GetConversationNode: async (nodeId: string) => this.client.get<ConvoNode>(`configure-conversations/nodes/${nodeId}`),[m
         GetNodeOptionsList: async (areaIdentifier: string, planTypeMeta: PlanTypeMeta) =>[m
             filterNodeTypeOptionsOnSubscription(await this.client.get<NodeTypeOptions>(`configure-conversations/${areaIdentifier}/node-type-options`), planTypeMeta),[m
         GetErrors: async (areaIdentifier: string, nodeList: ConvoNode[]) => this.client.post<TreeErrors, {}>(`configure-conversations/${areaIdentifier}/tree-errors`, { Transactions: nodeList }),[m
 [m
[31m-        CheckIfIsMultiOptionType: async (nodeType: string) => this.client.get<boolean>(`configure-conversations/check-multi-option/${nodeType}`),[m
[31m-        CheckIfIsTerminalType: async (nodeType: string) => this.client.get<boolean>(`configure-conversations/check-terminal/${nodeType}`),[m
[31m-        CheckIfIsSplitMergeType: async (nodeType: string) => this.client.get<boolean>(`configure-conversations/check-is-split-merge/${nodeType}`),[m
[31m-[m
[31m-        ModifyConversation: async (nodelist: ConvoNode[], areaIdentifier: string) => this.client.put<ConvoNode[], {}>(`configure-conversations/${areaIdentifier}`, { Transactions: nodelist }),[m
[32m+[m[32m        ModifyConversation: async (nodelist: ConvoNode[], areaIdentifier: string) =>[m
[32m+[m[32m            this.client.put<ConvoNode[], {}>(`configure-conversations/${areaIdentifier}`, { Transactions: nodelist }, CacheIds.PalavyrConfiguration),[m
         ModifyConversationNode: async (nodeId: string, areaIdentifier: string, updatedNode: ConvoTableRow) =>[m
[31m-            this.client.put<ConvoNode[], {}>(`configure-conversations/${areaIdentifier}/nodes/${nodeId}`, updatedNode),[m
[32m+[m[32m            this.client.put<ConvoNode[], {}>(`configure-conversations/${areaIdentifier}/nodes/${nodeId}`, updatedNode, [CacheIds.PalavyrConfiguration, areaIdentifier].join("-") as CacheIds),[m
 [m
         // TODO: Deprecate eventually[m
         EnsureDBIsValid: async () => this.client.post(`configure-conversations/ensure-db-valid`),[m
[36m@@ -194,30 +216,13 @@[m [mexport class PalavyrRepository {[m
 [m
     public WidgetDemo = {[m
         RunConversationPrecheck: async () => this.client.get<PreCheckResult>(`widget-config/demo/pre-check`),[m
[31m-        GetWidetPreferences: async () => this.client.get<WidgetPreferences>(`widget-config/preferences`),[m
[31m-        SaveWidgetPreferences: async (prefs: WidgetPreferences) => this.client.put<WidgetPreferences, WidgetPreferences>(`widget-config/preferences`, prefs),[m
[32m+[m[32m        GetWidetPreferences: async () => this.client.get<WidgetPreferences>(`widget-config/preferences`, CacheIds.WidgetPrefs),[m
[32m+[m[32m        SaveWidgetPreferences: async (prefs: WidgetPreferences) => this.client.put<WidgetPreferences, WidgetPreferences>(`widget-config/preferences`, prefs, CacheIds.WidgetPrefs),[m
     };[m
 [m
[31m-    private async GetWithCache(storageGetCall: () => any, apiGetPromise: Promise<any>, storagePutCall: (result: any) => void) {[m
[31m-        const cachedValue = storageGetCall();[m
[31m-        if (cachedValue) {[m
[31m-            return cachedValue;[m
[31m-        } else {[m
[31m-            const result = await apiGetPromise;[m
[31m-            storagePutCall(result);[m
[31m-            return result;[m
[31m-        }[m
[31m-    }[m
[31m-[m
     public Settings = {[m
         Subscriptions: {[m
[31m-            getCurrentPlanMeta: async () => {[m
[31m-                return await this.GetWithCache([m
[31m-                    () => SessionStorage.getCurrentPlanMeta(),[m
[31m-                    this.client.get<PlanTypeMeta>(`account/settings/current-plan-meta`),[m
[31m-                    (value: any) => SessionStorage.setCurrentPlanMeta(value)[m
[31m-                );[m
[31m-            },[m
[32m+[m[32m            getCurrentPlanMeta: async () => this.client.get<PlanTypeMeta>(`account/settings/current-plan-meta`, CacheIds.CurrentPlanMeta),[m
         },[m
 [m
         Account: {[m
[36m@@ -227,31 +232,28 @@[m [mexport class PalavyrRepository {[m
             checkIsActive: async () => this.client.get<boolean>(`account/is-active`),[m
 [m
             UpdatePassword: async (oldPassword: string, newPassword: string) => this.client.put<boolean, {}>(`account/settings/password`, { OldPassword: oldPassword, Password: newPassword }),[m
[31m-            updateCompanyName: async (companyName: string) => this.client.put<string, {}>(`account/settings/company-name`, { CompanyName: companyName }),[m
[31m-            updateEmail: async (newEmail: string) => this.client.put<EmailVerificationResponse, {}>(`account/settings/email`, { EmailAddress: newEmail }),[m
[31m-            updateUserName: async (newUserName: string) => this.client.put<string, {}>(`account/settings/user-name/`, { UserName: newUserName }),[m
[31m-            updatePhoneNumber: async (newPhoneNumber: string) => this.client.put<string, {}>(`account/settings/phone-number`, { PhoneNumber: newPhoneNumber }),[m
[31m-            updateLocale: async (newLocaleId: string) => this.client.put<LocaleDefinition, {}>(`account/settings/locale`, { LocaleId: newLocaleId }),[m
[32m+[m[32m            updateCompanyName: async (companyName: string) => this.client.put<string, {}>(`account/settings/company-name`, { CompanyName: companyName }, CacheIds.CompanyName),[m
[32m+[m[32m            updateEmail: async (newEmail: string) => this.client.put<EmailVerificationResponse, {}>(`account/settings/email`, { EmailAddress: newEmail }, CacheIds.Email),[m
[32m+[m[32m            updatePhoneNumber: async (newPhoneNumber: string) => this.client.put<string, {}>(`account/settings/phone-number`, { PhoneNumber: newPhoneNumber }, CacheIds.PhoneNumber),[m
[32m+[m[32m            updateLocale: async (newLocaleId: string) => this.client.put<LocaleDefinition, {}>(`account/settings/locale`, { LocaleId: newLocaleId }, CacheIds.Locale),[m
             updateCompanyLogo: async (formData: FormData) =>[m
[31m-                this.client.put<string, {}>(`account/settings/logo`, formData, {[m
[32m+[m[32m                this.client.put<string, {}>(`account/settings/logo`, formData, CacheIds.Logo, {[m
                     headers: {[m
                         Accept: "application/json",[m
                         "Content-Type": "multipart/form-data",[m
                     },[m
                 }),[m
 [m
[31m-            getCompanyName: async () => this.client.get<string>(`account/settings/company-name`),[m
[31m-            getEmail: async () => this.client.get<AccountEmailSettingsResponse>(`account/settings/email`),[m
[31m-            getUserName: async () => this.client.get<string>(`account/settings/user-name`),[m
[31m-            getPhoneNumber: async () => this.client.get<PhoneSettingsResponse>(`account/settings/phone-number`),[m
[32m+[m[32m            getCompanyName: async () => this.client.get<string>(`account/settings/company-name`, CacheIds.CompanyName),[m
[32m+[m[32m            getEmail: async () => this.client.get<AccountEmailSettingsResponse>(`account/settings/email`, CacheIds.Email),[m
[32m+[m[32m            getPhoneNumber: async () => this.client.get<PhoneSettingsResponse>(`account/settings/phone-number`, CacheIds.PhoneNumber),[m
 [m
[31m-            GetLocale: async () => this.client.get<LocaleDefinition>(`account/settings/locale`),[m
[31m-            getCompanyLogo: async () => this.client.get<string>(`account/settings/logo`),[m
[31m-            deleteCompanyLogo: async () => this.client.delete(`account/settings/logo`),[m
[31m-            getCurrentPlan: async () => this.client.get<PlanStatus>(`account/settings/current-plan`),[m
[32m+[m[32m            GetLocale: async () => this.client.get<LocaleDefinition>(`account/settings/locale`, CacheIds.Locale),[m
[32m+[m[32m            getCompanyLogo: async () => this.client.get<string>(`account/settings/logo`, CacheIds.Logo),[m
 [m
[32m+[m[32m            deleteCompanyLogo: async () => this.client.delete(`account/settings/logo`, CacheIds.Logo),[m
             DeleteAccount: async () => this.client.post(`account/delete-account`),[m
[31m-            CheckNeedsPassword: async () => this.client.get<boolean>(`account/needs-password`),[m
[32m+[m[32m            CheckNeedsPassword: async () => this.client.get<boolean>(`account/needs-password`, CacheIds.NeedsPassword),[m
         },[m
         EmailVerification: {[m
             RequestEmailVerification: async (emailAddress: string, areaIdentifier: string) =>[m
[36m@@ -261,19 +263,14 @@[m [mexport class PalavyrRepository {[m
     };[m
 [m
     public Enquiries = {[m
[31m-        getEnquiries: async () =>[m
[31m-            this.GetWithCache([m
[31m-                () => SessionStorage.getEnquiries(),[m
[31m-                this.client.get<Enquiries>(`enquiries`),[m
[31m-                (value: any) => SessionStorage.setEnquiries(value)[m
[31m-            ),[m
[31m-[m
[31m-        getShowSeenEnquiries: async () => this.client.get<boolean>(`enquiries/show`),[m
[31m-        toggleShowSeenEnquiries: async () => this.client.put<boolean, {}>(`enquiries/toggle-show`),[m
[31m-[m
[31m-        updateEnquiry: async (conversationId: string) => this.client.put<Enquiries, {}>(`enquiries/update/${conversationId}`),[m
[31m-        getConversation: async (conversationId: string) => this.client.get<CompletedConversation>(`enquiries/review/${conversationId}`),[m
[32m+[m[32m        getEnquiries: async () => this.client.get<Enquiries>(`enquiries`, CacheIds.Enquiries),[m
[32m+[m[32m        getShowSeenEnquiries: async () => this.client.get<boolean>(`enquiries/show`, CacheIds.ShowSeenQueries),[m
[32m+[m[32m        toggleShowSeenEnquiries: async () => this.client.put<boolean, {}>(`enquiries/toggle-show`, CacheIds.ShowSeenQueries),[m
[32m+[m
[32m+[m[32m        updateEnquiry: async (conversationId: string) => this.client.put<Enquiries, {}>(`enquiries/update/${conversationId}`, CacheIds.Enquiries),[m
[32m+[m[32m        deleteSelectedEnquiries: async (fileReferences: string[]) => this.client.put<Enquiries, {}>(`enquiries/selected`, { FileReferences: fileReferences }, CacheIds.Enquiries),[m
[32m+[m
         getSignedUrl: async (fileId: string) => this.client.get<string>(`enquiries/link/${fileId}`),[m
[31m-        deleteSelectedEnquiries: async (fileReferences: string[]) => this.client.put<Enquiries, {}>(`enquiries/selected`, { FileReferences: fileReferences }),[m
[32m+[m[32m        getConversation: async (conversationId: string) => this.client.get<CompletedConversation>(`enquiries/review/${conversationId}`, CacheIds.Conversation),[m
     };[m
 }[m
[1mdiff --git a/frontend/src/client/clientUtils.ts b/frontend/src/client/clientUtils.ts[m
[1mindex 3ed69a4c..2cf74217 100644[m
[1m--- a/frontend/src/client/clientUtils.ts[m
[1m+++ b/frontend/src/client/clientUtils.ts[m
[36m@@ -1,26 +1,31 @@[m
[32m+[m[32mimport { History } from "history";[m
 import { SessionStorage } from "localStorage/sessionStorage";[m
[32m+[m[32mimport { PalavyrRepository } from "./PalavyrRepository";[m
 [m
 /*[m
 This will retrieve login credental data from localsession and send it with the requestover to the server for retrieval.[m
 */[m
 export const getSessionIdFromLocalStorage = (): string => {[m
[31m-    var sessionId = SessionStorage.getSessionId();[m
[32m+[m[32m    const sessionId = SessionStorage.getSessionId();[m
     return sessionId || "noIdInStorage";[m
 };[m
 [m
 export const getJwtTokenFromLocalStorage = (): string => {[m
[31m-    var token = SessionStorage.getJwtToken();[m
[31m-    if (!token) {[m
[31m-        throw new Error("No token in local storage...");[m
[31m-    }[m
[32m+[m[32m    const token = SessionStorage.getJwtToken();[m
     return token || "noTokenInStorage";[m
 };[m
 [m
[31m-export const redirectToHomeWhenSessionNotEstablished = (history) => {[m
[31m-    const result = SessionStorage.getJwtToken();[m
[31m-    if (!result) {[m
[32m+[m
[32m+[m[32mexport const redirectToHomeWhenSessionNotEstablished = async (history: History<History.UnknownFacade> | string[], repository: PalavyrRepository) => {[m
[32m+[m[32m    const jwt_token = SessionStorage.getJwtToken();[m
[32m+[m[32m    if (!jwt_token) {[m
         history.push("/");[m
     }[m
[32m+[m[32m    const signedIn = await repository.AuthenticationCheck.check();[m
[32m+[m[32m    if (!signedIn) {[m
[32m+[m[32m        history.push("/");[m
[32m+[m[32m    }[m
[32m+[m
 };[m
 [m
 export const serverUrl = process.env.API_URL as string;[m
[1mdiff --git a/frontend/src/dashboard/content/enquiries/Enquiries.tsx b/frontend/src/dashboard/content/enquiries/Enquiries.tsx[m
[1mindex 9f18100e..7fdf9de1 100644[m
[1m--- a/frontend/src/dashboard/content/enquiries/Enquiries.tsx[m
[1m+++ b/frontend/src/dashboard/content/enquiries/Enquiries.tsx[m
[36m@@ -59,7 +59,7 @@[m [mexport const Enquires = () => {[m
         setEnquiries(enqs);[m
         setLoading(false);[m
         setIsLoading(false);[m
[31m-    }, [setLoading]);[m
[32m+[m[32m    }, []);[m
 [m
     const numberPropertyGetter = (enquiry: EnquiryRow) => {[m
         return enquiry.id;[m
[36m@@ -101,10 +101,10 @@[m [mexport const Enquires = () => {[m
                         {sortByPropertyNumeric(numberPropertyGetter, enquiries, true).map((enq: EnquiryRow, index: number) => {[m
                             if (!showSeen) {[m
                                 if (!enq.seen) {[m
[31m-                                    return <EnquiriesTableRow key={index} index={enquiries.length - (index + 1)} enquiry={enq} setEnquiries={setEnquiries} />;[m
[32m+[m[32m                                    return <EnquiriesTableRow key={[enq.conversationId, index].join("-")} index={enquiries.length - (index + 1)} enquiry={enq} setEnquiries={setEnquiries} />;[m
                                 }[m
                             } else {[m
[31m-                                return <EnquiriesTableRow key={index} index={enquiries.length - (index + 1)} enquiry={enq} setEnquiries={setEnquiries} />;[m
[32m+[m[32m                                return <EnquiriesTableRow key={[enq.conversationId, index].join("-")} index={enquiries.length - (index + 1)} enquiry={enq} setEnquiries={setEnquiries} />;[m
                             }[m
                         })}[m
                     </TableBody>[m
[1mdiff --git a/frontend/src/dashboard/content/responseConfiguration/areaSettings/AreaSettings.tsx b/frontend/src/dashboard/content/responseConfiguration/areaSettings/AreaSettings.tsx[m
[1mindex 597e2abf..7c78d065 100644[m
[1m--- a/frontend/src/dashboard/content/responseConfiguration/areaSettings/AreaSettings.tsx[m
[1m+++ b/frontend/src/dashboard/content/responseConfiguration/areaSettings/AreaSettings.tsx[m
[36m@@ -49,7 +49,9 @@[m [mexport const AreaSettings = () => {[m
 [m
     const loadSettings = useCallback(async () => {[m
         setIsLoading(true);[m
[31m-        const areaData = await repository.Area.GetArea(areaIdentifier);[m
[32m+[m[32m        const areas = await repository.Area.GetAreas();[m
[32m+[m[32m        const areaData = areas.filter((x) => x.areaIdentifier === areaIdentifier)[0];[m
[32m+[m
         setSettings({[m
             emailAddress: areaData.areaSpecificEmail,[m
             isVerified: areaData.emailIsVerified,[m
[36m@@ -61,11 +63,12 @@[m [mexport const AreaSettings = () => {[m
         });[m
         setIsEnabledState(areaData.isEnabled);[m
         setIsLoading(false);[m
[31m-        // eslint-disable-next-line react-hooks/exhaustive-deps[m
     }, [areaIdentifier]);[m
 [m
     useEffect(() => {[m
[31m-        if (!loaded) loadSettings();[m
[32m+[m[32m        if (!loaded) {[m
[32m+[m[32m            loadSettings();[m
[32m+[m[32m        }[m
 [m
         setLoaded(true);[m
         return () => {[m
[36m@@ -76,7 +79,8 @@[m [mexport const AreaSettings = () => {[m
     const handleAreaNameChange = async (newAreaName: string) => {[m
         if (newAreaName === settings.areaName) return;[m
         const updatedAreaName = await repository.Area.updateAreaName(areaIdentifier, newAreaName);[m
[31m-        setSettings({ ...settings, areaName: updatedAreaName });[m
[32m+[m[32m        const updatedSettings = { ...settings, areaName: updatedAreaName };[m
[32m+[m[32m        setSettings(updatedSettings);[m
         window.location.reload(); // reloads the sidebar...[m
     };[m
 [m
[36m@@ -84,7 +88,8 @@[m [mexport const AreaSettings = () => {[m
         if (newAreaDisplayTitle === settings.areaTitle) return;[m
         const updatedDisplayTitle = await repository.Area.updateDisplayTitle(areaIdentifier, newAreaDisplayTitle);[m
         window.location.reload();[m
[31m-        setSettings({ ...settings, areaTitle: updatedDisplayTitle });[m
[32m+[m[32m        const updatedSettings = { ...settings, areaTitle: updatedDisplayTitle };[m
[32m+[m[32m        setSettings(updatedSettings);[m
     };[m
 [m
     const handleAreaDelete = async () => {[m
[36m@@ -97,7 +102,10 @@[m [mexport const AreaSettings = () => {[m
         const emailVerification = await repository.Settings.EmailVerification.RequestEmailVerification(newEmailAddress, areaIdentifier);[m
         setAlertDetails({ title: emailVerification.title, message: emailVerification.message });[m
         setAlertState(true);[m
[31m-        if (!(emailVerification.status === "Failed")) setSettings({ ...settings, emailAddress: newEmailAddress });[m
[32m+[m[32m        if (!(emailVerification.status === "Failed")) {[m
[32m+[m[32m            const updatedSettings = { ...settings, emailAddress: newEmailAddress };[m
[32m+[m[32m            setSettings(updatedSettings);[m
[32m+[m[32m        }[m
     };[m
 [m
     const emailSeverity = (): "success" | "warning" | "error" | "info" | undefined => {[m
[36m@@ -178,7 +186,7 @@[m [mexport const AreaSettings = () => {[m
 [m
             <Grid container spacing={3} justify="center">[m
                 <Grid item xs={12}>[m
[31m-                    <Alert className={classNames(classes.alert, classes.alertTitle)} style={{backgroundColor: theme.palette.warning.dark}} variant="filled" severity="warning">[m
[32m+[m[32m                    <Alert className={classNames(classes.alert, classes.alertTitle)} style={{ backgroundColor: theme.palette.warning.dark }} variant="filled" severity="warning">[m
                         <AlertTitle>[m
                             <Typography variant="h5">Dashboard Specific Options</Typography>[m
                         </AlertTitle>[m
[1mdiff --git a/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThreshold.tsx b/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThreshold.tsx[m
[1mindex 1b9bf6c4..b1e40d8b 100644[m
[1m--- a/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThreshold.tsx[m
[1m+++ b/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThreshold.tsx[m
[36m@@ -77,7 +77,7 @@[m [mexport const BasicThreshold = ({ showDebug, tableId, tableTag, tableData, setTab[m
             />[m
             <TableContainer>[m
                 <Table>[m
[31m-                    <BasicThresholdHeader />[m
[32m+[m[32m                    <BasicThresholdHeader tableData={tableData} setTableData={setTableData} />[m
                     <BasicThresholdBody tableData={tableData} modifier={modifier} />[m
                 </Table>[m
             </TableContainer>[m
[1mdiff --git a/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThresholdHeader.tsx b/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThresholdHeader.tsx[m
[1mindex 8c19a66e..1cea741a 100644[m
[1m--- a/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThresholdHeader.tsx[m
[1m+++ b/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThresholdHeader.tsx[m
[36m@@ -1,6 +1,8 @@[m
 import React from "react";[m
[31m-import { TableHead, TableRow, TableCell, makeStyles } from "@material-ui/core";[m
[32m+[m[32mimport { TableHead, TableRow, TableCell, makeStyles, Button } from "@material-ui/core";[m
 import classNames from "classnames";[m
[32m+[m[32mimport { SetState, TableData } from "@Palavyr-Types";[m
[32m+[m[32mimport { reOrderBasicThresholdTableData } from "./BasicThresholdUtils";[m
 [m
 const useStyles = makeStyles((theme) => ({[m
     cell: {[m
[36m@@ -18,7 +20,12 @@[m [mconst useStyles = makeStyles((theme) => ({[m
     },[m
 }));[m
 [m
[31m-export const BasicThresholdHeader = () => {[m
[32m+[m[32mexport interface IBasicThresholdHeader {[m
[32m+[m[32m    tableData: TableData;[m
[32m+[m[32m    setTableData: SetState<TableData>;[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mexport const BasicThresholdHeader = ({ tableData, setTableData }: IBasicThresholdHeader) => {[m
     const cls = useStyles();[m
 [m
     return ([m
[36m@@ -35,6 +42,16 @@[m [mexport const BasicThresholdHeader = () => {[m
                     Max Amount (if range)[m
                 </TableCell>[m
                 <TableCell align="center"></TableCell>[m
[32m+[m[32m                <TableCell align="center">[m
[32m+[m[32m                    <Button[m
[32m+[m[32m                        onClick={() => {[m
[32m+[m[32m                            const sortedTableData = reOrderBasicThresholdTableData(tableData);[m
[32m+[m[32m                            setTableData(sortedTableData);[m
[32m+[m[32m                        }}[m
[32m+[m[32m                    >[m
[32m+[m[32m                        Reorder[m
[32m+[m[32m                    </Button>[m
[32m+[m[32m                </TableCell>[m
             </TableRow>[m
         </TableHead>[m
     );[m
[1mdiff --git a/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThresholdModifier.ts b/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThresholdModifier.ts[m
[1mindex ea72e58d..07448f9f 100644[m
[1m--- a/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThresholdModifier.ts[m
[1m+++ b/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThresholdModifier.ts[m
[36m@@ -77,6 +77,7 @@[m [mexport class BasicThresholdModifier {[m
     }[m
 [m
     public validateTable(tableData: BasicThresholdData[]) {[m
[32m+[m
         return true; // TODO: Validate this table[m
     }[m
 }[m
[1mdiff --git a/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThresholdUtils.ts b/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThresholdUtils.ts[m
[1mindex eb7752a1..15486ea4 100644[m
[1m--- a/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThresholdUtils.ts[m
[1m+++ b/frontend/src/dashboard/content/responseConfiguration/response/tables/dynamicTable/tableComponents/BasicThreshold/BasicThresholdUtils.ts[m
[36m@@ -6,9 +6,20 @@[m [mexport const reOrderBasicThresholdTableData = (tableData: any) => {[m
     const sortedByThreshold = sortByPropertyNumeric(getter, tableData);[m
 [m
     const reOrdered: BasicThresholdData[] = [];[m
[31m-    sortedByThreshold.forEach((row: BasicThresholdData, index: number) => {[m
[31m-        row.rowOrder = index;[m
[32m+[m[32m    let shouldReassignTriggerFallback = false;[m
[32m+[m[32m    sortedByThreshold.forEach((row: BasicThresholdData, newRowNumber: number) => {[m
[32m+[m[32m        console.log("STARTS FROM " + newRowNumber);[m
[32m+[m
[32m+[m[32m        row.rowOrder = newRowNumber;[m
[32m+[m[32m        if (newRowNumber + 1 !== sortedByThreshold.length && row.triggerFallback) {[m
[32m+[m[32m            row.triggerFallback = false;[m
[32m+[m[32m            shouldReassignTriggerFallback = true;[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        if (newRowNumber + 1 === sortedByThreshold.length && shouldReassignTriggerFallback) {[m
[32m+[m[32m            row.triggerFallback = true;[m
[32m+[m[32m        }[m
         reOrdered.push(row);[m
     });[m
     return reOrdered;[m
[31m-}[m
[32m+[m[32m};[m
[1mdiff --git a/frontend/src/dashboard/content/responseConfiguration/uploadable/emailTemplates/EmailConfiguration.tsx b/frontend/src/dashboard/content/responseConfiguration/uploadable/emailTemplates/EmailConfiguration.tsx[m
[1mindex 68ae8531..9cec5af4 100644[m
[1m--- a/frontend/src/dashboard/content/responseConfiguration/uploadable/emailTemplates/EmailConfiguration.tsx[m
[1m+++ b/frontend/src/dashboard/content/responseConfiguration/uploadable/emailTemplates/EmailConfiguration.tsx[m
[36m@@ -36,7 +36,8 @@[m [mexport const EmailConfiguration = () => {[m
     }, []);[m
 [m
     const loadSettings = useCallback(async () => {[m
[31m-        const areaData = await repository.Area.GetArea(areaIdentifier);[m
[32m+[m[32m        const areas = await repository.Area.GetAreas();[m
[32m+[m[32m        const areaData = areas.filter((x) => x.areaIdentifier === areaIdentifier)[0];[m
         setSettings({[m
             ...settings,[m
             useAreaFallbackEmail: areaData.useAreaFallbackEmail,[m
[36m@@ -56,7 +57,9 @@[m [mexport const EmailConfiguration = () => {[m
     return ([m
         <>[m
             <AreaConfigurationHeader title="Email Response" subtitle="Use this editor to create an HTML email template that will be sent as the email response for this area." />[m
[31m-            {useAreaFallbackEmail !== null && <OsTypeToggle controlledState={useAreaFallbackEmail} onChange={onUseAreaFallbackEmailToggle} enabledLabel="Use Area Fallback Email" disabledLabel="Use General Fallback Email" />}[m
[32m+[m[32m            {useAreaFallbackEmail !== null && ([m
[32m+[m[32m                <OsTypeToggle controlledState={useAreaFallbackEmail} onChange={onUseAreaFallbackEmailToggle} enabledLabel="Use Area Fallback Email" disabledLabel="Use General Fallback Email" />[m
[32m+[m[32m            )}[m
             {variableDetails && ([m
                 <EmailConfigurationComponent[m
                     variableDetails={variableDetails}[m
[1mdiff --git a/frontend/src/dashboard/content/subscribe/Subscribe.tsx b/frontend/src/dashboard/content/subscribe/Subscribe.tsx[m
[1mindex b40fb781..266d1afe 100644[m
[1m--- a/frontend/src/dashboard/content/subscribe/Subscribe.tsx[m
[1m+++ b/frontend/src/dashboard/content/subscribe/Subscribe.tsx[m
[36m@@ -36,7 +36,6 @@[m [mconst useStyles = makeStyles((theme) => ({[m
 }));[m
 [m
 export const Subscribe = () => {[m
[31m-    const [currentPlan, setCurrentPlan] = useState<PlanStatus | null>(null);[m
     const [productList, setProductList] = useState<ProductIds>();[m
     const { planTypeMeta } = useContext(DashboardContext);[m
 [m
[36m@@ -56,11 +55,6 @@[m [mexport const Subscribe = () => {[m
         history.push(purchaseRoute);[m
     };[m
 [m
[31m-    const getCurrentPlan = useCallback(async () => {[m
[31m-        const plan = await repository.Settings.Account.getCurrentPlan();[m
[31m-        setCurrentPlan(plan);[m
[31m-    }, []);[m
[31m-[m
     const getProducts = useCallback(async () => {[m
         const products = await repository.Products.getProducts();[m
         setProductList(products);[m
[36m@@ -68,7 +62,6 @@[m [mexport const Subscribe = () => {[m
 [m
     useEffect(() => {[m
         getProducts();[m
[31m-        getCurrentPlan();[m
     }, []);[m
 [m
     const orderedProductOptions: ProductOptions = [[m
[36m@@ -96,7 +89,7 @@[m [mexport const Subscribe = () => {[m
         <>[m
             <AreaConfigurationHeader title="Select a subscription plan" subtitle="You won't be charged yet." divider />[m
             <SubscribeStepper activeStep={0} />[m
[31m-            {currentPlan !== null && ([m
[32m+[m[32m            {planTypeMeta !== null && ([m
                 <div className={cls.body}>[m
                     <Grid container>[m
                         <Grid item xs={12}>[m
[36m@@ -104,7 +97,7 @@[m [mexport const Subscribe = () => {[m
                                 {planTypeMeta &&[m
                                     orderedProductOptions.map((product: ProductOption, key: number) => {[m
                                         return ([m
[31m-                                            <div onClick={() => (planTypeMeta && goToPurchase(product.purchaseType, product.productId))} className={classnames(cls.width, cls.card)}>[m
[32m+[m[32m                                            <div onClick={() => planTypeMeta && goToPurchase(product.purchaseType, product.productId)} className={classnames(cls.width, cls.card)}>[m
                                                 {product.card}[m
                                             </div>[m
                                         );[m
[1mdiff --git a/frontend/src/dashboard/layouts/DashboardLayout.tsx b/frontend/src/dashboard/layouts/DashboardLayout.tsx[m
[1mindex 9cee1ee7..b0fc96ad 100644[m
[1m--- a/frontend/src/dashboard/layouts/DashboardLayout.tsx[m
[1m+++ b/frontend/src/dashboard/layouts/DashboardLayout.tsx[m
[36m@@ -80,7 +80,6 @@[m [minterface IDashboardLayout {[m
 export const DashboardLayout = ({ helpComponent, children }: IDashboardLayout) => {[m
     const repository = new PalavyrRepository();[m
     const history = useHistory();[m
[31m-    redirectToHomeWhenSessionNotEstablished(history);[m
 [m
     const { areaIdentifier } = useParams<{ contentType: string; areaIdentifier: string }>();[m
 [m
[36m@@ -116,40 +115,25 @@[m [mexport const DashboardLayout = ({ helpComponent, children }: IDashboardLayout) =[m
 [m
     const [unseenNotifications, setUnseenNotifications] = useState<number>(0);[m
 [m
[32m+[m[32m    useEffect(() => {[m
[32m+[m[32m        (async () => {[m
[32m+[m[32m            await redirectToHomeWhenSessionNotEstablished(history, repository);[m
[32m+[m[32m        })();[m
[32m+[m[32m    }, []);[m
[32m+[m
     const loadAreas = useCallback(async () => {[m
         setDashboardAreasLoading(true);[m
 [m
         const planTypeMeta = await repository.Settings.Subscriptions.getCurrentPlanMeta();[m
         setPlanTypeMeta(planTypeMeta);[m
 [m
[31m-        const cachedAreas = SessionStorage.getAreas();[m
[31m-        let areas: Areas;[m
[31m-        if (cachedAreas) {[m
[31m-            areas = cachedAreas;[m
[31m-        } else {[m
[31m-            areas = await repository.Area.GetAreas();[m
[31m-            SessionStorage.setAreas(areas);[m
[31m-        }[m
[32m+[m[32m        const areas = await repository.Area.GetAreas();[m
         setAreaNameDetails(sortByPropertyAlphabetical((x: AreaNameDetail) => x.areaName, fetchSidebarInfo(areas)));[m
 [m
[31m-        const cachedLocale = SessionStorage.getLocale();[m
[31m-        let locale: LocaleDefinition;[m
[31m-        if (cachedLocale) {[m
[31m-            locale = cachedLocale;[m
[31m-        } else {[m
[31m-            locale = await repository.Settings.Account.GetLocale();[m
[31m-            SessionStorage.setLocale(locale);[m
[31m-        }[m
[32m+[m[32m        const locale = await repository.Settings.Account.GetLocale();[m
         setCurrencySymbol(locale.localeCurrencySymbol);[m
 [m
[31m-        const cachedNeedsPassword = SessionStorage.getNeedsPassword();[m
[31m-        let needsPassword: boolean;[m
[31m-        if (cachedNeedsPassword) {[m
[31m-            needsPassword = cachedNeedsPassword;[m
[31m-        } else {[m
[31m-            needsPassword = await repository.Settings.Account.CheckNeedsPassword();[m
[31m-            SessionStorage.setNeedsPassword(needsPassword);[m
[31m-        }[m
[32m+[m[32m        const needsPassword = await repository.Settings.Account.CheckNeedsPassword();[m
         setAccountTypeNeedsPassword(needsPassword);[m
 [m
         const enqs = await repository.Enquiries.getEnquiries();[m
[1mdiff --git a/frontend/src/landing/login/LoginDialog.tsx b/frontend/src/landing/login/LoginDialog.tsx[m
[1mindex 41bf2657..8e6ab835 100644[m
[1m--- a/frontend/src/landing/login/LoginDialog.tsx[m
[1m+++ b/frontend/src/landing/login/LoginDialog.tsx[m
[36m@@ -63,7 +63,7 @@[m [mexport const LoginDialog = ({ status, setStatus, onClose, openChangePasswordDial[m
         }[m
     }, [])[m
 [m
[31m-    const success = () => {[m
[32m+[m[32m    const successRedirectToDashboard = () => {[m
         setTimeout(() => {[m
             setIsLoading(false);[m
             history.push(DASHBOARD_HOME);[m
[36m@@ -98,7 +98,7 @@[m [mexport const LoginDialog = ({ status, setStatus, onClose, openChangePasswordDial[m
         setStatus(null);[m
 [m
         if (loginEmail && loginPassword) {[m
[31m-            const successfulResponse = await Auth.login(loginEmail, loginPassword, success, error);[m
[32m+[m[32m            const successfulResponse = await Auth.login(loginEmail, loginPassword, successRedirectToDashboard, error);[m
             SessionStorage.setDefaultLoginType();[m
             setIsLoading(false);[m
             if (successfulResponse === null) {[m
[36m@@ -122,7 +122,7 @@[m [mexport const LoginDialog = ({ status, setStatus, onClose, openChangePasswordDial[m
     const googleLogin = async (response: GoogleResponse) => {[m
         setIsLoading(true);[m
         setStatus(null);[m
[31m-        var successfulResponse = await Auth.loginWithGoogle(response.tokenId, response.googleId, success, googleError);[m
[32m+[m[32m        var successfulResponse = await Auth.loginWithGoogle(response.tokenId, response.googleId, successRedirectToDashboard, googleError);[m
         if (successfulResponse === null) {[m
             Auth.ClearAuthentication();[m
             Auth.googleLogout(noop);[m
[1mdiff --git a/frontend/src/localStorage/sessionStorage.ts b/frontend/src/localStorage/sessionStorage.ts[m
[1mindex 96c849da..41e6e375 100644[m
[1m--- a/frontend/src/localStorage/sessionStorage.ts[m
[1m+++ b/frontend/src/localStorage/sessionStorage.ts[m
[36m@@ -229,6 +229,17 @@[m [mclass SessionStorageAccess {[m
         this._setItem(this.Enquiries, "");[m
     }[m
 [m
[32m+[m[32m    setCacheValue(key: string, value: any) {[m
[32m+[m[32m        this._setItem(key, JSON.stringify(value));[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    getCacheValue(key: string) {[m
[32m+[m[32m        return this.getStoredJson(key);[m
[32m+[m[32m    }[m
[32m+[m[32m    clearCacheValue(key: string) {[m
[32m+[m[32m        this._setItem(key, "");[m
[32m+[m[32m    }[m
[32m+[m
     private getStoredBoolean(key: string) {[m
         const value = this._getItem(key);[m
         if (value) {[m
[1mdiff --git a/server/Palavyr.API/Controllers/HomeController.cs b/server/Palavyr.API/Controllers/HomeController.cs[m
[1mindex 46de4efe..4252c557 100644[m
[1m--- a/server/Palavyr.API/Controllers/HomeController.cs[m
[1m+++ b/server/Palavyr.API/Controllers/HomeController.cs[m
[36m@@ -1,5 +1,4 @@[m
[31m-﻿using Microsoft.AspNetCore.Authorization;[m
[31m-using Microsoft.AspNetCore.Mvc;[m
[32m+[m[32m﻿using Microsoft.AspNetCore.Mvc;[m
 [m
 namespace Palavyr.API.Controllers[m
 {[m
[36m@@ -9,7 +8,6 @@[m [mnamespace Palavyr.API.Controllers[m
         {[m
         }[m
 [m
[31m-        [AllowAnonymous][m
         [HttpGet()][m
         public string Get()[m
         {[m
[1mdiff --git a/server/Palavyr.Core/Models/Resources/Responses/ResponseCustomizer.cs b/server/Palavyr.Core/Models/Resources/Responses/ResponseCustomizer.cs[m
[1mindex bbc0a7ef..271a965c 100644[m
[1m--- a/server/Palavyr.Core/Models/Resources/Responses/ResponseCustomizer.cs[m
[1m+++ b/server/Palavyr.Core/Models/Resources/Responses/ResponseCustomizer.cs[m
[36m@@ -56,7 +56,7 @@[m [mnamespace Palavyr.Core.Models.Resources.Responses[m
         {[m
             if (string.IsNullOrWhiteSpace(account.AccountLogoUri))[m
             {[m
[31m-                return html;[m
[32m+[m[32m                return html.Replace(ResponseVariableDefinition.LogoUriVariablePattern, string.Empty);[m
             }[m
             else[m
             {[m
[1mdiff --git a/widget/src/componentRegistry/standardComponentRegistry.tsx b/widget/src/componentRegistry/standardComponentRegistry.tsx[m
[1mindex 60768757..c9ece16b 100644[m
[1m--- a/widget/src/componentRegistry/standardComponentRegistry.tsx[m
[1m+++ b/widget/src/componentRegistry/standardComponentRegistry.tsx[m
[36m@@ -1,4 +1,4 @@[m
[31m-import { assembleCompletedConvo, getOrderedChildNodes } from "./utils";[m
[32m+[m[32mimport { assembleCompletedConvo, getOrderedChildNodes, MinNumeric, parseNumericResponse } from "./utils";[m
 import React, { useEffect, useState } from "react";[m
 import { Table, TableRow, TableCell, makeStyles, TextField, Typography } from "@material-ui/core";[m
 import { responseAction } from "./responseAction";[m
[36m@@ -10,7 +10,6 @@[m [mimport { ResponseButton } from "common/ResponseButton";[m
 import { SingleRowSingleCell } from "common/TableCell";[m
 import { splitValueOptionsByDelimiter } from "widget/utils/valueOptionSplitter";[m
 import { ChatLoadingSpinner } from "common/UserDetailsDialog/ChatLoadingSpinner";[m
[31m-import { uuid } from "uuidv4";[m
 import { CustomImage } from "common/CustomImage";[m
 import { floor, max, min } from "lodash";[m
 [m
[36m@@ -45,16 +44,7 @@[m [mconst useStyles = makeStyles(theme => ({[m
     },[m
 }));[m
 [m
[31m-export class ComponentRegisteryMethods {[m
[31m-    MinNumeric: number = 0;[m
[31m-[m
[31m-    public parseNumericResponse(newValue: string): string {[m
[31m-        const intValue = parseInt(newValue);[m
[31m-        return intValue < this.MinNumeric ? this.MinNumeric.toString() : intValue.toString();[m
[31m-    }[m
[31m-}[m
[31m-[m
[31m-export class StandardComponents extends ComponentRegisteryMethods {[m
[32m+[m[32mexport class StandardComponents {[m
     public makeProvideInfo({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {[m
         const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];[m
         const prefs = getWidgetPreferences();[m
[36m@@ -194,7 +184,7 @@[m [mexport class StandardComponents extends ComponentRegisteryMethods {[m
                                 label=""[m
                                 type="number"[m
                                 onChange={event => {[m
[31m-                                    setResponse(this.parseNumericResponse(event.target.value));[m
[32m+[m[32m                                    setResponse(parseNumericResponse(event.target.value));[m
                                     setDisabled(false);[m
                                 }}[m
                             />[m
[36m@@ -410,8 +400,8 @@[m [mexport class StandardComponents extends ComponentRegisteryMethods {[m
                                     setDisabled(false);[m
                                     const intValue = parseInt(event.target.value);[m
                                     if (!intValue) return;[m
[31m-                                    if (intValue < this.MinNumeric) {[m
[31m-                                        setResponse(this.MinNumeric);[m
[32m+[m[32m                                    if (intValue < MinNumeric) {[m
[32m+[m[32m                                        setResponse(MinNumeric);[m
                                     } else {[m
                                         setResponse(intValue);[m
                                     }[m
[1mdiff --git a/widget/src/componentRegistry/utils.ts b/widget/src/componentRegistry/utils.ts[m
[1mindex 6228ff93..b861b6e7 100644[m
[1m--- a/widget/src/componentRegistry/utils.ts[m
[1m+++ b/widget/src/componentRegistry/utils.ts[m
[36m@@ -1,5 +1,7 @@[m
 import { CompleteConverationDetails, WidgetNodeResource, WidgetNodes } from "@Palavyr-Types";[m
 [m
[32m+[m[32mexport const MinNumeric: number = 0;[m
[32m+[m
 export const getRootNode = (nodeList: WidgetNodes): WidgetNodeResource => {[m
     var node = nodeList.filter(x => x.isRoot === true)[0];[m
     return node;[m
[36m@@ -9,9 +11,9 @@[m [mexport const getOrderedChildNodes = (childrenIDs: string, nodeList: WidgetNodes)[m
     const ids = childrenIDs.split(",");[m
     const children: WidgetNodes = [];[m
     ids.forEach((id: string) => {[m
[31m-        const node = nodeList.filter((node) => node.nodeId === id)[0] // each ID should only refer to 1 existing node.[m
[31m-        children.push(node)[m
[31m-    })[m
[32m+[m[32m        const node = nodeList.filter(node => node.nodeId === id)[0]; // each ID should only refer to 1 existing node.[m
[32m+[m[32m        children.push(node);[m
[32m+[m[32m    });[m
     return children;[m
 };[m
 [m
[36m@@ -22,7 +24,7 @@[m [mexport const assembleCompletedConvo = (conversationId: string, areaIdentifier: s[m
         Name: name,[m
         Email: email,[m
         PhoneNumber: PhoneNumber,[m
[31m-        HasResponse: hasResponse[m
[32m+[m[32m        HasResponse: hasResponse,[m
     };[m
 };[m
 [m
[36m@@ -32,3 +34,8 @@[m [mexport const extractDynamicTypeGuid = (dynamicType: string) => {[m
     const guid = guidParts.join("-");[m
     return guid;[m
 };[m
[32m+[m
[32m+[m[32mexport const parseNumericResponse = (newValue: string): string => {[m
[32m+[m[32m    const intValue = parseInt(newValue);[m
[32m+[m[32m    return intValue < MinNumeric ? MinNumeric.toString() : intValue.toString();[m
[32m+[m[32m};[m
