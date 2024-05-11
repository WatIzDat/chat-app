"use client";

import Message from "@/app/ui/message";
import { useEffect, useRef } from "react";

export default function DiscussionContents() {
    const bottomRef = useRef<HTMLDivElement | null>(null);

    const scrollToBottom = () => {
        bottomRef.current?.scrollIntoView({ behavior: "smooth" });
    };

    useEffect(() => {
        scrollToBottom();
    }, []);

    return (
        <div
            className="flex flex-col justify-center items-center
                       w-[calc(100vw-theme('spacing.32'))] h-full relative
                       mb-8 mt-12 p-8
                       bg-zinc-900 rounded-3xl"
        >
            <div className="w-full h-full px-8 pb-8 overflow-y-auto absolute">
                <Message
                    username="Username"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <Message
                    username="test123"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <Message
                    username="another test"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <Message
                    username="another test"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <Message
                    username="another test"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <Message
                    username="another test"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <Message
                    username="another test"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <Message
                    username="another test"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <Message
                    username="another test"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <Message
                    username="another test"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <Message
                    username="another test"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <Message
                    username="another test"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <Message
                    username="another test"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <Message
                    username="another test"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <Message
                    username="another test"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <Message
                    username="hello"
                    contents="Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Quis recusandae cumque consectetur fuga asperiores magni
                    nulla harum? Blanditiis, quod repudiandae. Ipsam, nulla.
                    Qui, velit eos omnis itaque nam reiciendis commodi?"
                />

                <div ref={bottomRef} />
            </div>
        </div>
    );
}
