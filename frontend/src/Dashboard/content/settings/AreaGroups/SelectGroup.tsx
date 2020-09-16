import { makeStyles, FormControl, InputLabel, Select, FormHelperText, Button, MenuItem, Divider } from "@material-ui/core";
import { GroupRow, Groups } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import React from "react";


const useStyles = makeStyles(() => ({
    formControl: {
        minWidth: 120,
        maxWidth: 185,
        width: "100%",
    },
}));

interface ISelectGroup {
    onChange: (event: React.ChangeEvent<{ name?: string | undefined; value: unknown }>) => void;
    option: string;
    groups: Array<GroupRow>;
    selectedGroupId: string;
    areaIdentifier: string;
    setModalState: (val: boolean) => void;
    setNodes: (val: Groups) => void;
}

export const SelectGroup = ({ onChange, option, groups, selectedGroupId, areaIdentifier, setModalState, setNodes }: ISelectGroup) => {
    var client = new ApiClient();

    const classes = useStyles();

    return (
        <div>
            <h3>{option}</h3>
            This is the option.
            <FormControl className={classes.formControl}>
                <InputLabel id="simple-select-helper-label">Group</InputLabel>
                <Select labelId="simple-select-helper-label" id="simple-select-helper" value={option} onChange={onChange}>
                    {
                        groups.map(m => <MenuItem value={m.groupId}>{m.groupName}</MenuItem>)
                    }
                </Select>
                <FormHelperText>Select the type of node</FormHelperText>
            </FormControl>
            <Divider />
            <Button
                color="secondary"
                variant="contained"
                onClick={(() => setModalState(false))}
            >
                Cancel
            </Button>
            <Button
                color="primary"
                variant="outlined"
                onClick={async () => {

                    var res = await client.Settings.Groups.UpdateAreaGroup(areaIdentifier, selectedGroupId);
                    setNodes(res.data);
                    setModalState(false);
                }}
            >
                Save
            </Button>
        </div>
    );
};
