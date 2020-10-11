declare module '@unicef/material-ui-currency-textfield' {
    const noTypesYet: any;
    export default noTypesYet;
}

// declare module '*.svg' {
//     import * as React from 'react';

//     export const ReactComponent: React.FunctionComponent<React.SVGProps<
//         SVGSVGElement
//     > & { title?: string }>;

//     const src: string;
//     export default src;
// }


// declare module "\*.svg" {
//     import React = require("react");
//     export const ReactComponent: React.SFC<React.SVGProps<SVGSVGElement>>;
//     const src: string;
//     export default src;
// }


//https://github.com/gregberge/svgr/issues/38
interface SvgrComponent
  extends React.StatelessComponent<React.SVGAttributes<SVGElement>> {}

declare module "*.svg" {
  const ReactComponent: SvgrComponent;

  export { ReactComponent };
}