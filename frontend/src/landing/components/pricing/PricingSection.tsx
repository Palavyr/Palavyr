import React from 'react'
import { PremiumCard } from './PremiumCard';
import { FreeCard } from './FreeCard';
import { ProCard } from './ProCard';
import { Paper } from '@material-ui/core';
import classNames from 'classnames';
import { pricingContainerStyles } from './cardStyles';


export const PricingSection = () => {
    const cls = pricingContainerStyles();

    return (
        <section className={cls.body}>

            <Paper data-aos="fade-down" data-aos-delay="100" className={classNames(cls.paperCommon, cls.paperFree)} variant="outlined">
                <FreeCard />
            </Paper>
            <Paper data-aos="fade-down" data-aos-delay="100" className={classNames(cls.paperCommon, cls.paperPremium)} variant="outlined">
                <PremiumCard border />
            </Paper>
            <Paper data-aos="fade-down" data-aos-delay="100" className={classNames(cls.paperCommon, cls.paperPro)} variant="outlined">
                <ProCard />
            </Paper>
        </section >
    )
}