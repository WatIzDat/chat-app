import { XMarkIcon } from "@heroicons/react/24/outline";
import { ReactNode } from "react";

export default function Modal({
    isVisible,
    onClose,
    children,
}: {
    isVisible: boolean;
    onClose: any;
    children: ReactNode;
}) {
    if (!isVisible) {
        return;
    }

    const handleClose = (event: any) => {
        if (event.target.id === "wrapper") {
            onClose();
        }
    };

    return (
        <div
            className="flex justify-center items-center fixed inset-0 bg-black bg-opacity-25 backdrop-blur-sm"
            id="wrapper"
            onClick={handleClose}
        >
            <div className="w-1/6 flex flex-col">
                <button className="size-6 place-self-end" onClick={onClose}>
                    <XMarkIcon />
                </button>
                <div className="bg-zinc-600 p-4 rounded-lg">{children}</div>
            </div>
        </div>
    );
}
