// src/helpers/videoPlayerHelpers.ts
import { useEffect, useRef, useState } from 'react';

export interface UseYouTubePlayerTimeResult {
    playerRef: React.MutableRefObject<any | null>;
    currentTimeMs: number;
    handlePlayerReady: (event: any) => void;
    handlePlayerStateChange: (event: any) => void;
}

/**
 * Small hook that wires a react-youtube player to a "currentTimeMs" state.
 * It starts polling when the video is playing and stops when paused/ended.
 */
export function useYouTubePlayerTime(pollIntervalMs: number = 200): UseYouTubePlayerTimeResult {
    const playerRef = useRef<any | null>(null);
    const [currentTimeMs, setCurrentTimeMs] = useState(0);
    const timeUpdateIntervalRef = useRef<number | null>(null);

    const startTimeTracking = () => {
        if (timeUpdateIntervalRef.current !== null) return;

        timeUpdateIntervalRef.current = window.setInterval(() => {
            if (playerRef.current && typeof playerRef.current.getCurrentTime === 'function') {
                const seconds: number = playerRef.current.getCurrentTime();
                setCurrentTimeMs(seconds * 1000);
            }
        }, pollIntervalMs);
    };

    const stopTimeTracking = () => {
        if (timeUpdateIntervalRef.current !== null) {
            window.clearInterval(timeUpdateIntervalRef.current);
            timeUpdateIntervalRef.current = null;
        }
    };

    useEffect(() => {
        return () => {
            stopTimeTracking();
        };
    }, []);

    const handlePlayerReady = (event: any) => {
        playerRef.current = event.target;
    };

    const handlePlayerStateChange = (event: any) => {
        const state = event.data; // 0 ended, 1 playing, 2 paused...
        if (state === 1) {
            startTimeTracking();
        } else if (state === 0 || state === 2) {
            stopTimeTracking();
        }
    };

    return {
        playerRef,
        currentTimeMs,
        handlePlayerReady,
        handlePlayerStateChange
    };
}

/**
 * Extract a YouTube video ID from several common URL formats.
 */
export function getYouTubeVideoId(url: string): string {
    const regExp = /^.*((youtu.be\/)|(v\/)|(\/u\/\w\/)|(embed\/)|(watch\?))\??v?=?([^#&?]*).*/;
    const match = url.match(regExp);
    return match && match[7] && match[7].length === 11 ? match[7] : '';
}
