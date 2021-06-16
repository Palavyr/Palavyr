import { Selector, ParsedAnchor } from "./LineTypes";
export default () => null;
// export const defaultAnchor = { x: 0.5, y: 0.5 };
// export const defaultBorderColor = "#f00";
// export const defaultBorderStyle = "solid";
// export const defaultBorderWidth = 1;

// export const parseAnchor = (value: string) => {
//     switch (value) {
//         case "top":
//             return { x: 0.5, y: 0 };
//         case "left":
//             return { x: 0, y: 0.5 };
//         case "middle":
//             return { x: 0.5, y: 0.5 };
//         case "center":
//             return { x: 0.5, y: 0.5 };
//         case "bottom":
//             return { x: 0.5, y: 1 };
//         case "right":
//             return { x: 1, y: 0.5 };
//         default:
//             throw new Error("Must provide compatible anchor position.");
//     }
// };


// export const findElementById = (id: string) => {
//     return document.getElementById(id);
// };

// export const findElementByClassName = (className: string) => {
//     return document.getElementsByClassName(className)[0];
// };

// export const getPoints = (from: Selector, to: Selector, parsedFromAnchor: ParsedAnchor, parsedToAnchor: ParsedAnchor) => {
//     const a = findElementByClassName(from);
//     const b = findElementByClassName(to);

//     if (!a || !b) {
//         return false;
//     }

//     const box0 = a.getBoundingClientRect();
//     const box1 = b.getBoundingClientRect();

//     let offsetX = window.pageXOffset;
//     let offsetY = window.pageYOffset;

//     const p = findElementByClassName("tree-item");
//     const boxp = p.getBoundingClientRect();

//     offsetX -= boxp.left + (window.pageXOffset || document.documentElement.scrollLeft) - p.scrollLeft;
//     offsetY -= boxp.top + (window.pageYOffset || document.documentElement.scrollTop) - p.scrollTop;

//     const x0 = box0.left + box0.width * parsedFromAnchor.x + offsetX;
//     const x1 = box1.left + box1.width * parsedToAnchor.x + offsetX;
//     const y0 = box0.top + box0.height * parsedFromAnchor.y + offsetY;
//     const y1 = box1.top + box1.height * parsedToAnchor.y + offsetY;
//     return { x0, y0, x1, y1 };
// };
