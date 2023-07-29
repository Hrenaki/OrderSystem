export interface ModalProps {
    children: JSX.Element,
    className: string,
    isShown: boolean,
    onClose: () => void
}

export default function Modal(props: ModalProps) {
    return (
        <div className={props.className} hidden={!props.isShown}>
            {props.children}
        </div>
    )
};