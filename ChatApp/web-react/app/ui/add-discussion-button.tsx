import { PlusIcon } from "@heroicons/react/24/outline";
import { MouseEventHandler } from "react";

export default function AddDiscussionButton({
    onClick,
}: {
    onClick: MouseEventHandler<HTMLButtonElement>;
}) {
    return (
        <button
            className="flex items-center justify-center size-80 bg-zinc-900 rounded-3xl"
            onClick={onClick}
        >
            <PlusIcon className="size-1/3" />
        </button>
    );
}
