import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { TermsOfServiceDialog, ITermsOfServiceDialog } from './TermsOfService';


export default {
    title: "Landing/TermsOfServiceDialog",
    component: TermsOfServiceDialog
} as Meta;


const Template = (args: ITermsOfServiceDialog) => <TermsOfServiceDialog {...args} />;
export const Primary = Template.bind({});
Primary.args = {
    onClose: () => {}
}
