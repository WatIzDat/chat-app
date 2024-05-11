"use client";

import { useState } from "react";
import { sendMessage } from "@/app/lib/actions";
import { ArrowRightCircleIcon } from "@heroicons/react/24/solid";

export default function SendMessageForm({
    discussionId,
}: {
    discussionId: string;
}) {
    const [state, setState] = useState({ contents: "" });

    const reset = (e) => {
        setState({ contents: "" });
    };

    const sendMessageWithDiscussionId = sendMessage.bind(null, discussionId);

    return (
        <form
            className="flex flex-row w-[calc(100vw-theme('spacing.32'))] mb-16 bg-zinc-900 rounded-3xl"
            action={sendMessageWithDiscussionId}
            onSubmit={reset}
        >
            <input
                className="bg-zinc-900 rounded-3xl flex-grow pl-4 pt-2 pb-2 pr-2"
                type="text"
                id="contents"
                name="contents"
                placeholder="Send a message..."
                value={state.contents}
                onChange={(e) => {
                    setState({ ...state, contents: e.target.value });
                }}
            />

            <button className="p-2 place-self-end" type="submit">
                <ArrowRightCircleIcon className="size-12 text-zinc-500" />
            </button>
        </form>
    );
}
