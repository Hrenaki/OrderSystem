import 'bootstrap/dist/css/bootstrap.css';
import { useState } from 'react';

interface MultiSelectListItemProps {
    option: SelectListOption,
    onSelect: () => void
};

function MultiSelectListItem(props: MultiSelectListItemProps) {
    return (
        <div className="list-group-item" onChange={props.onSelect}>
            <input className="form-check-input me-2" type="checkbox"/>
            <input type="hidden" value={props.option.key}/>
            {props.option.value}
        </div>
    );
};

export interface SelectListOption {
    key: string,
    value: string
}

export interface MultiSelectListProps {
    options: SelectListOption[],
    onSelect: (option: SelectListOption) => void
};

function MultiSelectList(props: MultiSelectListProps) {
    return (
        <ul className="list-group text-start mb-3 border border-secondary">
            {props.options.map(o => <MultiSelectListItem key={o.key} option={o} onSelect={() => props.onSelect(o)} />)}
        </ul>
    );
};

export default MultiSelectList;