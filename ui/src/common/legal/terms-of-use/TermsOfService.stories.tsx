import * as React from 'react';
import { Story, Meta } from '@storybook/react';
import { TermsOfServiceDialog, ITermsOfServiceDialog } from './TermsOfServiceDialog';


export default {
    title: "Landing/TermsOfServiceDialog",
    component: TermsOfServiceDialog
} as Meta;


const Template = (args: ITermsOfServiceDialog) => <TermsOfServiceDialog {...args} />;
export const Primary = Template.bind({});
Primary.args = {
    onClose: () => {}
}
