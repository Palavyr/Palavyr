import React from "react";
import classNames from "classnames";

interface IContentLoader {
    classes: any;
    open: boolean;
    children: React.ReactNode;
}

export const ContentLoader = ({ classes, open, children }: IContentLoader) => {
    return (
        <main
            className={classNames(classes.content, {
                [classes.contentShift]: open,
            })}
        >
            <div className={classes.drawerHeader} />
            <div>{children}</div>
        </main>
    );
};
