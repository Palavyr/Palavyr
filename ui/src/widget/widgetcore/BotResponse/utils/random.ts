const INFINITY = 1 / 0
const MAX_INTEGER = 1.7976931348623157e+308


export const random = (lower: number | undefined, upper: number | undefined, floating: boolean) => {
    if (floating === undefined) {
        if (typeof upper === "boolean") {
            floating = upper;
            upper = undefined;
        } else if (typeof lower === "boolean") {
            floating = lower;
            lower = undefined;
        }
    }
    if (lower === undefined && upper === undefined) {
        lower = 0;
        upper = 1;
    } else {
        lower = toFinite(lower!);
        if (upper === undefined) {
            upper = lower;
            lower = 0;
        } else {
            upper = toFinite(upper);
        }
    }
    if (lower > upper) {
        const temp = lower;
        lower = upper;
        upper = temp;
    }
    if (floating || lower % 1 || upper % 1) {
        const rand = Math.random();
        const randLength = `${rand}`.length - 1;
        return Math.min(lower + rand * (upper - lower + freeParseFloat(`1e-${randLength}`)), upper);
    }
    return lower + Math.floor(Math.random() * (upper - lower + 1));
}

function toFinite(value: number) {
    if (!value) {
        return value === 0 ? value : 0;
    }
    value = toNumber(value);
    if (value === INFINITY || value === -INFINITY) {
        const sign = value < 0 ? -1 : 1;
        return sign * MAX_INTEGER;
    }
    // eslint-disable-next-line no-self-compare
    return value === value ? value : 0;
}

function toNumber(value: string | number) {
    if (typeof value === "number") {
        return value;
    }
    if (isSymbol(value)) {
        return NAN;
    }
    if (isObject(value)) {
        const other = typeof value.valueOf === "function" ? value.valueOf() : value;
        value = isObject(other) ? `${other}` : other;
    }
    if (typeof value !== "string") {
        return value === 0 ? value : +value;
    }
    value = value.replace(reTrim, "");
    const isBinary = reIsBinary.test(value);
    return isBinary || reIsOctal.test(value) ? freeParseInt(value.slice(2), isBinary ? 2 : 8) : reIsBadHex.test(value) ? NAN : +value;
}

const NAN = 0 / 0
const freeParseFloat = parseFloat
const reTrim = /^\s+|\s+$/g
const reIsBadHex = /^[-+]0x[0-9a-f]+$/i
const reIsBinary = /^0b[01]+$/i
const reIsOctal = /^0o[0-7]+$/i
const freeParseInt = parseInt

function isObject(value: string) {
  const type = typeof value
  return value != null && (type === 'object' || type === 'function')
}

function isSymbol(value: string) {
  const type = typeof value
  return type === 'symbol' || (type === 'object' && value != null && getTag(value) === '[object Symbol]')
}

const toString = Object.prototype.toString

function getTag(value: any) {
  if (value == null) {
    return value === undefined ? '[object Undefined]' : '[object Null]'
  }
  return toString.call(value)
}