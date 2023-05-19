import * as React from 'react';
import { Story, Meta } from '@storybook/react';
import { ZoomImage, ZoomImageProps } from './ZoomImage';


export default {
    title: "Common/Borrowed/ZoomImage",
    component: ZoomImage
} as Meta;


const Template = (args: ZoomImageProps) => <div style={{height: "30%", width: "30%"}}><ZoomImage {...args} /></div>;

export const Primary = Template.bind({});
Primary.args = {
    src: "https://images.freeimages.com/images/large-previews/20c/my-puppy-maggie-1362787.jpg",
    alt: "No image",
    className: ""

}

export const NoImage = Template.bind({});
NoImage.args = {
    src: "",
    alt: "No image",
    className: ""

}
